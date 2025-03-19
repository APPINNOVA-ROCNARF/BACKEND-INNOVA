using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackendAPI.Controllers.Usuarios;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _userService;

    public UsuarioController(IUsuarioService userService)
    {
        _userService = userService;
    }

    // Obtener todos los usuarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsers()
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


    // Crear WebUser
    [HttpPost("usuarioweb")]
    public async Task<ActionResult<UsuarioWeb>> CrearUsuarioWeb([FromBody] NewUsuarioWebDTO usuarioDto)
    {
        var usuarioWeb = await _userService.CrearUsuarioWebAsync(usuarioDto);
        return CreatedAtAction(nameof(CrearUsuarioWeb), new { id = usuarioWeb.Id }, usuarioWeb);
    }

    // Actualizar WebUser
    [HttpPut("usuarioweb/{id}")]
    public async Task<IActionResult> UpdateWebUser(int id, [FromBody] UsuarioWeb usuarioWeb)
    {
        if (id != usuarioWeb.Id) return BadRequest("El ID del WebUser no coincide.");

        await _userService.UpdateWebUserAsync(usuarioWeb);
        return NoContent();
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
