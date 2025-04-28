using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ViaticoDTO
{
    public class DetalleSolicitudDTO
    {
        public string UsuarioNombre { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string CicloNombre { get; set; } = string.Empty;
    }
}
