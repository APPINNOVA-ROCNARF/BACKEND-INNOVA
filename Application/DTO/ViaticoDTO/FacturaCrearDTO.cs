using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ArchivoDTO;


namespace Application.DTO.ViaticoDTO
{
    public class FacturaCrearDTO
    {
        public string NumeroFactura { get; set; }
        public DateTime FechaFactura { get; set; }

        public string RucProveedor { get; set; }
        public string ProveedorNombre { get; set; } 

        public decimal Subtotal { get; set; }
        public decimal SubtotalIva { get; set; }
        public decimal Total { get; set; }

        public string RutaTemporal { get; set; } 
    }
}
