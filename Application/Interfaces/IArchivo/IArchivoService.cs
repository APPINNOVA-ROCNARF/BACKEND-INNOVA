using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ArchivoDTO;

namespace Application.Interfaces.IArchivo
{
    public interface IArchivoService
    {
        Task<string> SubirArchivoTempAsync(ArchivoUploadDTO archivoDto, string webRootPath);
    }
}
