using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Usuarios
{
    public class RolSeccion
    {
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        public int SeccionId { get; set; }
    }
}
