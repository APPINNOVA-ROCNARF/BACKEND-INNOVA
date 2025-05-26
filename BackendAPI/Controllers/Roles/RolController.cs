using Application.DTO.RolDTO;
using Application.Interfaces.IRol;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers.Roles
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _rolService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{rolId}")]
        public async Task<IActionResult> GetRolConModulos(int rolId)
        {
            var rol = await _rolService.GetRolConModulosAsync(rolId);
            if (rol == null)
                return NotFound($"No se encontró el rol con ID {rolId}");

            return Ok(rol);
        }

        [HttpGet("modulos")]
        public async Task<IActionResult> GetModulos()
        {
            var modulos = await _rolService.GetModulosAsync();
            return Ok(modulos);
        }

        [HttpPost]
        public async Task<IActionResult> CrearRol([FromBody] CrearRolRequestDTO dto)
        {
            try
            {
                await _rolService.CrearRolAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error al crear el rol", detalle = ex.Message });
            }
        }
    }
}
