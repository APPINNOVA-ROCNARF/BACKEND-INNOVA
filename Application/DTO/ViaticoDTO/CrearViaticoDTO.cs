using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Viaticos;

namespace Application.DTO.ViaticoDTO
{
    public class CrearViaticoDTO
    {
        public Viatico Viatico { get; set; }
        public FacturaViatico Factura { get; set; }
        public int UsuarioAppId { get; set; }
        public int CicloId { get; set; }
        public decimal Monto { get; set; }
    }
}
