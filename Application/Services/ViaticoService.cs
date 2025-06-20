using System.Runtime.CompilerServices;
using Application.Audit;
using Application.DTO.ArchivoDTO;
using Application.DTO.ViaticoDTO;
using Application.DTO.ViaticoDTO.mobile;
using Application.Enums.Viatico;
using Application.Exceptions;
using Application.Interfaces.IArchivo;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.IUsuario;
using Application.Interfaces.IViatico;
using ClosedXML.Excel;
using Domain.Common;
using Domain.Entities.Viaticos;
using Domain.Events;

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
        private readonly IUsuarioActualService _usuarioActualService;

        public ViaticoService(
            IUnitOfWork unitOfWork,
            IViaticoRepository viaticoRepository,
            ISolicitudViaticoRepository solicitudRepository,
            IArchivoRepository archivoRepository,
            IProveedorViaticoRepository proveedorRepository,
            IAuditoriaRepository auditoriaRepository,
            IUsuarioService usuarioService,
            IDomainEventDispatcher eventDispatcher,
            IUsuarioActualService usuarioActualService)
        {
            _unitOfWork = unitOfWork;
            _viaticoRepository = viaticoRepository;
            _solicitudRepository = solicitudRepository;
            _archivoRepository = archivoRepository;
            _proveedorRepository = proveedorRepository;
            _auditoriaRepository = auditoriaRepository;
            _usuarioService = usuarioService;
            _eventDispatcher = eventDispatcher;
            _usuarioActualService = usuarioActualService;
        }

        public async Task<int> CrearViaticoAsync(ViaticoCrearDTO dto, string rutaBase)
        {
            var moverArchivoDto = new MoverArchivoDTO
            {
                RutaRelativaTemporal = dto.Factura.RutaTemporal,
                UsuarioAppId = dto.UsuarioAppId,
                CicloId = dto.CicloId,
                FechaFactura = dto.Factura.FechaFactura,
                PrefijoNombre = "factura"
            };

            var rutaFinal = await _archivoRepository.MoverArchivoFinalAsync(moverArchivoDto, rutaBase);
            var rutaAbsoluta = Path.Combine(rutaBase, rutaFinal);

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

                await _eventDispatcher.Dispatch(new ViaticoCreadoEvent(
                    id,
                    dto.CategoriaId,
                    dto.SubcategoriaId,
                    dto.Factura.Total
                ));

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

            var viatico = await _viaticoRepository.GetIdPorFacturaAsync(facturaId);
            if (viatico == null)
                throw new Exception("Viático no encontrado");

            if (viatico.Factura is null)
                throw new BusinessException("El viático no tiene una factura asociada");

            var eventos = new List<IDomainEvent>();

            if (!string.IsNullOrWhiteSpace(dto.NombreProveedor) && dto.NombreProveedor != viatico.Factura.Proveedor.RazonSocial)
            {
                eventos.Add(new FacturaEditadaEvent(
                    viatico.Factura.Id,
                    "NombreProveedor",
                    viatico.Factura.Proveedor.RazonSocial ?? "",
                    dto.NombreProveedor
                ));

                viatico.Factura.Proveedor.RazonSocial = dto.NombreProveedor;
            }

            if (!string.IsNullOrWhiteSpace(dto.NumeroFactura) && dto.NumeroFactura != viatico.Factura.NumeroFactura)
            {
                eventos.Add(new FacturaEditadaEvent(
                    viatico.Factura.Id,
                    "NumeroFactura",
                    viatico.Factura.NumeroFactura ?? "",
                    dto.NumeroFactura
                ));

                viatico.Factura.NumeroFactura = dto.NumeroFactura;
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
            // Cargar viático con su factura
            var viatico = await _viaticoRepository
                .GetIdPorFacturaAsync(viaticoId);

            if (viatico == null)
                throw new BusinessException("Viático no encontrado");

            // Historial del viático
            var historialViatico = await _auditoriaRepository.ObtenerHistorialAsync("Viatico", viaticoId);

            // Historial de la factura (si existe)
            var historialFactura = viatico.Factura != null
                ? await _auditoriaRepository.ObtenerHistorialAsync("Factura", viatico.Factura.Id)
                : new List<HistorialAuditoriaDTO>();

            // Combinar resultados
            return historialViatico
                .Concat(historialFactura)
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
            var datos = await ObtenerResumenPorCategoriaAsync(cicloId, fechaInicio, fechaFin);

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

        public async Task<Dictionary<int, bool>> ObtenerFacturasVistasAsync(List<int> facturaIds)
        {
            var usuarioId = _usuarioActualService.Obtener().Id;

            return await _auditoriaRepository.FacturasVistas(
                "Factura",
                facturaIds,
                usuarioId,
                "ArchivoAccedido"
            );
        }

        // APP MOVIL

        public async Task<IEnumerable<AppViaticoListDTO>> ObtenerViaticosPorUsuarioApp(string nombreUsuario, int cicloId)
        {
            var usuarioId = await _usuarioService.ObtenerIdPorNombreUsuario(nombreUsuario);

            var solicitud = await _solicitudRepository.ObtenerPorCicloUsuarioAsync(cicloId, usuarioId);

            return await _viaticoRepository.ObtenerViaticosApp(solicitud.Id);

        }
    }

}
