using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ArchivoDTO;
using Application.Interfaces.IArchivo;
using Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Repositories
{
    public class ArchivoRepository : IArchivoRepository
    {
        private readonly  _env;
        private readonly ArchivosOptions _opciones;

        public ArchivoRepository(IWebHost env, IOptions<ArchivosOptions> opciones)
        {
            _env = env;
            _opciones = opciones.Value;
        }
        public async Task<string> GuardarTemporalDesdeDtoAsync(ArchivoDTO archivoDto)
        {
            // Validación de extensión
            var extension = archivoDto.Extension.ToLower();
            if (!_opciones.ExtensionesPermitidas.Contains(extension))
                throw new InvalidOperationException("Extensión no permitida.");

            // Validación de tamaño (en bytes)
            var maxBytes = _opciones.MaximoMB * 1024 * 1024;
            if (archivoDto.Contenido.Length > maxBytes)
                throw new InvalidOperationException($"Archivo excede el tamaño máximo de {_opciones.MaximoMB} MB.");

            // Carpeta por fecha
            var fecha = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var nombreArchivo = Guid.NewGuid() + extension;

            var rutaCarpeta = Path.Combine(_env., _opciones.RutaBase, "temp", fecha);
            Directory.CreateDirectory(rutaCarpeta);

            var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
            await File.WriteAllBytesAsync(rutaCompleta, archivoDto.Contenido);

            var rutaRelativa = Path.Combine(_opciones.RutaBase, "temp", fecha, nombreArchivo).Replace("\\", "/");
            return rutaRelativa;
        }
    }
}
