using Application.Interfaces.IAuth;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Application.DTO;

namespace BackendAPI.Controllers.Auth;

[Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var token = await _authService.AuthenticateUser(request.Email, request.Password);
            if (token == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            return Ok(new { token });
        }

    }

