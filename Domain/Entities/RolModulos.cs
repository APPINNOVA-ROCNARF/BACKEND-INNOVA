using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RolModulos
    {
        public int RolId { get; set; }
        public int ModuloId { get; set; }
        public Rol Rol { get; set; }
        public Modulo Modulo { get; set; }
    }
}
