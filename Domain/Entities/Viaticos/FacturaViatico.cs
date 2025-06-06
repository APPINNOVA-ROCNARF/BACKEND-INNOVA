﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Viaticos
{
    public class FacturaViatico
    {
        public int Id { get; set; }             
        public string NumeroFactura { get; set; } = string.Empty;
        public DateTime FechaFactura { get; set; }

        public string RucProveedor { get; set; } = string.Empty;   
        public ProveedorViatico? Proveedor { get; set; }

        public decimal Subtotal { get; set; }
        public decimal SubtotalIva { get; set; }
        public decimal Total { get; set; }
        public string RutaImagen { get; set; } = string.Empty;
    }
}
