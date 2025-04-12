using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Usuarios
{
    public class Permiso
    {
        public int Id { get; set; }

        [Required]
        public required string Nombre { get; set; }

        [Required]
        public required string Ruta { get; set; }

        public int ModuloId { get; set; }

        public Modulo Modulo { get; set; }

        public ICollection<RolPermisos> RolPermisos { get; set; } = new List<RolPermisos>();
    }
}
