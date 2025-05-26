using Application.DTO.ArchivoDTO;
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

        [HttpGet("descargar")]
        public IActionResult DescargarArchivo([FromQuery] string rutaRelativa, [FromQuery] string? modo = "ver")
        {
            if (string.IsNullOrWhiteSpace(rutaRelativa))
                return BadRequest("Ruta no válida.");

            if (rutaRelativa.Contains(".."))
                return BadRequest("Ruta no permitida.");

            rutaRelativa = Uri.UnescapeDataString(rutaRelativa);
            rutaRelativa = rutaRelativa.TrimStart('/');

            var rutaCompleta = Path.Combine(_env.ContentRootPath, "Storage", rutaRelativa.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (!System.IO.File.Exists(rutaCompleta))
                return NotFound("Archivo no encontrado.");

            var extension = Path.GetExtension(rutaCompleta).ToLowerInvariant();
            var mime = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };

            var contenido = System.IO.File.ReadAllBytes(rutaCompleta);
            var nombreArchivo = Path.GetFileName(rutaRelativa);
            var encodedFileName = Uri.EscapeDataString(nombreArchivo);

            var disposition = modo?.ToLowerInvariant() == "descargar" ? "attachment" : "inline";

            return File(contenido, mime, disposition == "attachment" ? nombreArchivo : null);
        }

    }
}
