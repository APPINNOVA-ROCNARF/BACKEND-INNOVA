using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Entities.Usuarios;

namespace Application.DTO.UsuarioDTO
{
    public class UsuarioListDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime ModificadoEn { get; set; }

    }
}
