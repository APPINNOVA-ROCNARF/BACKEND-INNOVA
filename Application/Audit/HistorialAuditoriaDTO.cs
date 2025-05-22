using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Audit
{
    public class HistorialAuditoriaDTO
    {
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string TipoEvento { get; set; } = string.Empty;
        public string? Campo { get; set; }
        public string? ValorAnterior { get; set; }
        public string? ValorNuevo { get; set; }
    }
}
