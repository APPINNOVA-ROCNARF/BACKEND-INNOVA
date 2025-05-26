using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Events
{
    public class FacturaEditadaEvent : IDomainEvent
    {
        public int FacturaId { get; }
        public string Campo { get; }
        public string ValorAnterior { get; }
        public string ValorNuevo { get; }
        public DateTime FechaEvento {  get; }

        public FacturaEditadaEvent(int facturaId, string campo, string valorAnterior, string valorNuevo)
        {
            FacturaId = facturaId;
            Campo = campo;
            ValorAnterior = valorAnterior;
            ValorNuevo = valorNuevo;
            FechaEvento = DateTime.UtcNow;
        }
    }
}
