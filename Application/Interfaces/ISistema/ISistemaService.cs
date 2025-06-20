using Application.DTO.GuiaProductoDTO;
using Application.DTO.ParrillaPromocionalDTO;
using Application.DTO.SistemaDTO;
using Application.DTO.TablaBonificacionesDTO;

namespace Application.Interfaces.ISistema
{
    public interface ISistemaService
    {
        Task<List<CicloSelectDTO>> ObtenerCiclosSelectAsync();
        Task<List<FuerzaSelectDTO>> ObtenerFuerzasSelectAsync();
        Task<List<SeccionSelectDTO>> ObtenerSeccionesSelectAsync();

        Task<string> ObtenerNombreCicloAsync(int cicloId);
        Task<int?> ObtenerIdPorCodigoSeccionAsync(string codigo);
        // GUIAS DE PRODUCTO
        Task<int> CrearGuiaProductoAsync(CrearGuiaProductoDTO dto, string rutaBase);
        Task<List<GuiaProductoDTO>> ObtenerGuiasProductoAsync();
        Task<GuiaProductoSelectsDTO> ObtenerSelectsAsync();
        Task<GuiaProductoDetalleDTO?> ObtenerGuiaDetalleAsync(int id);
        Task EliminarGuiaAsync(int id, string rutaBase);
        Task EliminarArchivoGuiaProductoAsync(int archivoId, string rutaBase);
        Task ActualizarGuiaProductoAsync(UpdateGuiaProductoDTO dto, string rutaBase);
        // PARRILLA PROMOCIONAL
        Task<int> GuardarParrillaPromocionalAsync(CrearParrillaPromocionalDTO dto, string rutaBase);
        Task<ParrillaPromocionalDTO?> ObtenerAsync();
        Task EliminarArchivoParrillaAsync(string rutaBase);

        // TABLA BONIFICACIONES
        Task<int> GuardarTablaBonificacionesAsync(CrearTablaBonificacionesDTO dto, string rutaBase);
        Task<TablaBonificacionesDTO?> ObtenerTablaBonificacionesAsync();
        Task EliminarArchivoTablaBonificacionesAsync(string rutaBase);
    }
}
