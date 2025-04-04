using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers.Sistema
{
    [Route("api/[controller]")]
    [ApiController]
    public class SistemaController : ControllerBase
    {
        private readonly ISistemaService _service;

        public SistemaController(ISistemaService service)
        {
            _service = service;
        }

        [HttpGet("periodos/select")]
        public async Task<IActionResult> GetCiclosSelect()
        {
            var ciclos = await _service.ObtenerCiclosSelectAsync();
            return Ok(ciclos);
        }
    }
}
