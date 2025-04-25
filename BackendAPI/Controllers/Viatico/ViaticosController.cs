using Application.DTO.ViaticoDTO;
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
        private readonly IWebHostEnvironment _env;

        public ViaticosController(IViaticoService viaticoService, ISolicitudViaticoService solicitudViaticoService, IWebHostEnvironment env)
        {
            _viaticoService = viaticoService;
            _solicitudViaticoService = solicitudViaticoService;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> CrearViatico([FromBody] ViaticoCrearDTO dto)
        {
            if (dto == null || dto.Factura == null)
                return BadRequest("Datos incompletos.");

            try
            {
                var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                var viaticoId = await _viaticoService.CrearViaticoAsync(dto, webRootPath);

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

        [HttpGet("{solicitudId:int}")]
        public async Task<ActionResult<IEnumerable<ViaticoListDTO>>> GetListadoPorSolicitud(int solicitudId)
        {
            var resultado = await _viaticoService.ObtenerViaticosPorSolicitudAsync(solicitudId);
            return Ok(resultado);
        }
    }
}
