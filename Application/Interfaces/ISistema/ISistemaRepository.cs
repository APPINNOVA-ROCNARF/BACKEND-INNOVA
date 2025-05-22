using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.GuiaProductoDTO;
using Application.DTO.SistemaDTO;
using Domain.Entities.Sistema;

namespace Application.Interfaces.ISistema
{
    public interface ISistemaRepository
    {
        Task<List<CicloSelectDTO>> ObtenerCiclosSelectAsync();
        Task<string> ObtenerNombreCicloAsync(int cicloId);
        Task<int?> ObtenerIdPorCodigoSeccionAsync(string codigo);

        //GUIA DE PRODUCTOS
        Task<int> InsertarAsync(GuiaProducto guia);
        Task InsertarArchivosAsync(IEnumerable<ArchivoGuiaProducto> archivos);
        Task<List<GuiaProductoDTO>> ObtenerGuiasProductoAsync();

    }
}
