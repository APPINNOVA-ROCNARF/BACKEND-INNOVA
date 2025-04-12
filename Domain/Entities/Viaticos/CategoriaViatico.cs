using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Viaticos
{
    public class CategoriaViatico
    {
        public int Id { get; set; }                   
        public string Nombre { get; set; } = string.Empty;
        public bool Estado { get; set; } = true;               

        public ICollection<Viatico>? Viaticos { get; set; }    
    }
}
