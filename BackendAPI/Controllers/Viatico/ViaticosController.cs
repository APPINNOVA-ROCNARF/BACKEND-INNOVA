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
        private readonly IWebHostEnvironment _env;

        public ViaticosController(IViaticoService viaticoService, IWebHostEnvironment env)
        {
            _viaticoService = viaticoService;
            _env = env;
        }

        [HttpPost("crear")]
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
                // Errores de negocio como factura duplicada
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                // Errores inesperados
                return StatusCode(500, new { mensaje = "Ocurrió un error interno.", detalle = ex.Message });
            }
        }
    }
}
