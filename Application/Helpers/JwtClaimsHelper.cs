using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    class JwtClaimsHelper
    {
        public static List<Claim> GenerateClaims(int userId, string email, string role, string nombre)
        {
            return new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(ClaimTypes.Name, nombre), 
        new Claim(ClaimTypes.Role, role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
        }
    }
}
