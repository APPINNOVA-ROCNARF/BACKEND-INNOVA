using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities.Viaticos
{
    public class Viatico : ICreado, IModificado
    {
        public int Id { get; set; }

        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificado { get; set; }
        public int SolicitudViaticoId { get; set; }
        public SolicitudViatico SolicitudViatico { get; set; }
        public int CategoriaId { get; set; }
        public CategoriaViatico? Categoria { get; set; }
        public int? SubcategoriaId { get; set; }
        public SubcategoriaViatico Subcategoria { get; set; }
        public int? VehiculoId { get; set; }
        public Vehiculo? Vehiculo { get; set; }
        public EstadoViatico EstadoViatico { get; set; }
        public string? Comentario { get; set; }

        public List<CampoRechazado>? CamposRechazados { get; set; }
        public decimal Monto { get; set; }
        public int FacturaId { get; set; }
        public FacturaViatico Factura {  get; set; }
    }
}
