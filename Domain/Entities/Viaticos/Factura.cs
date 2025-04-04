using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Viaticos
{
    public class Factura
    {
        public int Id { get; set; }             
        public string NumeroFactura { get; set; } = string.Empty;
        public DateTime FechaFactura { get; set; }

        public string RucProveedor { get; set; } = string.Empty;   
        public Proveedor? Proveedor { get; set; }

        public decimal Subtotal { get; set; }
        public decimal SubtotalIva { get; set; }
        public decimal Total { get; set; }
        public string RutaImagen { get; set; } = string.Empty;
        public ICollection<Viatico>? Viaticos { get; set; }        
    }
}
