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

        [HttpPost("upload-zip")]
        public async Task<IActionResult> ProcesarZip([FromForm] ArchivoFormDTO form)
        {
            var file = form.File;

            if (file == null || file.Length == 0)
                return BadRequest("Archivo ZIP no válido.");

            if (!Path.GetExtension(file.FileName).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                return BadRequest("El archivo debe tener extensión .zip.");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var archivoZipDto = new ArchivoUploadDTO
            {
                Nombre = Path.GetFileNameWithoutExtension(file.FileName),
                Extension = Path.GetExtension(file.FileName),
                Contenido = ms.ToArray()
            };

            var rutaBase = Path.Combine(_env.ContentRootPath, "Storage");
            var archivosDbf = await _archivoService.ProcesarArchivoZipAsync(archivoZipDto, rutaBase);

            return Ok(archivosDbf);
        }

    }
}
