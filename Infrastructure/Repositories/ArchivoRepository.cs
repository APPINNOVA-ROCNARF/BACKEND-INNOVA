
using Application.DTO.ArchivoDTO;
using Application.Exceptions;
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

        public async Task<string> GuardarArchivoTempAsync(ArchivoUploadDTO archivoDto, string rutaBase)
        {
            if (archivoDto == null)
                throw new ArgumentNullException(nameof(archivoDto), "El archivo enviado está vacío.");

            if (archivoDto.Contenido == null || archivoDto.Contenido.Length == 0)
                throw new BusinessException("El contenido del archivo no puede estar vacío.");




            var extension = archivoDto.Extension.ToLower();
            if (!_opciones.ExtensionesPermitidas.Contains(extension))
                throw new BusinessException("Extensión no permitida.");

            if (archivoDto.Contenido.Length > _opciones.MaximoMB * 1024 * 1024)
                throw new BusinessException("Archivo excede el tamaño máximo permitido.");

            var fecha = DateTime.Now.ToString("yyyy-MM-dd");
            var nombreFinal = Guid.NewGuid() + extension;

            var rutaRelativa = Path.Combine(_opciones.RutaBase, "temp", fecha, nombreFinal).Replace("\\", "/");
            var rutaCompleta = Path.Combine(rutaBase, rutaRelativa);

            Directory.CreateDirectory(Path.GetDirectoryName(rutaCompleta)!);
            await File.WriteAllBytesAsync(rutaCompleta, archivoDto.Contenido);

            return rutaRelativa;
        }

        public async Task<List<ArchivoTemporalGuardadoDTO>> GuardarArchivosTempAsync(
            List<ArchivoUploadDTO> archivosDto, string rutaBase)
        {
            var archivosGuardados = new List<ArchivoTemporalGuardadoDTO>();

            foreach (var archivoDto in archivosDto)
            {
                var rutaRelativa = await GuardarArchivoTempAsync(archivoDto, rutaBase);

                archivosGuardados.Add(new ArchivoTemporalGuardadoDTO
                {
                    NombreOriginal = archivoDto.Nombre,
                    RutaTemporal = rutaRelativa,
                    Extension = archivoDto.Extension
                });
            }

            return archivosGuardados;
        }

        // MOVER ARCHIVOS VIATICOS A CARPETA FINAL
        public async Task<string> MoverArchivoFinalAsync(MoverArchivoDTO dto, string rutaBase)
        {
            var rutaTemporalCompleta = Path.Combine(rutaBase, dto.RutaRelativaTemporal);

            if (!File.Exists(rutaTemporalCompleta))
                throw new FileNotFoundException("No se encontró el archivo temporal para mover.", rutaTemporalCompleta);

            var extension = Path.GetExtension(dto.RutaRelativaTemporal);
            var fechaStr = dto.FechaFactura.ToString("yyyy-MM-dd");
            var nombreFinal = $"{dto.PrefijoNombre}_{fechaStr}_{Guid.NewGuid()}{extension}";

            var rutaRelativaFinal = Path.Combine(
                _opciones.RutaBase,
                "viaticos",
                $"usuario_{dto.UsuarioAppId}",
                $"ciclo_{dto.CicloId}",
                fechaStr,
                nombreFinal
            ).Replace("\\", "/");

            var rutaFinalCompleta = Path.Combine(rutaBase, rutaRelativaFinal);

            Directory.CreateDirectory(Path.GetDirectoryName(rutaFinalCompleta)!);
            File.Move(rutaTemporalCompleta, rutaFinalCompleta);

            return  rutaRelativaFinal;
        }

        // MOVER ARCHIVOS GUIAS DE PRODUCTO A CARPETA FINAL

        public async Task<List<string>> MoverArchivosAGuiaProductoAsync(List<MoverArchivoGuiaDTO> archivos, int guiaProductoId, string rutaBase)
        {
            var rutasFinales = new List<string>();

            foreach (var archivo in archivos)
            {
                var rutaTemporalCompleta = Path.Combine(rutaBase, archivo.RutaTemporal);

                if (!File.Exists(rutaTemporalCompleta))
                    throw new FileNotFoundException("Archivo no encontrado: " + archivo.RutaTemporal);

                var extension = Path.GetExtension(archivo.RutaTemporal);
                var nombreFinal = $"{archivo.NombreOriginal}_{Guid.NewGuid()}{extension}";

                var rutaRelativaFinal = Path.Combine(
                    _opciones.RutaBase,
                    "guias-producto",
                    guiaProductoId.ToString(),
                    nombreFinal
                ).Replace("\\", "/");

                var rutaFinalCompleta = Path.Combine(rutaBase, rutaRelativaFinal);

                Directory.CreateDirectory(Path.GetDirectoryName(rutaFinalCompleta)!);
                File.Move(rutaTemporalCompleta, rutaFinalCompleta);

                rutasFinales.Add(rutaRelativaFinal);
            }

            return rutasFinales;
        }

        // MOVER ARCHIVOS PARRILLA PROMOCIONAL A SU CARPETA FINAL
        public async Task<string> MoverArchivosParrillaPromocionalAsync(MoverArchivoGuiaDTO archivo, int parrillaPromocionalId, string rutaBase)
        {
            var rutaTemporalCompleta = Path.Combine(rutaBase, archivo.RutaTemporal);

            if (!File.Exists(rutaTemporalCompleta))
                throw new FileNotFoundException("Archivo no encontrado: " + archivo.RutaTemporal);

            var extension = Path.GetExtension(archivo.RutaTemporal);
            var nombreFinal = $"{Path.GetFileNameWithoutExtension(archivo.NombreOriginal)}_{Guid.NewGuid()}{extension}";

            var rutaRelativaFinal = Path.Combine(
                _opciones.RutaBase,
                "parrilla-promocional",
                parrillaPromocionalId.ToString(),
                nombreFinal
            ).Replace("\\", "/");

            var rutaFinalCompleta = Path.Combine(rutaBase, rutaRelativaFinal);

            Directory.CreateDirectory(Path.GetDirectoryName(rutaFinalCompleta)!);
            File.Move(rutaTemporalCompleta, rutaFinalCompleta);

            return rutaRelativaFinal;
        }


        // MOVER ARCHIVOS TABLA BONIFICACIONES A CARPETA FINAL
        public async Task<string> MoverArchivosTablaBonificacionesAsync(MoverArchivoGuiaDTO archivo, int tablaBonificacionesId, string rutaBase)
        {
            var rutaTemporalCompleta = Path.Combine(rutaBase, archivo.RutaTemporal);

            if (!File.Exists(rutaTemporalCompleta))
                throw new FileNotFoundException("Archivo no encontrado: " + archivo.RutaTemporal);

            var extension = Path.GetExtension(archivo.RutaTemporal);
            var nombreFinal = $"{Path.GetFileNameWithoutExtension(archivo.NombreOriginal)}_{Guid.NewGuid()}{extension}";

            var rutaRelativaFinal = Path.Combine(
                _opciones.RutaBase,
                "tabla-bonificaciones",
                tablaBonificacionesId.ToString(),
                nombreFinal
            ).Replace("\\", "/");

            var rutaFinalCompleta = Path.Combine(rutaBase, rutaRelativaFinal);

            Directory.CreateDirectory(Path.GetDirectoryName(rutaFinalCompleta)!);
            File.Move(rutaTemporalCompleta, rutaFinalCompleta);

            return rutaRelativaFinal;
        }

    }

}
