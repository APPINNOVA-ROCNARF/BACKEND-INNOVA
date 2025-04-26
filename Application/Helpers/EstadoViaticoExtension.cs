using Domain.Entities.Viaticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class EstadoViaticoExtension
    {
        public static string ToFriendlyString(this EstadoViatico estado)
        {
            return estado switch
            {
                EstadoViatico.Borrador => "Borrador",
                EstadoViatico.EnRevision => "En revisión",
                EstadoViatico.Aprobado => "Aprobado",
                EstadoViatico.Rechazado => "Rechazado",
                _ => "Desconocido"
            };
        }
    }
}
