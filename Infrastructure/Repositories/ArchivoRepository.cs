
using Application.DTO.ArchivoDTO;
using Application.Interfaces.IArchivo;
using Application.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class ArchivoRepository : IArchivoRepository
    {
        private readonly ArchivosOptions _opciones;

        public ArchivoRepository(IOptions<ArchivosOptions> opciones)
        {
            _opciones = opciones.Value;
        }

        public async Task<string> GuardarArchivoTempAsync(ArchivoDTO archivoDto, string webRootPath)
        {
            if (archivoDto == null)
                throw new ArgumentNullException(nameof(archivoDto), "El archivo enviado está vacío.");

            if (archivoDto.Contenido == null || archivoDto.Contenido.Length == 0)
                throw new InvalidOperationException("El contenido del archivo no puede estar vacío.");




            var extension = archivoDto.Extension.ToLower();
            if (!_opciones.ExtensionesPermitidas.Contains(extension))
                throw new InvalidOperationException("Extensión no permitida.");

            if (archivoDto.Contenido.Length > _opciones.MaximoMB * 1024 * 1024)
                throw new InvalidOperationException("Archivo excede el tamaño máximo permitido.");

            var fecha = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var nombreFinal = Guid.NewGuid() + extension;

            var rutaRelativa = Path.Combine(_opciones.RutaBase, "temp", fecha, nombreFinal).Replace("\\", "/");
            var rutaCompleta = Path.Combine(webRootPath, rutaRelativa);

            Directory.CreateDirectory(Path.GetDirectoryName(rutaCompleta)!);
            await File.WriteAllBytesAsync(rutaCompleta, archivoDto.Contenido);

            return rutaRelativa;
        }
    }

}
