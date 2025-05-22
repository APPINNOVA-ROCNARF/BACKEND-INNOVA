using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities.Viaticos
{
    public class SolicitudVehiculoPrincipal : ICreado
    {
        public int Id { get; set; }

        public int UsuarioAppId { get; set; }
        public int VehiculoIdSolicitado { get; set; }

        public string Motivo { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }

        public int? AprobadoPorUsuarioId { get; set; } 
        public DateTime? FechaAprobacion { get; set; }
        public EstadoSolicitudVehiculo Estado { get; set; }
        public Vehiculo? Vehiculo { get; set; }
    }
}
