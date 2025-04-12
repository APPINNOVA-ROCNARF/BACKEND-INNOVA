using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Viaticos
{
    public class Viatico
    {
        public int Id { get; set; }

        public DateTime FechaRegistro { get; set; }

        public int IdUsuario { get; set; }

        public int CicloId { get; set; }

        public int CategoriaId { get; set; }
        public CategoriaViatico? Categoria { get; set; }

        public int? FacturaId { get; set; }
        public FacturaViatico? Factura { get; set; }

        public string? PlacaVehiculo { get; set; }
        public Vehiculo? Vehiculo { get; set; }
        public EstadoViatico EstadoViatico { get; set; }
        public EstadoCicloViatico EstadoCiclo { get; set; }

        public string? Comentario { get; set; }

        public List<CampoRechazado>? CamposRechazados { get; set; }
    }
}
