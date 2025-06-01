using Application.DTO;
using Application.DTO.UsuarioDTO;
using Application.Interfaces.IUsuario;
using Application.Services;
using Domain.Entities.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackendAPI.Controllers.Usuarios;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _userService;

    public UsuarioController(IUsuarioService userService)
    {
        _userService = userService;
    }

    // Obtener todos los usuarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioListDTO>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    // Obtener usuario por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }


    // Eliminar un usuario
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpPut("usuarioweb/{id}")]
    public async Task<IActionResult> UpdateWebUser(int id, [FromBody] UsuarioWeb usuarioWeb)
    {
        if (id != usuarioWeb.Id) return BadRequest("El ID del WebUser no coincide.");

        await _userService.UpdateWebUserAsync(usuarioWeb);
        return NoContent();
    }

    // Obtener Select de Usuarios App
    [HttpGet("usuariosapp/select")]
    public async Task<ActionResult<List<UsuarioAppSelectDTO>>> ObtenerUsuariosSelect()
    {
        var usuarios = await _userService.ObtenerUsuariosAppSelectDTOAsync();
        return Ok(usuarios);
    }

    // Obtener Menú de Usuario
    [HttpGet("menu")]
    public async Task<IActionResult> GetMisModulos()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var modulos = await _userService.GetModulosUsuarioAsync(email);
        return Ok(modulos);
    }

}
