using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Entities.Viaticos;

namespace Domain.Events
{
    public class ViaticoCreadoEvent : IDomainEvent
    {
        public int ViaticoId { get; }
        public DateTime FechaEvento { get; }
        public int CategoriaId { get; }
        public int? SubcategoriaId { get; }
        public decimal Monto { get; }

        public ViaticoCreadoEvent(int viaticoId, int categoriaId, int? subcategoriaId, decimal monto)
        {
            ViaticoId = viaticoId;
            CategoriaId = categoriaId;
            SubcategoriaId = subcategoriaId;
            Monto = monto;
            FechaEvento = DateTime.UtcNow;
        }
    }
}
