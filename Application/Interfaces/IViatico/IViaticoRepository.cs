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
        Task<RelacionViaticoFactura?> ObtenerRelacionConFacturaYViaticoAsync(int facturaId);
        void MarcarModificado<T>(T entidad) where T : class, IModificado;
        Task<SubcategoriaViatico?> ObtenerPorIdAsync(int? id);
        Task SaveChangesAsync();
        Task<List<int>> ObtenerIdsFacturasPorViaticoAsync(int viaticoId);
        Task<List<Viatico>> ObtenerConSolicitudYCategoriaPorFiltroAsync(int? cicloId, DateTime? fechaInicio, DateTime? fechaFin);
        Task<List<CupoMensual>> ObtenerCuposMensualesAsync(List<int> usuarioIds, List<int> ciclosIds);
    }
}
