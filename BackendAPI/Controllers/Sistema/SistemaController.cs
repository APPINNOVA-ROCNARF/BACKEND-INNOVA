using Application.DTO.GuiaProductoDTO;
using Application.Interfaces.ISistema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers.Sistema
{
    [Route("api/[controller]")]
    [ApiController]
    public class SistemaController : ControllerBase
    {
        private readonly ISistemaService _service;
        private readonly IWebHostEnvironment _env;

        public SistemaController(ISistemaService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet("ciclos/select")]
        public async Task<IActionResult> GetCiclosSelect()
        {
            var ciclos = await _service.ObtenerCiclosSelectAsync();
            return Ok(ciclos);
        }

        [HttpPost("crear-guia-producto")]
        public async Task<IActionResult> CrearGuiaProducto([FromBody] CrearGuiaProductoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var webRootPath = _env.WebRootPath;

            var id = await _service.CrearGuiaProductoAsync(dto, webRootPath);

            return Ok(new { id });
        }

        [HttpGet("obtener-guias-producto")]
        public async Task<IActionResult> ObtenerGuiasProducto()
        {
            var guias = await _service.ObtenerGuiasProductoAsync();
            return Ok(guias);
        }
    }
}
