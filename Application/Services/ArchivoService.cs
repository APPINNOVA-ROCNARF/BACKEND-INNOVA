using Application.DTO.ArchivoDTO;
using Application.Interfaces.IArchivo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ArchivoService : IArchivoService
    {
        private readonly IArchivoRepository _archivoRepository;

        public ArchivoService(IArchivoRepository archivoRepository)
        {
            _archivoRepository = archivoRepository;
        }

        public async Task<string> SubirArchivoTempAsync(ArchivoUploadDTO archivoDto, string webRootPath)
        {
            return await _archivoRepository.GuardarArchivoTempAsync(archivoDto, webRootPath);
        }
    }

}
