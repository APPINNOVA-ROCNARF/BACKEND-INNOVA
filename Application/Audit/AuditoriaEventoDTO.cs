using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Audit
{
    public class AuditoriaEventoDTO
    {
        public string Modulo { get; set; } = string.Empty;
        public string TipoEvento { get; set; } = string.Empty;
        public object Datos { get; set; } = new();
        public string? Entidad { get; set; }
        public int? EntidadId { get; set; }
        public int? UsuarioId { get; set; }
    }
}
