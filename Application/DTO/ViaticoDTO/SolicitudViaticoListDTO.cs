using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ViaticoDTO
{
    public class SolicitudViaticoListDTO
    {
        public int Id { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public string CicloNombre { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }
}
