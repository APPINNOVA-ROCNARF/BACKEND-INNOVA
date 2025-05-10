using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.IUsuario;
using Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class UsuarioActualService : IUsuarioActualService
    {
        private readonly IHttpContextAccessor _http;

        public UsuarioActualService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public UsuarioActual Obtener()
        {
            var user = _http.HttpContext?.User;
            if (user == null) throw new UnauthorizedAccessException("No hay usuario autenticado.");

            return new UsuarioActual
            {
                Id = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"),
                Nombre = user.FindFirst(ClaimTypes.Name)?.Value ?? "",
                Rol = user.FindFirst(ClaimTypes.Role)?.Value ?? ""
            };
        }
    }
}
