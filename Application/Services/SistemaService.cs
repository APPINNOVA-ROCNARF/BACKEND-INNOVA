using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ArchivoDTO;
using Application.DTO.GuiaProductoDTO;
using Application.DTO.ParrillaPromocionalDTO;
using Application.DTO.SistemaDTO;
using Application.Interfaces.IArchivo;
using Application.Interfaces.ISistema;
using Domain.Entities.Sistema;

namespace Application.Services
{
    public class SistemaService : ISistemaService
    {
        private readonly ISistemaRepository _repository;
        private readonly IArchivoRepository _archivoRepository;

        public SistemaService(ISistemaRepository repository, IArchivoRepository archivoRepository)
        {
            _repository = repository;
            _archivoRepository = archivoRepository;
        }

        public async Task<List<CicloSelectDTO>> ObtenerCiclosSelectAsync()
        {
            return await _repository.ObtenerCiclosSelectAsync();
        }

        public async Task<List<FuerzaSelectDTO>> ObtenerFuerzasSelectAsync()
        {
            return await _repository.ObtenerFuerzasSelectAsync();
        }

        public async Task<List<SeccionSelectDTO>> ObtenerSeccionesSelectAsync()
        {
            return await _repository.ObtenerSeccionSelectAsync();
        }
        public async Task<string> ObtenerNombreCicloAsync(int cicloId)
        {
            return await _repository.ObtenerNombreCicloAsync(cicloId);
        }

        public async Task<int?> ObtenerIdPorCodigoSeccionAsync(string codigo)
        {
            return await _repository.ObtenerIdPorCodigoSeccionAsync(codigo);
        }

        public async Task<int> CrearGuiaProductoAsync(CrearGuiaProductoDTO dto, string rutaBase)
        {
            var guia = new GuiaProducto
            {
                Marca = dto.Marca,
                Nombre = dto.Nombre,
                UrlVideo = dto.UrlVideo,
                FuerzaId = dto.FuerzaId,
                Activo = true,
            };

            var guiaId = 0;
            List<string> rutasFinales = new();

            try
            {
                // Intentamos crear la guía primero
                guiaId = await _repository.InsertarAsync(guia);

                // Preparamos los archivos a mover
                var moverDtos = dto.Archivos.Select(a => new MoverArchivoGuiaDTO
                {
                    RutaTemporal = a.RutaTemporal,
                    NombreOriginal = a.NombreOriginal
                }).ToList();

                // Movemos archivos
                rutasFinales = await _archivoRepository.MoverArchivosAGuiaProductoAsync(
                    moverDtos, guiaId, rutaBase);

                // Creamos los registros
                var archivos = dto.Archivos.Select((archivoTemp, index) => new ArchivoGuiaProducto
                {
                    GuiaProductoId = guiaId,
                    NombreOriginal = archivoTemp.NombreOriginal,
                    RutaRelativa = rutasFinales[index],
                    Extension = archivoTemp.Extension,
                    Activo = true,
                }).ToList();

                await _repository.InsertarArchivosAsync(archivos);

                return guiaId;
            }
            catch (Exception ex)
            {
                foreach (var ruta in rutasFinales)
                {
                    var pathCompleto = Path.Combine(rutaBase, ruta);
                    if (File.Exists(pathCompleto))
                        File.Delete(pathCompleto);
                }

                throw new Exception("Error al crear guía de producto. Se revirtieron los archivos.", ex);
            }
        }

        public Task<List<GuiaProductoDTO>> ObtenerGuiasProductoAsync()
        {
            return _repository.ObtenerGuiasProductoAsync();
        }

        public Task<GuiaProductoDetalleDTO?> ObtenerGuiaDetalleAsync(int id)
        {
            return _repository.ObtenerGuiaDetalleAsync(id);
        }

        public async Task EliminarGuiaAsync(int id, string rutaBase)
        {
            await _repository.EliminarGuiaAsync(id, rutaBase);
        }

        public async Task ActualizarGuiaProductoAsync(UpdateGuiaProductoDTO dto, string rutaBase)
        {
            var guia = await _repository.ObtenerGuiaPorIdAsync(dto.Id);
            if (guia == null)
                throw new Exception("Guía no encontrada");

            // Actualizar datos básicos
            guia.Marca = dto.Marca;
            guia.Nombre = dto.Nombre;
            guia.UrlVideo = dto.UrlVideo;
            guia.FuerzaId = dto.FuerzaId;

            // Actualizar entidad en BD
            await _repository.ActualizarGuiaAsync(guia);

            // Agregar nuevos archivos si aplica
            if (dto.Archivos.Any())
            {
                var moverDtos = dto.Archivos.Select(a => new MoverArchivoGuiaDTO
                {
                    RutaTemporal = a.RutaTemporal,
                    NombreOriginal = a.NombreOriginal
                }).ToList();

                var rutasFinales = await _archivoRepository.MoverArchivosAGuiaProductoAsync(
                    moverDtos, guia.Id, rutaBase);

                var nuevosArchivos = dto.Archivos.Select((archivoTemp, index) => new ArchivoGuiaProducto
                {
                    GuiaProductoId = guia.Id,
                    NombreOriginal = archivoTemp.NombreOriginal,
                    RutaRelativa = rutasFinales[index],
                    Extension = archivoTemp.Extension,
                    Activo = true,
                }).ToList();

                await _repository.InsertarArchivosAsync(nuevosArchivos);
            }
        }

        public async Task EliminarArchivoGuiaProductoAsync(int archivoId, string rutaBase)
        {
            await _repository.EliminarArchivoAsync(archivoId, rutaBase);
        }

        // PARRILLA PROMOCIONAL

        public async Task<int> GuardarParrillaPromocionalAsync(CrearParrillaPromocionalDTO dto, string rutaBase)
        {
            return await _repository.GuardarParrillaPromocionalAsync(dto, rutaBase);
        }

        public async Task<ParrillaPromocionalDTO?> ObtenerAsync()
        {
            var entidad = await _repository.ObtenerParrillaAsync();

            if (entidad == null)
                return null;

            return new ParrillaPromocionalDTO
            {
                Id = entidad.Id,
                Nombre = entidad.Nombre,
                Descripcion = entidad.Descripcion,
                NombreArchivo = entidad.NombreArchivo,
                ExtensionArchivo = entidad.ExtensionArchivo,
                UrlArchivo = entidad.UrlArchivo,
                FechaModificado = entidad.FechaModificado
            };
        }

        public async Task EliminarArchivoParrillaAsync(string rutaBase)
        {
            await _repository.EliminarArchivoParrillaAsync(rutaBase);
        }

    }
}
