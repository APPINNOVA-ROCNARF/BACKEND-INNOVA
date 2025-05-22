using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ViaticoDTO
{
    public class FacturaDTO
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; }
        public DateTime FechaFactura { get; set; }
        public string ProveedorNombre { get; set; }
        public string RucProveedor { get; set; }
        public decimal Monto { get; set; }
        public string RutaImagen { get; set; }
    }
}
