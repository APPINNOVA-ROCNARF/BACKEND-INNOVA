using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Viaticos;

namespace Application.Interfaces.IViatico
{
    public interface ISolicitudViaticoRepository
    {
        Task<SolicitudViatico?> ObtenerPorCicloUsuarioAsync(int cicloId, int usuarioAppId);
        Task CrearAsync(SolicitudViatico solicitud);
        Task ActualizarMontoAsync(SolicitudViatico solicitud);
        Task<List<SolicitudViatico>> ObtenerSolicitudPorCicloAsync(int cicloId);
        Task<SolicitudViatico?> ObtenerDetalleSolicitud(int solicitudId);
        Task<SolicitudViatico> ObtenerViaticosPorIdAsync(int solicitudId);
        Task ActualizarEstadoAsync(SolicitudViatico solicitud);
    }
}
