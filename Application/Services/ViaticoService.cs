using Application.DTO.ArchivoDTO;
using Application.DTO.ViaticoDTO;
using Application.Enums.Viatico;
using Application.Exceptions;
using Application.Helpers;
using Application.Interfaces.IArchivo;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.IVehiculo;
using Application.Interfaces.IViatico;
using Domain.Common;
using Domain.Entities.Viaticos;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ViaticoService : IViaticoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IViaticoRepository _viaticoRepository;
        private readonly ISolicitudViaticoRepository _solicitudRepository;
        private readonly IArchivoRepository _archivoRepository;
        private readonly IProveedorViaticoRepository _proveedorRepository;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IDomainEventDispatcher _eventDispatcher;

        public ViaticoService(
            IUnitOfWork unitOfWork,
            IViaticoRepository viaticoRepository,
            ISolicitudViaticoRepository solicitudRepository,
            IArchivoRepository archivoRepository,
            IProveedorViaticoRepository proveedorRepository,
            IVehiculoRepository vehiculoRepository,
            IDomainEventDispatcher eventDispatcher)
        {
            _unitOfWork = unitOfWork;
            _viaticoRepository = viaticoRepository;
            _solicitudRepository = solicitudRepository;
            _archivoRepository = archivoRepository;
            _proveedorRepository = proveedorRepository;
            _vehiculoRepository = vehiculoRepository;
            _eventDispatcher=eventDispatcher;
        }

        public async Task<int> CrearViaticoAsync(ViaticoCrearDTO dto, string webRootPath)
        {
            var moverArchivoDto = new MoverArchivoDTO
            {
                RutaRelativaTemporal = dto.Factura.RutaTemporal,
                UsuarioAppId = dto.UsuarioAppId,
                CicloId = dto.CicloId,
                FechaFactura = dto.Factura.FechaFactura,
                PrefijoNombre = "factura"
            };

            var rutaFinal = await _archivoRepository.MoverArchivoFinalAsync(moverArchivoDto, webRootPath);
            var rutaAbsoluta = Path.Combine(webRootPath, rutaFinal);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (!await _proveedorRepository.ExistePorRucAsync(dto.Factura.RucProveedor))
                {
                    var proveedor = new ProveedorViatico
                    {
                        Ruc = dto.Factura.RucProveedor,
                        RazonSocial = dto.Factura.ProveedorNombre
                    };
                    await _proveedorRepository.CrearAsync(proveedor);
                }

                var solicitud = await _solicitudRepository.ObtenerPorCicloUsuarioAsync(dto.CicloId, dto.UsuarioAppId);
                if (solicitud == null)
                {
                    solicitud = new SolicitudViatico
                    {
                        CicloId = dto.CicloId,
                        UsuarioAppId = dto.UsuarioAppId,
                        Estado = EstadoSolicitud.Borrador,
                        Monto = 0
                    };

                    await _solicitudRepository.CrearAsync(solicitud);
                }

                var factura = new FacturaViatico
                {
                    NumeroFactura = dto.Factura.NumeroFactura,
                    FechaFactura = DateTime.SpecifyKind(dto.Factura.FechaFactura, DateTimeKind.Unspecified),
                    RucProveedor = dto.Factura.RucProveedor,
                    Subtotal = dto.Factura.Subtotal,
                    SubtotalIva = dto.Factura.SubtotalIva,
                    Total = dto.Factura.Total,
                    RutaImagen = rutaFinal
                };

                var viatico = new Viatico
                {
                    CategoriaId = dto.CategoriaId,
                    Comentario = dto.Comentario,
                    EstadoViatico = EstadoViatico.Borrador,
                    SolicitudViaticoId = solicitud.Id,
                    VehiculoId = dto.VehiculoId
                };

                var crearDTO = new CrearViaticoDTO
                {
                    Viatico = viatico,
                    Factura = factura,
                    UsuarioAppId = dto.UsuarioAppId,
                    CicloId = dto.CicloId,
                    Monto = dto.Factura.Total
                };

                // Actualizar el monto con cada inserción de viatico
                solicitud.Monto += dto.Factura.Total;
                await _solicitudRepository.ActualizarMontoAsync(solicitud);

                var id = await _viaticoRepository.CrearViaticoAsync(crearDTO);
                await _unitOfWork.CommitAsync();

                return id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                if (File.Exists(rutaAbsoluta))
                    File.Delete(rutaAbsoluta);

                throw;
            }
        }

        public async Task<EstadisticaSolicitudViaticoDTO?> ObtenerEstadisticaSolicitudViaticoAsync(int cicloId)
        {
            return await _viaticoRepository.ObtenerEstadisticaSolicitudViaticoAsync(cicloId);
        }

        public async Task<IEnumerable<ViaticoListDTO>> ObtenerViaticosPorSolicitudAsync(int solicitudId)
        {
            return await _viaticoRepository.ObtenerViaticosPorSolicitudAsync(solicitudId);
        }

        public async Task ActualizarEstadoViaticosAsync(ActualizarEstadoViaticoRequest request)
        {
            if (request.Viaticos == null || request.Viaticos.Count == 0)
                throw new BusinessException("Debe seleccionar al menos un viático.");

            await _unitOfWork.BeginTransactionAsync();

            var eventos = new List<IDomainEvent>();

            try
            {
                var ids = request.Viaticos.Select(v => v.Id).ToList();
                var viaticos = await _viaticoRepository.ObtenerViaticosPorIdsAsync(ids);

                if (viaticos.Count != ids.Count)
                    throw new BusinessException("Uno o más viáticos no existen.");

                foreach (var viatico in viaticos)
                {
                    ValidarEstadoActual(viatico, request.Accion);

                    var estadoAnterior = viatico.EstadoViatico;

                    switch (request.Accion)
                    {
                        case AccionViatico.Aprobar:
                            AprobarViatico(viatico);
                            break;

                        case AccionViatico.Rechazar:
                            RechazarViatico(viatico, request);
                            break;
                    }

                    if (estadoAnterior != viatico.EstadoViatico)
                    {
                        eventos.Add(new EstadoViaticoCambiadoEvent(
                            viatico.Id,
                            estadoAnterior,
                            viatico.EstadoViatico
                        ));
                    }
                }

                await _viaticoRepository.ActualizarViaticosAsync(viaticos);

                var solicitudesIds = viaticos.Select(v => v.SolicitudViaticoId).Distinct().ToList();
                await ActualizarEstadoSolicitudesAsync(solicitudesIds);

                await _unitOfWork.CommitAsync();

                foreach (var evento in eventos)
                    await _eventDispatcher.Dispatch(evento);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        private void ValidarEstadoActual(Viatico viatico, AccionViatico accion)
        {
            if (accion == AccionViatico.Aprobar && viatico.EstadoViatico == EstadoViatico.Aprobado)
                throw new BusinessException($"El viático {viatico.Id} ya está aprobado.");

            if (accion == AccionViatico.Rechazar)
            {
                if (viatico.EstadoViatico == EstadoViatico.Rechazado)
                    throw new BusinessException($"El viático {viatico.Id} ya fue rechazado y no puede modificarse.");

                if (viatico.EstadoViatico == EstadoViatico.Devuelto)
                    throw new BusinessException($"El viático {viatico.Id} ya fue devuelto para corrección. Debe ser corregido por el usuario.");
            }
        }

        private void AprobarViatico(Viatico viatico)
        {
            viatico.EstadoViatico = EstadoViatico.Aprobado;
            viatico.Comentario = null;
            viatico.CamposRechazados = null;
        }

        private void RechazarViatico(Viatico viatico, ActualizarEstadoViaticoRequest request)
        {
            viatico.EstadoViatico = EstadoViatico.Rechazado;

            if (request.Viaticos.Count > 1)
            {
                // Rechazo masivo
                viatico.Comentario = "Viático inválido";
                viatico.CamposRechazados = null;
            }
            else
            {
                // Rechazo individual
                var viaticoRequest = request.Viaticos.FirstOrDefault(v => v.Id == viatico.Id);

                if (viaticoRequest != null)
                {
                    viatico.Comentario = string.IsNullOrWhiteSpace(viaticoRequest.Comentario)
                        ? "Viático inválido"
                        : viaticoRequest.Comentario;

                    viatico.CamposRechazados = viaticoRequest.CamposRechazados?.Select(c => new CampoRechazado
                    {
                        Campo = c.Campo,
                        Comentario = c.Comentario
                    }).ToList();
                }
                else
                {
                    viatico.Comentario = "Viático inválido";
                    viatico.CamposRechazados = null;
                }
            }
        }

        private async Task ActualizarEstadoSolicitudesAsync(List<int> solicitudesIds)
        {
            foreach (var solicitudId in solicitudesIds)
            {
                var solicitud = await _solicitudRepository.ObtenerViaticosPorIdAsync(solicitudId);

                if (solicitud == null)
                    continue;

                var estadosViaticos = solicitud.Viaticos.Select(v => v.EstadoViatico).ToList();

                if (estadosViaticos.All(e => e == EstadoViatico.Aprobado))
                {
                    solicitud.Estado = EstadoSolicitud.Aprobado;
                }
                else if (estadosViaticos.All(e => e == EstadoViatico.Rechazado))
                {
                    solicitud.Estado = EstadoSolicitud.Rechazado;
                }
                else
                {
                    solicitud.Estado = EstadoSolicitud.EnRevision;
                }

                await _solicitudRepository.ActualizarEstadoAsync(solicitud);
            }
        }

        public async Task EditarCamposFacturaAsync(int id, EditarViaticoDTO dto)
        {
            var viatico = await _viaticoRepository.GetIdPorFacturaAsync(id);
            if (viatico == null)
                throw new Exception("Viático no encontrado");

            if (viatico.Factura is null)
                throw new BusinessException("El viático no tiene una factura asociada");

            if (!string.IsNullOrWhiteSpace(dto.NombreProveedor))
                viatico.Factura.Proveedor.RazonSocial = dto.NombreProveedor;

            if (!string.IsNullOrWhiteSpace(dto.NumeroFactura))
                viatico.Factura.NumeroFactura = dto.NumeroFactura;

            // Actualizar fecha viatico
            _viaticoRepository.MarcarModificado(viatico);

            // Actualizar fecha solicitud viatico
            if (viatico.SolicitudViatico is not null)
            {
                _viaticoRepository.MarcarModificado(viatico.SolicitudViatico);
            }

            await _viaticoRepository.SaveChangesAsync();
        }
    }

}
