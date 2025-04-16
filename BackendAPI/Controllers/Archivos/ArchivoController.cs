using Application.DTO.ArchivoDTO;
using Application.Helpers;
using Application.Interfaces.IArchivo;
using Application.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BackendAPI.Controllers.Archivos
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArchivoController : ControllerBase
    {
        private readonly IArchivoService _archivoService;
        private readonly IWebHostEnvironment _env;

        public ArchivoController(IArchivoService archivoService, IWebHostEnvironment env)
        {
            _archivoService = archivoService;
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> SubirTemporal([FromBody] ArchivoDTO archivoDto)
        {
            if (archivoDto == null || archivoDto.Contenido == null || archivoDto.Contenido.Length == 0)
                return BadRequest("Archivo no válido.");

            try
            {
                var rutaRelativa = await _archivoService.SubirArchivoTempAsync(archivoDto, _env.WebRootPath);
                return Ok(new { rutaRelativa });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
