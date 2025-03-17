using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UsuarioWeb
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }
        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
