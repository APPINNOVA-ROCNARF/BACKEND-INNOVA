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
        public Categoria? Categoria { get; set; }

        public int? FacturaId { get; set; }
        public Factura? Factura { get; set; }

        public string? PlacaVehiculo { get; set; }
        public Vehiculo? Vehiculo { get; set; }
        public EstadoViatico EstadoViatico { get; set; }
        public EstadoCicloViatico EstadoCiclo { get; set; }

        public string? Comentario { get; set; }

        public List<CampoRechazado>? CamposRechazados { get; set; }
    }
}
