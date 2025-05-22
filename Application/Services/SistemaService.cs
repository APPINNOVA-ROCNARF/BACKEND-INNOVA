using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ArchivoDTO;
using Application.DTO.GuiaProductoDTO;
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
        public async Task<string> ObtenerNombreCicloAsync(int cicloId)
        {
            return await _repository.ObtenerNombreCicloAsync(cicloId);
        }

        public async Task<int?> ObtenerIdPorCodigoSeccionAsync(string codigo)
        {
            return await _repository.ObtenerIdPorCodigoSeccionAsync(codigo);
        }

        public async Task<int> CrearGuiaProductoAsync(CrearGuiaProductoDTO dto, string webRootPath)
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
                    moverDtos, guiaId, webRootPath);

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
                // Si ya se habían movido archivos, los eliminamos
                foreach (var ruta in rutasFinales)
                {
                    var pathCompleto = Path.Combine(webRootPath, ruta);
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
    }
}
