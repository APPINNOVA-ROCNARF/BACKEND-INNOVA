using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities.Viaticos
{
    public class Vehiculo : ICreado
    {
        public int Id { get; set; }
        public required int UsuarioAppId {  get; set; } 
        public required string Placa { get; set; }
        public required string Fabricante { get; set; }
        public required string Modelo { get; set; }
        public required string Color { get; set; }
        public DateTime FechaRegistro { get; set; }

        public ICollection<Viatico>? Viaticos { get; set; }      
    }
}
