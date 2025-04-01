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
        public int? PlataformaId { get; set; }
        public Plataforma Plataforma { get; set; }

        [JsonIgnore]
        public ICollection<UsuarioWeb> UsuariosWeb { get; set; } = new List<UsuarioWeb>();
        public ICollection<UsuarioApp> UsuariosApp { get; set; }

        [JsonIgnore]
        public ICollection<RolModulos> RolModulos { get; set; } = new List<RolModulos>(); 

        public ICollection<RolPermisos> RolPermisos { get; set; } = new List<RolPermisos>(); 
    }
}
