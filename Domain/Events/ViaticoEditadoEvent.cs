using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Events
{
    public class ViaticoEditadoEvent : IDomainEvent
    {
        public int ViaticoId { get; }
        public string CampoEditado { get; }
        public string ValorAnterior { get; }
        public string ValorNuevo { get; }
        public DateTime FechaEvento { get; }

        public ViaticoEditadoEvent(int viaticoId, string campoEditado, string valorAnterior, string valorNuevo)
        {
            ViaticoId = viaticoId;
            CampoEditado = campoEditado;
            ValorAnterior = valorAnterior;
            ValorNuevo = valorNuevo;
            FechaEvento = DateTime.Now;
        }
    }
}
