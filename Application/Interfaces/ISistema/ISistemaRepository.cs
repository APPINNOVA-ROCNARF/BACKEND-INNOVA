using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.GuiaProductoDTO;
using Application.DTO.ParrillaPromocionalDTO;
using Application.DTO.SistemaDTO;
using Domain.Entities.Sistema;

namespace Application.Interfaces.ISistema
{
    public interface ISistemaRepository
    {
        Task<List<CicloSelectDTO>> ObtenerCiclosSelectAsync();
        Task<List<FuerzaSelectDTO>> ObtenerFuerzasSelectAsync();
        Task<string> ObtenerNombreCicloAsync(int cicloId);
        Task<int?> ObtenerIdPorCodigoSeccionAsync(string codigo);
        Task<List<SeccionSelectDTO>> ObtenerSeccionSelectAsync();
        //GUIA DE PRODUCTOS
        Task<int> InsertarAsync(GuiaProducto guia);
        Task InsertarArchivosAsync(IEnumerable<ArchivoGuiaProducto> archivos);
        Task<List<GuiaProductoDTO>> ObtenerGuiasProductoAsync();
        Task<GuiaProductoDetalleDTO?> ObtenerGuiaDetalleAsync(int id);
        Task EliminarGuiaAsync(int id, string rutaBase);
        Task<GuiaProducto?> ObtenerGuiaPorIdAsync(int id);
        Task ActualizarGuiaAsync(GuiaProducto guia);
        Task EliminarArchivoAsync(int archivoId, string rutaBase);

        //PARRILLA PROMOCIONAL
        Task<int> GuardarParrillaPromocionalAsync(CrearParrillaPromocionalDTO dto, string rutaBase);
        Task<ParrillaPromocional?> ObtenerParrillaAsync();
        Task EliminarArchivoParrillaAsync(string rutaBase);
    }
}
