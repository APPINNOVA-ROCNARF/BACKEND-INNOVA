using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IAuth
{
    public interface IAuthService
    {
        Task<string> AuthenticateUser(string email, string password);
    }
}
