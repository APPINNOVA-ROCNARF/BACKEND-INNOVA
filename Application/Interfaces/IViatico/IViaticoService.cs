using Application.Audit;
using Application.DTO.ViaticoDTO;
using Application.DTO.ViaticoDTO.mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IViatico
{
    public interface IViaticoService
    {
        Task<int> CrearViaticoAsync(ViaticoCrearDTO dto, string rutaBase);

        Task<EstadisticaSolicitudViaticoDTO> ObtenerEstadisticaSolicitudViaticoAsync(int cicloId);
        Task<List<EstadisticaViaticoDTO>> ObtenerEstadisticaViaticoAsync(int solicitudId);
        Task<IEnumerable<ViaticoListDTO>> ObtenerViaticosPorSolicitudAsync(int solicitudId);
        Task ActualizarEstadoViaticosAsync(ActualizarEstadoViaticoRequest request);
        Task EditarCamposFacturaAsync(int id, EditarViaticoDTO dto);
        Task<IList<HistorialAuditoriaDTO>> ObtenerHistorialViaticoAsync(int viaticoId);
        Task<List<ViaticoReporteDTO>> ObtenerResumenPorCategoriaAsync(int? cicloId, DateTime? fechaInicio, DateTime? fechaFin);
        Task<byte[]> GenerarExcelAsync(int? cicloId, DateTime? fechaInicio, DateTime? fechaFin);
        Task<Dictionary<int, bool>> ObtenerFacturasVistasAsync(List<int> facturaIds);

        // APP MOVIL

        Task<IEnumerable<AppViaticoListDTO>> ObtenerViaticosPorUsuarioApp(string nombreUsuario, int cicloId);


    }
}
