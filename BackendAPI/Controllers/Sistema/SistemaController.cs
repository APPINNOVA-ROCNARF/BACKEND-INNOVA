using Application.DTO.GuiaProductoDTO;
using Application.DTO.ParrillaPromocionalDTO;
using Application.DTO.TablaBonificacionesDTO;
using Application.Interfaces.IArchivo;
using Application.Interfaces.ISistema;
using Application.Services;
using Domain.Entities.Auditoria;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers.Sistema
{
    [Route("api/[controller]")]
    [ApiController]
    public class SistemaController : ControllerBase
    {
        private readonly ISistemaService _service;
        private readonly IArchivoService _archivoService;
        private readonly IWebHostEnvironment _env;

        public SistemaController(ISistemaService service, IArchivoService archivoService, IWebHostEnvironment env)
        {
            _service = service;
            _archivoService = archivoService;
            _env = env;
        }

        [HttpGet("ciclos/select")]
        public async Task<IActionResult> GetCiclosSelect()
        {
            var ciclos = await _service.ObtenerCiclosSelectAsync();
            return Ok(ciclos);
        }

        [HttpGet("fuerzas/select")]
        public async Task<IActionResult> GetFuerzasSelect()
        {
            var fuerzas = await _service.ObtenerFuerzasSelectAsync();
            return Ok(fuerzas);
        }

        [HttpGet("secciones/select")]
        public async Task<IActionResult> GetSeccionesSelect()
        {
            var secciones = await _service.ObtenerSeccionesSelectAsync();
            return Ok(secciones);
        }

        [HttpGet("guia-producto/select")]
        public async Task<ActionResult<GuiaProductoSelectsDTO>> ObtenerSelects()
        {
            var selects = await _service.ObtenerSelectsAsync();
            return Ok(selects);
        }


        [HttpPost("guia-producto")]
        public async Task<IActionResult> CrearGuiaProducto([FromBody] CrearGuiaProductoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rutaBase = Path.Combine(_env.ContentRootPath, "Storage");

            var id = await _service.CrearGuiaProductoAsync(dto, rutaBase);

            return Ok(new { id });
        }

        [HttpGet("guia-producto")]
        public async Task<IActionResult> ObtenerGuiasProducto()
        {
            var guias = await _service.ObtenerGuiasProductoAsync();
            return Ok(guias);
        }

        [HttpGet("guia-producto/{id}")]
        public async Task<IActionResult> ObtenerGuiaDetalle(int id)
        {
            var dto = await _service.ObtenerGuiaDetalleAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpDelete("guia-producto/{id}")]
        public async Task<IActionResult> EliminarGuiaProducto(int id)
        {
            var rutaBase = Path.Combine(_env.ContentRootPath, "Storage");
            try
            {
                await _service.EliminarGuiaAsync(id, rutaBase);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Guía no encontrada.");
            }
        }

        [HttpPut("guia-producto/{id}")]
        public async Task<IActionResult> ActualizarGuiaProducto(int id, [FromBody] UpdateGuiaProductoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID del cuerpo no coincide con el de la URL.");

            try
            {
                var rutaBase = Path.Combine(_env.ContentRootPath, "Storage");
                await _service.ActualizarGuiaProductoAsync(dto, rutaBase);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la guía: {ex.Message}");
            }
        }

        [HttpDelete("guia-producto/archivo/{archivoId}")]
        public async Task<IActionResult> EliminarArchivoGuia(int archivoId)
        {
            try
            {
                var rutaBase = Path.Combine(_env.ContentRootPath, "Storage");
                await _service.EliminarArchivoGuiaProductoAsync(archivoId, rutaBase);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Archivo no encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar archivo: {ex.Message}");
            }
        }

        [HttpGet("guia-producto/descargar")]
        public IActionResult DescargarGuia([FromQuery] string rutaRelativa, [FromQuery] int entidadId)
        {
            var archivo = _archivoService.ObtenerArchivo(rutaRelativa, _env.ContentRootPath, "GuiaProducto", entidadId, "descargar", ModulosAuditoria.Sistema);
            return File(archivo.Contenido, archivo.Mime, archivo.Nombre);
        }

        [HttpGet("guia-producto/ver")]
        public IActionResult VerGuia([FromQuery] string rutaRelativa, [FromQuery] int entidadId)
        {
            var archivo = _archivoService.ObtenerArchivo(rutaRelativa, _env.ContentRootPath, "GuiaProducto", entidadId, "ver", ModulosAuditoria.Sistema);
            return File(archivo.Contenido, archivo.Mime, null);
        }

        // PARRILLA PROMOCIONAL

        [HttpPost("parrilla-promocional")]
        public async Task<IActionResult> CrearParrillaPromocional([FromBody] CrearParrillaPromocionalDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rutaBase = Path.Combine(_env.ContentRootPath, "Storage");

            var id = await _service.GuardarParrillaPromocionalAsync(dto, rutaBase);

            return Ok(new { id });
        }

        [HttpGet("parrilla-promocional")]
        public async Task<ActionResult<ParrillaPromocionalDTO>> Obtener()
        {
            var resultado = await _service.ObtenerAsync();
            if (resultado == null)
                return NotFound("No existe una parrilla promocional registrada.");

            return Ok(resultado);
        }

        [HttpDelete("parrilla-promocional-archivo")]
        public async Task<IActionResult> EliminarArchivo()
        {
            try
            {
                var rutaBase = Path.Combine(_env.ContentRootPath, "Storage");
                await _service.EliminarArchivoParrillaAsync(rutaBase);
                return NoContent();
            }
            catch (FileNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("parrilla-promocional/descargar")]
        public IActionResult DescargarManual([FromQuery] string rutaRelativa, [FromQuery] int entidadId)
        {
            var archivo = _archivoService.ObtenerArchivo(rutaRelativa, _env.ContentRootPath, "ParrillaPromocional", entidadId, "descargar", ModulosAuditoria.Sistema);
            return File(archivo.Contenido, archivo.Mime, archivo.Nombre);
        }

        // TABLA BONIFICACIONES

        [HttpPost("tabla-bonificaciones")]
        public async Task<IActionResult> CrearTablaBonificaciones([FromBody] CrearTablaBonificacionesDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rutaBase = Path.Combine(_env.ContentRootPath, "Storage");

            var id = await _service.GuardarTablaBonificacionesAsync(dto, rutaBase);

            return Ok(new { id });
        }

        [HttpGet("tabla-bonificaciones")]
        public async Task<ActionResult<TablaBonificacionesDTO>> ObtenerTablaBonificaciones()
        {
            var resultado = await _service.ObtenerTablaBonificacionesAsync();
            if (resultado == null)
                return NotFound("No existe una tabla de bonificaciones registrada.");

            return Ok(resultado);
        }

        [HttpDelete("tabla-bonificaciones-archivo")]
        public async Task<IActionResult> EliminarArchivoTablaBonificaciones()
        {
            try
            {
                var rutaBase = Path.Combine(_env.ContentRootPath, "Storage");
                await _service.EliminarArchivoTablaBonificacionesAsync(rutaBase);
                return NoContent();
            }
            catch (FileNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("tabla-bonificaciones/descargar")]
        public IActionResult DescargarTablaBonificaciones([FromQuery] string rutaRelativa, [FromQuery] int entidadId)
        {
            var archivo = _archivoService.ObtenerArchivo(rutaRelativa, _env.ContentRootPath, "TablaBonificaciones", entidadId, "descargar", ModulosAuditoria.Sistema);
            return File(archivo.Contenido, archivo.Mime, archivo.Nombre);
        }

    }
}