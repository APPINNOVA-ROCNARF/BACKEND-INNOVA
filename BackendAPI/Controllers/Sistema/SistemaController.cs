using Application.DTO.GuiaProductoDTO;
using Application.DTO.ParrillaPromocionalDTO;
using Application.Interfaces.ISistema;
using Application.Services;
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

        [HttpGet("fuerzas/select")]
        public async Task<IActionResult> GetFuerzasSelect()
        {
            var fuerzas = await _service.ObtenerFuerzasSelectAsync();
            return Ok(fuerzas);
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
    }
}
