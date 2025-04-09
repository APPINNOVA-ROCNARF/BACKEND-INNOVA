using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        [Required]
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }

        [JsonIgnore]
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        [JsonIgnore]
        public ICollection<RolModulos> RolModulos { get; set; } = new List<RolModulos>();

        public ICollection<RolPermisos> RolPermisos { get; set; } = new List<RolPermisos>();
    }
}
