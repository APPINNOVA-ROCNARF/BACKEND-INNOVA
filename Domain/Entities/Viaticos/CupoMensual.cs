using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities.Viaticos
{
    public class CupoMensual : ICreado
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int CicloId { get; set; }
        public string Categoria { get; set; } = null!; 
        public decimal MontoAsignado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
