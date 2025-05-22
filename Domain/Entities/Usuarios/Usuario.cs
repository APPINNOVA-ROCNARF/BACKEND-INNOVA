using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities.Usuarios
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool Estado { get; set; }
        public DateTime CreadoEn { get; set; } = DateTime.Now;
        public DateTime ModificadoEn { get; set; } = DateTime.Now;

        public int RolId { get; set; }
        public Rol Rol { get; set; }

        [JsonIgnore]
        public UsuarioWeb UsuarioWeb { get; set; }
        [JsonIgnore]
        public UsuarioApp UsuarioApp { get; set; }
    }
}
