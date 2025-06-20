using Application.DTO.ArchivoDTO;
using Application.Exceptions;
using Application.Helpers;
using Application.Interfaces.IArchivo;
using Application.Options;
using Infrastructure.Repositories;
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
        public async Task<IActionResult> SubirTemporal([FromBody] ArchivoUploadDTO archivoDto)
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

        [HttpPost("upload-temp")]
        public async Task<IActionResult> UploadTemp([FromForm] ArchivoFormDTO form)
        {
            var file = form.File;

            if (file == null || file.Length == 0)
                return BadRequest("Archivo no válido.");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var contenido = ms.ToArray();

            var dto = new ArchivoUploadDTO
            {
                Nombre = Path.GetFileNameWithoutExtension(file.FileName),
                Extension = Path.GetExtension(file.FileName),
                Contenido = contenido
            };

            var rutaBase = Path.Combine(_env.ContentRootPath, "Storage");
            var rutaTemporal = await _archivoService.SubirArchivoTempAsync(dto, rutaBase);

            var resultado = new ArchivoTemporalGuardadoDTO
            {
                NombreOriginal = file.FileName,
                RutaTemporal = rutaTemporal,
                Extension = dto.Extension
            };

            return Ok(resultado);
        }

        /*[HttpGet("descargar")]
        public IActionResult DescargarArchivo(
            [FromQuery] string rutaRelativa,
            [FromQuery] string? modo = "ver",
            [FromQuery] string? modulo = "general")
        {
            try
            {
                var archivo = _archivoService.ObtenerArchivo(rutaRelativa, _env.ContentRootPath, modo, modulo);
                var disposition = archivo.Modo.ToLowerInvariant() == "descargar" ? "attachment" : "inline";
                return File(archivo.Contenido, archivo.Mime, disposition == "attachment" ? archivo.Nombre : null);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        */
    }
}
