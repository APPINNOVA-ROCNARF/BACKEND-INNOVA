using Application.Audit;
using Application.DTO.PresupuestoViaticoDTO;
using Application.DTO.ViaticoDTO;
using Application.Interfaces.IPresupuestoViatico;
using Application.Interfaces.IViatico;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers.Viatico
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViaticosController : ControllerBase
    {
        private readonly IViaticoService _viaticoService;
        private readonly ISolicitudViaticoService _solicitudViaticoService;
        private readonly ICupoMensualService _cupoMensualService;
        private readonly IAuditoriaService _auditoriaService;
        private readonly IWebHostEnvironment _env;

        public ViaticosController(IViaticoService viaticoService, ISolicitudViaticoService solicitudViaticoService, ICupoMensualService cupoMensualService, IAuditoriaService auditoriaService, IWebHostEnvironment env)
        {
            _viaticoService = viaticoService;
            _solicitudViaticoService = solicitudViaticoService;
            _cupoMensualService = cupoMensualService;
            _auditoriaService = auditoriaService;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> CrearViatico([FromBody] ViaticoCrearDTO dto)
        {
            if (dto == null || dto.Factura == null)
                return BadRequest("Datos incompletos.");

            try
            {
                var rutaBase = Path.Combine(_env.ContentRootPath, "Storage");

                var viaticoId = await _viaticoService.CrearViaticoAsync(dto, rutaBase);

                return Ok(new
                {
                    mensaje = "Viático registrado correctamente.",
                    viaticoId
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error interno.", detalle = ex.Message });
            }
        }

        [HttpGet("solicitud/ciclo/{cicloId}")]
        public async Task<IActionResult> ObtenerSolicitudPorCicloAsync(int cicloId)
        {
            var resultado = await _solicitudViaticoService.ObtenerSolicitudPorCicloAsync(cicloId);
            return Ok(resultado);
        }

        [HttpGet("estadistica-solicitud-viatico/{cicloId}")]
        public async Task<IActionResult> GetDashboard(int cicloId)
        {
            var resultado = await _viaticoService.ObtenerEstadisticaSolicitudViaticoAsync(cicloId);

            if (resultado == null)
                return NotFound("No se encontró información para el ciclo indicado.");

            return Ok(resultado);
        }

        [HttpGet("estadistica-viatico/{solicitudId}")]
        public async Task<IActionResult> GetDashboardDetalle(int solicitudId)
        {
            var resultado = await _viaticoService.ObtenerEstadisticaViaticoAsync(solicitudId);

            if (resultado == null)
                return NotFound("No se encontró información para la solicitud proporcionada.");

            return Ok(resultado);
        }

        [HttpGet("{solicitudId:int}")]
        public async Task<ActionResult<IEnumerable<ViaticoListDTO>>> GetViaticoPorSolicitud(int solicitudId)
        {
            var resultado = await _viaticoService.ObtenerViaticosPorSolicitudAsync(solicitudId);
            return Ok(resultado);
        }

        [HttpGet("solicitud/detalle/{solicitudId}")]
        public async Task<IActionResult> ObtenerDetalleSolicitud(int solicitudId)
        {
            var resultado = await _solicitudViaticoService.ObtenerDetalleSolicitud(solicitudId);
            return Ok(resultado);
        }

        [HttpPost("actualizar-estado")]
        public async Task<IActionResult> ActualizarEstadoViaticos([FromBody] ActualizarEstadoViaticoRequest request)
        {
            await _viaticoService.ActualizarEstadoViaticosAsync(request);
            return Ok(new { success = true, message = "Estado de viáticos actualizado correctamente." });
        }

        [HttpPatch("{facturaId}")]
        public async Task<IActionResult> EditarCamposViatico(int facturaId, [FromBody] EditarViaticoDTO dto)
        {
            await _viaticoService.EditarCamposFacturaAsync(facturaId, dto);
            return Ok();
        }

        /// <summary>
        /// Carga masiva de cupos mensuales por sección para un ciclo.
        /// </summary>
        /// <param name="sectores">Lista de cupos por sección</param>
        /// <param name="cicloId">Id del ciclo al que se asignan los cupos</param>
        /// <param name="cancellationToken">Token para cancelación</param>
        [HttpPost("cargar-cupos")]
        public async Task<IActionResult> CargarCupos(
            [FromBody] List<CupoMensualDTO> sectores,
            [FromQuery] int cicloId,
            CancellationToken cancellationToken)
        {
            if (sectores == null || !sectores.Any())
                return BadRequest("La lista de sectores está vacía o es nula.");

            var (insertados, errores) = await _cupoMensualService
                .CargarCuposDesdeSectoresAsync(sectores, cicloId, cancellationToken);

            return Ok(new
            {
                mensaje = "Carga de cupos completada",
                cuposInsertados = insertados,
                sectoresNoEncontrados = errores
            });
        }

        [HttpGet("verificar-cupos")]
        public async Task<IActionResult> VerificarCarga([FromQuery] int cicloId)
        {
            if (cicloId <= 0)
                return BadRequest("Debe proporcionar un ciclo válido.");

            var existe = await _cupoMensualService.ExisteCargaParaCicloAsync(cicloId);

            return existe ? Ok() : NoContent();
        }

        [HttpGet("historial")]
        public async Task<IActionResult> ObtenerHistorial([FromQuery] int viaticoId)
        {
            var historial = await _viaticoService.ObtenerHistorialViaticoAsync(viaticoId);
            return Ok(historial);
        }

        [HttpGet("reporte")]
        public async Task<ActionResult<List<ViaticoReporteDTO>>> GetResumenPorCategoria(
    [FromQuery] int? cicloId,
    [FromQuery] DateTime? fechaInicio,
    [FromQuery] DateTime? fechaFin)
        {
            if (!cicloId.HasValue && (!fechaInicio.HasValue || !fechaFin.HasValue))
            {
                return BadRequest("Debe especificar cicloId o un rango de fechas (fechaInicio y fechaFin).");
            }

            var resultado = await _viaticoService.ObtenerResumenPorCategoriaAsync(cicloId, fechaInicio, fechaFin);
            return Ok(resultado);
        }

        [HttpGet("exportar-excel")]
        public async Task<IActionResult> ExportarExcel([FromQuery] int? cicloId, [FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            var excelBytes = await _viaticoService.GenerarExcelAsync(cicloId, fechaInicio, fechaFin);
            return File(excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "resumen_viaticos.xlsx");
        }
    }
}
