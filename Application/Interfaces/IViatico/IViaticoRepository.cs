using Application.DTO.ViaticoDTO;
using Domain.Common;
using Domain.Entities.Viaticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IViatico
{
    public interface IViaticoRepository
    {
        Task<int> CrearViaticoAsync(CrearViaticoDTO dto);
        Task<EstadisticaSolicitudViaticoDTO?> ObtenerEstadisticaSolicitudViaticoAsync(int cicloId);
        Task<List<EstadisticaViaticoDTO>> ObtenerEstadisticaViaticoAsync(int solicitudId);
        Task<IEnumerable<ViaticoListDTO>> ObtenerViaticosPorSolicitudAsync(int solicitudId);
        Task<List<Viatico>> ObtenerViaticosPorIdsAsync(List<int> ids);
        Task ActualizarViaticosAsync(List<Viatico> viaticos);
        Task<Viatico?> GetIdPorFacturaAsync(int id);
        void MarcarModificado<T>(T entidad) where T : class, IModificado;
        Task SaveChangesAsync();
    }
}
