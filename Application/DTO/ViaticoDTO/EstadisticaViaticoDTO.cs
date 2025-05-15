using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ViaticoDTO
{
    public class EstadisticaViaticoDTO
    {
        public string categoria { get; set; } = null!;
        public decimal total_registrado { get; set; }
        public decimal total_aprobado { get; set; }
        public decimal total_acreditado { get; set; }
        public decimal diferencia { get; set; }
        public decimal porcentaje_ejecucion { get; set; }
    }
}
