using Application.DTO.ViaticoDTO;
using Application.DTO.ViaticoDTO.mobile;
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
        void MarcarModificado<T>(T entidad) where T : class, IModificado;
        Task<SubcategoriaViatico?> ObtenerSubcategoriaPorIdAsync(int? id);
        Task SaveChangesAsync();
        Task<Viatico?> GetIdPorFacturaAsync(int id);
        Task<List<Viatico>> ObtenerConSolicitudYCategoriaPorFiltroAsync(int? cicloId, DateTime? fechaInicio, DateTime? fechaFin);
        Task<List<CupoMensual>> ObtenerCuposMensualesAsync(List<int> usuarioIds, List<int> ciclosIds);

        // APP MOVIL
        Task<IEnumerable<AppViaticoListDTO>> ObtenerViaticosApp(int solicitudId);

    }
}
