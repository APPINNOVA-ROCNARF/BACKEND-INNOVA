using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ViaticoDTO
{
    public class FacturaViaticoDTO
    {
        public string NumeroFactura { get; set; } = default!;
        public DateTime FechaFactura { get; set; }

        public decimal Subtotal { get; set; }
        public decimal SubtotalIva { get; set; }
        public decimal Total { get; set; }

        public string RucProveedor { get; set; } = default!;
        public string RazonSocialProveedor { get; set; } = default!;
        public string RutaTemporal { get; set; } = default!; 
    }
}
