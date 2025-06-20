using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ArchivoDTO;

namespace Application.Interfaces.IArchivo
{
    public interface IArchivoRepository
    {
        Task<string> GuardarArchivoTempAsync(ArchivoUploadDTO archivoDto, string webRootPath);
        Task<string> MoverArchivoFinalAsync(MoverArchivoDTO dto, string rutaBase);
        Task<List<string>> MoverArchivosAGuiaProductoAsync(
    List<MoverArchivoGuiaDTO> archivos,
    int guiaProductoId,
    string rutaBase);

        Task<string> MoverArchivosParrillaPromocionalAsync(MoverArchivoGuiaDTO archivo, int parrillaPromocionalId, string rutaBase);
        Task<string> MoverArchivosTablaBonificacionesAsync(MoverArchivoGuiaDTO archivo, int tablaBonificacionesId, string rutaBase);
    }

}
