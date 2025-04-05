using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Viaticos
{
    public class Vehiculo
    {
        public string Placa { get; set; } = string.Empty;        

        public ICollection<Viatico>? Viaticos { get; set; }      
    }
}
