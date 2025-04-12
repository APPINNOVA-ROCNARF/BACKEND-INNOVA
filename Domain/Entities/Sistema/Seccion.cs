using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Sistema
{
    public class Seccion
    {
        public int Id { get; set; }         
        public string Codigo { get; set; }  

        public int RegionId { get; set; }
        public Region Region { get; set; }

        public int FuerzaId { get; set; }
        public Fuerza Fuerza { get; set; }
    }

}
