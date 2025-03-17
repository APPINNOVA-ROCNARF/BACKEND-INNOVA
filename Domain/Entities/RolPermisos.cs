using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RolPermisos
    {
        public int RolId { get; set; }
        public Rol Rol { get; set; }
        public int PermisoId { get; set; }
        public Permisos Permiso { get; set; }
        public int AccionId { get; set; }
        public Accion Accion { get; set; }
    }
}
