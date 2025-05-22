using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Viaticos
{
    public class SubcategoriaViatico
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int CategoriaId { get; set; }
        public CategoriaViatico Categoria { get; set; }

        public int FacturasRequeridas { get; set; }

        public ICollection<Viatico> Viaticos { get; set; }
    }
}
