using Application.Audit;
using Application.DTO.ArchivoDTO;
using Application.DTO.ViaticoDTO;
using Application.Enums.Viatico;
using Application.Exceptions;
using Application.Helpers;
using Application.Interfaces.IArchivo;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.IUsuario;
using Application.Interfaces.IVehiculo;
using Application.Interfaces.IViatico;
using ClosedXML.Excel;
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
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IDomainEventDispatcher _eventDispatcher;

        public ViaticoService(
            IUnitOfWork unitOfWork,
            IViaticoRepository viaticoRepository,
            ISolicitudViaticoRepository solicitudRepository,
            IArchivoRepository archivoRepository,
            IProveedorViaticoRepository proveedorRepository,
            IAuditoriaRepository auditoriaRepository,
            IUsuarioService usuarioService,
            IDomainEventDispatcher eventDispatcher)
        {
            _unitOfWork = unitOfWork;
            _viaticoRepository = viaticoRepository;
            _solicitudRepository = solicitudRepository;
            _archivoRepository = archivoRepository;
            _proveedorRepository = proveedorRepository;
            _auditoriaRepository = auditoriaRepository;
            _usuarioService = usuarioService;
            _eventDispatcher =eventDispatcher;
        }

        public async Task<int> CrearViaticoAsync(ViaticoCrearDTO dto, string webRootPath)
        {
            var facturasFinales = new List<FacturaViatico>();
            decimal montoTotal = 0;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                SubcategoriaViatico? subcategoria = null;

                if (dto.SubcategoriaId.HasValue)
                {
                    subcategoria = await _viaticoRepository.ObtenerPorIdAsync(dto.SubcategoriaId);

                    if (subcategoria == null)
                        throw new BusinessException("La subcategoría especificada no existe.");

                    if (dto.Facturas.Count != subcategoria.FacturasRequeridas)
                        throw new BusinessException($"La subcategoría '{subcategoria.Nombre}' requiere {subcategoria.FacturasRequeridas} factura(s).");
                }

                foreach (var facturaDTO in dto.Facturas)
                {
                    // Mover archivo
                    var moverArchivoDto = new MoverArchivoDTO
                    {
                        RutaRelativaTemporal = facturaDTO.RutaTemporal,
                        UsuarioAppId = dto.UsuarioAppId,
                        CicloId = dto.CicloId,
                        FechaFactura = facturaDTO.FechaFactura,
                        PrefijoNombre = "factura"
                    };

                    var rutaFinal = await _archivoRepository.MoverArchivoFinalAsync(moverArchivoDto, webRootPath);
                    var rutaAbsoluta = Path.Combine(webRootPath, rutaFinal);

                    // Crear proveedor si no existe
                    if (!await _proveedorRepository.ExistePorRucAsync(facturaDTO.RucProveedor))
                    {
                        var proveedor = new ProveedorViatico
                        {
                            Ruc = facturaDTO.RucProveedor,
                            RazonSocial = facturaDTO.ProveedorNombre
                        };
                        await _proveedorRepository.CrearAsync(proveedor);
                    }

                    // Crear factura
                    var factura = new FacturaViatico
                    {
                        NumeroFactura = facturaDTO.NumeroFactura,
                        FechaFactura = DateTime.SpecifyKind(facturaDTO.FechaFactura, DateTimeKind.Unspecified),
                        RucProveedor = facturaDTO.RucProveedor,
                        Subtotal = facturaDTO.Subtotal,
                        SubtotalIva = facturaDTO.SubtotalIva,
                        Total = facturaDTO.Total,
                        RutaImagen = rutaFinal
                    };

                    facturasFinales.Add(factura);
                    montoTotal += facturaDTO.Total;
                }

                // Obtener o crear solicitud
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

                // Crear viático
                var viatico = new Viatico
                {
                    CategoriaId = dto.CategoriaId,
                    SubcategoriaId = dto.SubcategoriaId,
                    Comentario = dto.Comentario,
                    EstadoViatico = EstadoViatico.Borrador,
                    SolicitudViaticoId = solicitud.Id,
                    VehiculoId = dto.VehiculoId
                };

                // Armar DTO combinado
                var crearDTO = new CrearViaticoDTO
                {
                    Viatico = viatico,
                    Facturas = facturasFinales,
                    UsuarioAppId = dto.UsuarioAppId,
                    CicloId = dto.CicloId,
                    Monto = montoTotal
                };

                solicitud.Monto += montoTotal;
                await _solicitudRepository.ActualizarMontoAsync(solicitud);

                var id = await _viaticoRepository.CrearViaticoAsync(crearDTO);
                await _unitOfWork.CommitAsync();

                return id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                foreach (var factura in facturasFinales)
                {
                    var rutaAbsoluta = Path.Combine(webRootPath, factura.RutaImagen);
                    if (File.Exists(rutaAbsoluta))
                        File.Delete(rutaAbsoluta);
                }

                throw;
            }
        }

        public async Task<EstadisticaSolicitudViaticoDTO?> ObtenerEstadisticaSolicitudViaticoAsync(int cicloId)
        {
            return await _viaticoRepository.ObtenerEstadisticaSolicitudViaticoAsync(cicloId);
        }

        public async Task<List<EstadisticaViaticoDTO>> ObtenerEstadisticaViaticoAsync(int solicitudId)
        {
            return await _viaticoRepository.ObtenerEstadisticaViaticoAsync(solicitudId);
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
                            "Estado",
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
            var viaticoRequest = request.Viaticos.FirstOrDefault(v => v.Id == viatico.Id);

            if (viaticoRequest != null)
            {
                var camposRechazados = viaticoRequest.CamposRechazados?.Where(c => !string.IsNullOrWhiteSpace(c.Campo)).ToList();

                if (camposRechazados != null && camposRechazados.Count > 0)
                {
                    viatico.EstadoViatico = EstadoViatico.Devuelto;
                    viatico.CamposRechazados = camposRechazados.Select(c => new CampoRechazado
                    {
                        Campo = c.Campo,
                        Comentario = c.Comentario
                    }).ToList();

                    viatico.Comentario = string.IsNullOrWhiteSpace(viaticoRequest.Comentario)
                        ? "Viático devuelto para corrección"
                        : viaticoRequest.Comentario;
                }
                else
                {
                    viatico.EstadoViatico = EstadoViatico.Rechazado;
                    viatico.CamposRechazados = null;
                    viatico.Comentario = string.IsNullOrWhiteSpace(viaticoRequest.Comentario)
                        ? "Viático rechazado"
                        : viaticoRequest.Comentario;
                }
            }
            else
            {
                viatico.EstadoViatico = EstadoViatico.Rechazado;
                viatico.Comentario = "Viático rechazado";
                viatico.CamposRechazados = null;
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

        public async Task EditarCamposFacturaAsync(int facturaId, EditarViaticoDTO dto)
        {
            var relacion = await _viaticoRepository.ObtenerRelacionConFacturaYViaticoAsync(facturaId);

            if (relacion == null)
                throw new BusinessException("La factura no está asociada a ningún viático");

            var factura = relacion.Factura;
            var viatico = relacion.Viatico;

            var eventos = new List<IDomainEvent>();

            if (!string.IsNullOrWhiteSpace(dto.NombreProveedor) && dto.NombreProveedor != factura.Proveedor.RazonSocial)
            {
                eventos.Add(new FacturaEditadaEvent(
                    factura.Id,
                    "NombreProveedor",
                    factura.Proveedor.RazonSocial ?? "",
                    dto.NombreProveedor
                ));

                factura.Proveedor.RazonSocial = dto.NombreProveedor;
            }

            if (!string.IsNullOrWhiteSpace(dto.NumeroFactura) && dto.NumeroFactura != factura.NumeroFactura)
            {
                eventos.Add(new FacturaEditadaEvent(
                    factura.Id,
                    "NumeroFactura",
                    factura.NumeroFactura ?? "",
                    dto.NumeroFactura
                ));

                factura.NumeroFactura = dto.NumeroFactura;
            }


            // Actualizar fecha viatico
            _viaticoRepository.MarcarModificado(viatico);

            // Actualizar fecha solicitud viatico
            if (viatico.SolicitudViatico is not null)
            {
                _viaticoRepository.MarcarModificado(viatico.SolicitudViatico);
            }

            await _viaticoRepository.SaveChangesAsync();

            foreach (var evento in eventos)
            {
                await _eventDispatcher.Dispatch(evento);
            }
        }


        public async Task<IList<HistorialAuditoriaDTO>> ObtenerHistorialViaticoAsync(int viaticoId)
        {
            // 1) historial directo de viático
            var historialViatico = await _auditoriaRepository.ObtenerHistorialAsync("Viatico", viaticoId);
            // 2) historial de facturas relacionadas
            var facturaIds = await _viaticoRepository.ObtenerIdsFacturasPorViaticoAsync(viaticoId);

            var historialFacturas = new List<HistorialAuditoriaDTO>();
            foreach (var fid in facturaIds)
                historialFacturas.AddRange(await _auditoriaRepository.ObtenerHistorialAsync("Factura", fid));
            // 3) combinar y ordenar
            return historialViatico
                   .Concat(historialFacturas)
                   .OrderByDescending(h => h.Fecha)
                   .ToList();
        }

        public async Task<List<ViaticoReporteDTO>> ObtenerResumenPorCategoriaAsync(int? cicloId, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var viaticos = await _viaticoRepository.ObtenerConSolicitudYCategoriaPorFiltroAsync(cicloId, fechaInicio, fechaFin);

            var usuarioIds = viaticos.Select(v => v.SolicitudViatico!.UsuarioAppId).Distinct().ToList();
            var cicloIds = viaticos.Select(v => v.SolicitudViatico!.CicloId).Distinct().ToList();

            var cupos = await _viaticoRepository.ObtenerCuposMensualesAsync(usuarioIds, cicloIds);

            var resultado = new List<ViaticoReporteDTO>();

            foreach (var usuarioId in usuarioIds)
            {
                var fila = new ViaticoReporteDTO
                {
                    UsuarioId = usuarioId,
                    NombreUsuario = await _usuarioService.ObtenerNombreCompletoAsync(usuarioId)
                };

                foreach (var categoria in new[] { "Movilización", "Alimentación", "Hospedaje" })
                {
                    var viaticosCategoria = viaticos
                        .Where(v => v.SolicitudViatico!.UsuarioAppId == usuarioId && v.Categoria!.Nombre == categoria)
                        .ToList();

                    var acreditado = cupos
                        .Where(c => c.UsuarioId == usuarioId && c.Categoria == categoria)
                        .Sum(c => c.MontoAsignado);

                    var aprobado = viaticosCategoria
                        .Where(v => v.EstadoViatico == EstadoViatico.Aprobado)
                        .Sum(v => v.Monto);

                    var rechazado = viaticosCategoria
                        .Where(v => v.EstadoViatico == EstadoViatico.Rechazado)
                        .Sum(v => v.Monto);

                    switch (categoria)
                    {
                        case "Movilización":
                            fila.MovilizacionAcreditado = acreditado;
                            fila.MovilizacionAprobado = aprobado;
                            fila.MovilizacionRechazado = rechazado;
                            break;
                        case "Alimentación":
                            fila.AlimentacionAcreditado = acreditado;
                            fila.AlimentacionAprobado = aprobado;
                            fila.AlimentacionRechazado = rechazado;
                            break;
                        case "Hospedaje":
                            fila.HospedajeAcreditado = acreditado;
                            fila.HospedajeAprobado = aprobado;
                            fila.HospedajeRechazado = rechazado;
                            break;
                    }
                }

                resultado.Add(fila);
            }

            return resultado;
        }

        public async Task<byte[]> GenerarExcelAsync(int? cicloId, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var datos = await ObtenerResumenPorCategoriaAsync(cicloId, fechaInicio, fechaFin); // tu método existente

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Resumen Viáticos");

            // Cabecera
            var encabezados = new[]
            {
        "Usuario",
        "Movilización Acreditado", "Movilización Aprobado", "Movilización Rechazado", "Movilización Diferencia",
        "Alimentación Acreditado", "Alimentación Aprobado", "Alimentación Rechazado", "Alimentación Diferencia",
        "Hospedaje Acreditado", "Hospedaje Aprobado", "Hospedaje Rechazado", "Hospedaje Diferencia"
    };

            for (int i = 0; i < encabezados.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = encabezados[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Column(i + 1).AdjustToContents();
            }

            // Datos
            for (int i = 0; i < datos.Count; i++)
            {
                var d = datos[i];
                int fila = i + 2;

                worksheet.Cell(fila, 1).Value = d.NombreUsuario;
                worksheet.Cell(fila, 2).Value = d.MovilizacionAcreditado;
                worksheet.Cell(fila, 3).Value = d.MovilizacionAprobado;
                worksheet.Cell(fila, 4).Value = d.MovilizacionRechazado;
                worksheet.Cell(fila, 5).Value = d.MovilizacionDiferencia;

                worksheet.Cell(fila, 6).Value = d.AlimentacionAcreditado;
                worksheet.Cell(fila, 7).Value = d.AlimentacionAprobado;
                worksheet.Cell(fila, 8).Value = d.AlimentacionRechazado;
                worksheet.Cell(fila, 9).Value = d.AlimentacionDiferencia;

                worksheet.Cell(fila, 10).Value = d.HospedajeAcreditado;
                worksheet.Cell(fila, 11).Value = d.HospedajeAprobado;
                worksheet.Cell(fila, 12).Value = d.HospedajeRechazado;
                worksheet.Cell(fila, 13).Value = d.HospedajeDiferencia;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }

}
