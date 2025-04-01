using Application.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers.Roles
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
    }
}
