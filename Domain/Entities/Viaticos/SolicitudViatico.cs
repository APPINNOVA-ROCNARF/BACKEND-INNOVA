using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities.Viaticos
{
    public class SolicitudViatico : ICreado, IModificado
    {
        public int Id { get; set; }

        public int UsuarioAppId { get; set; }  
        public int CicloId { get; set; }

        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificado { get; set; }
        public decimal Monto { get; set; }
        public EstadoSolicitud Estado { get; set; }
        public ICollection<Viatico> Viaticos { get; set; } = new List<Viatico>();
    }
}
