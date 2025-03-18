using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Modulo
    {
        public int Id { get; set; }

        [Required]
        public required string Nombre { get; set; }

        public string? Icono { get; set; } 

        public int? Orden { get; set; } 

        public bool Estado { get; set; } = true; 

        public ICollection<RolModulos> RolModulos { get; set; } = new List<RolModulos>();

        public ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
    }
}
