using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtService _jwtService;

        public AuthService(IAuthRepository authRepository, JwtService jwtService)
        {
            _authRepository=authRepository;
            _jwtService=jwtService;
        }

        public async Task<string> AuthenticateUser(string email, string password)
        {
            var usuario = await _authRepository.GetUsuarioByEmail(email);
            if (usuario == null || usuario.Password != password)
            {
                return null;
            }

            string role = usuario.UsuarioWeb?.Rol?.Nombre ?? "UsuarioApp";

            return _jwtService.GenerateToken(usuario.Id, usuario.Email, role);

        }
    }
}
