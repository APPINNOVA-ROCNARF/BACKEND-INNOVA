using Domain.Common;
using Domain.Entities.Viaticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class EstadoViaticoCambiadoEvent : IDomainEvent
    {
        public int ViaticoId { get; }
        public string Campo { get; }
        public EstadoViatico ValorAnterior { get; }
        public EstadoViatico ValorNuevo { get; }
        public DateTime FechaEvento { get; }

        public EstadoViaticoCambiadoEvent(int viaticoId, string campo, EstadoViatico anterior, EstadoViatico nuevo)
        {
            ViaticoId = viaticoId;
            Campo = campo;
            ValorAnterior = anterior;
            ValorNuevo = nuevo;
            FechaEvento = DateTime.Now;
        }
    }
}
