using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UsuarioApp
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string NombreUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
