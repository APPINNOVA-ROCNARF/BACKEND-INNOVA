using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities.Viaticos
{
    public class VehiculoPrincipal : IModificado
    {
        public int UsuarioAppId { get; set; }
        public int VehiculoId { get; set; }

        public DateTime FechaModificado { get; set; }
        public Vehiculo Vehiculo { get; set; } = null!;
    }
}
