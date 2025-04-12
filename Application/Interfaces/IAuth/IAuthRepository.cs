using Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IAuth
{
    public interface IAuthRepository
    {
        Task<Usuario> GetUsuarioByEmail(string email);
    }
}
