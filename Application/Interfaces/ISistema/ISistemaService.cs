using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.GuiaProductoDTO;
using Application.DTO.SistemaDTO;

namespace Application.Interfaces.ISistema
{
    public interface ISistemaService
    {
        Task<List<CicloSelectDTO>> ObtenerCiclosSelectAsync();
        Task<string> ObtenerNombreCicloAsync(int cicloId);
        Task<int?> ObtenerIdPorCodigoSeccionAsync(string codigo);
        // GUIAS DE PRODUCTO
        Task<int> CrearGuiaProductoAsync(CrearGuiaProductoDTO dto, string webRootPath);
        Task<List<GuiaProductoDTO>> ObtenerGuiasProductoAsync();

    }
}
