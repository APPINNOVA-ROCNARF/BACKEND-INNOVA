using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Sistema
{
    public class Region
    {
        public int Id { get; set; }           
        public string Nombre { get; set; }    
        public ICollection<Seccion> Secciones { get; set; }
    }
}
