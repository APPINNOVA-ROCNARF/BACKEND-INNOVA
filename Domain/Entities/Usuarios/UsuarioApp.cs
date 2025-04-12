using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities.Usuarios
{
    public class UsuarioApp
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string NombreUsuario { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }
        public ICollection<UsuarioAppSeccion> UsuarioSecciones { get; set;}
    }
}
