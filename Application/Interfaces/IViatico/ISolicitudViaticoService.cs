using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ViaticoDTO;
using Domain.Entities.Viaticos;

namespace Application.Interfaces.IViatico
{
    public interface ISolicitudViaticoService
    {
        Task<List<SolicitudViaticoListDTO>> ObtenerSolicitudPorCicloAsync(int cicloId);
        Task<DetalleSolicitudDTO> ObtenerDetalleSolicitud(int solicitudId);
    }
}
