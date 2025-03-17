using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Permisos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public string Modulo { get; set; }
        public string Categoria { get; set; }
        public List<RolPermisos> RolPermisos { get; set; } = new();
    }
}
