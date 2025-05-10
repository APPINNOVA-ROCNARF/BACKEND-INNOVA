using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Application.Interfaces.IUsuario
{
    public interface IUsuarioActualService
    {
        UsuarioActual Obtener();
    }
}
