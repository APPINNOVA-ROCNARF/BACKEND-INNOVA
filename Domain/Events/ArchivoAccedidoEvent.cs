using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Events
{
    public class ArchivoAccedidoEvent : IDomainEvent
    {
        public string NombreArchivo { get; }
        public string RutaRelativa { get; }
        public string ModoAcceso { get; }
        public string Modulo {  get; }
        public string Entidad { get; }
        public int EntidadId { get; }
        public DateTime FechaEvento { get; }

        public ArchivoAccedidoEvent(string nombreArchivo, string rutaRelativa, string modoAcceso, string modulo, string entidad, int entidadId)
        {
            NombreArchivo = nombreArchivo;
            RutaRelativa = rutaRelativa;
            ModoAcceso  = modoAcceso;
            Modulo = modulo;
            Entidad = entidad;
            EntidadId = entidadId;
            FechaEvento = DateTime.UtcNow;
        }
    }
}
