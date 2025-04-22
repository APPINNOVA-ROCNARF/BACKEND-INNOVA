using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Viaticos;

namespace Application.Helpers
{
    public static class EstadoSolicitudExtension
    {
        public static string ToFriendlyString(this EstadoSolicitud estado)
        {
            return estado switch
            {
                EstadoSolicitud.NoEnviada => "No enviada",
                EstadoSolicitud.EnRevision => "En revisión",
                EstadoSolicitud.Aprobado => "Aprobado",
                EstadoSolicitud.Rechazado => "Rechazado",
                EstadoSolicitud.ParaCorregir => "Para corregir",
                _ => "Desconocido"
            };
        }
    }
}
