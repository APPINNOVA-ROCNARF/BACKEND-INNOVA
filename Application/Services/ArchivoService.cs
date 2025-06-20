using Application.DTO.ArchivoDTO;
using Application.Exceptions;
using Application.Interfaces.IArchivo;
using Domain.Common;
using Domain.Events;
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
        private readonly IDomainEventDispatcher _eventDispatcher;


        public ArchivoService(IArchivoRepository archivoRepository, IDomainEventDispatcher eventDispatcher)
        {
            _archivoRepository = archivoRepository;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<string> SubirArchivoTempAsync(ArchivoUploadDTO archivoDto, string rutaBase)
        {
            return await _archivoRepository.GuardarArchivoTempAsync(archivoDto, rutaBase);
        }

        public ArchivoResultadoDTO ObtenerArchivo(string rutaRelativa, string rootPath, string entidad, int entidadId, string? modo = "ver", string? modulo = "general")
        {
            if (string.IsNullOrWhiteSpace(rutaRelativa))
                throw new BusinessException("Ruta no válida.");

            if (rutaRelativa.Contains(".."))
                throw new BusinessException("Ruta no permitida.");

            rutaRelativa = Uri.UnescapeDataString(rutaRelativa).TrimStart('/');
            var rutaCompleta = Path.Combine(rootPath, "Storage", rutaRelativa.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (!File.Exists(rutaCompleta))
                throw new FileNotFoundException("Archivo no encontrado.");

            var extension = Path.GetExtension(rutaCompleta).ToLowerInvariant();
            var mime = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };

            var contenido = File.ReadAllBytes(rutaCompleta);
            var nombreArchivo = Path.GetFileName(rutaRelativa);

            _eventDispatcher.Dispatch(new ArchivoAccedidoEvent(
                nombreArchivo,
                rutaRelativa,
                modo ?? "ver",
                modulo ?? "general",
                entidad,
                entidadId
            ));

            return new ArchivoResultadoDTO
            {
                Nombre = nombreArchivo,
                Mime = mime,
                Contenido = contenido,
                Modo = modo ?? "ver"
            };
        }
    }

}
