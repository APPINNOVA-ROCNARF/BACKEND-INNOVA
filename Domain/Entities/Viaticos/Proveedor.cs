using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Viaticos
{
    public class Proveedor
    {
        public string Ruc { get; set; } = string.Empty;      
        public string RazonSocial { get; set; } = string.Empty;

        public ICollection<Factura>? Facturas { get; set; }  
    }
}
