using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        [JsonIgnore]
        public virtual ICollection<UsuarioWeb> UsuariosWeb { get; set; }
        public virtual List<RolPermisos>? RolPermisos { get; set; }
    }
}
