using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ViaticoDTO
{
    public class EstadisticaSolicitudViaticoDTO
    {
        public int ciclo_id { get; set; }
        public long total_solicitudes { get; set; }
        public decimal total_monto { get; set; }
        public decimal total_en_revision { get; set; }
        public decimal total_aprobado { get; set; }
        public decimal total_rechazado { get; set; }
        public long cantidad_en_revision { get; set; }
        public long cantidad_aprobado { get; set; }
        public long cantidad_rechazado { get; set; }
        public decimal monto_movilizacion { get; set; }
        public decimal monto_alimentacion { get; set; }
        public decimal monto_hospedaje { get; set; }
    }
}
