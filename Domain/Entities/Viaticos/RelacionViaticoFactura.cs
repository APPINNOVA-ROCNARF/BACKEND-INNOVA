using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Viaticos
{
    public class RelacionViaticoFactura
    {
        public int ViaticoId { get; set; }
        public Viatico Viatico { get; set; }

        public int FacturaId { get; set; }
        public FacturaViatico Factura { get; set; }
    }
}
