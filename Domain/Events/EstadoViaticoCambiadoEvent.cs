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
        public EstadoViatico EstadoAnterior { get; }
        public EstadoViatico EstadoNuevo { get; }
        public int UsuarioId { get; }
        public DateTime FechaEvento { get; }

        public EstadoViaticoCambiadoEvent(int viaticoId, EstadoViatico anterior, EstadoViatico nuevo, int usuarioId)
        {
            ViaticoId = viaticoId;
            EstadoAnterior = anterior;
            EstadoNuevo = nuevo;
            UsuarioId = usuarioId;
            FechaEvento = DateTime.UtcNow;
        }
    }
}
