using Application.DTO.ViaticoDTO;
using Application.Helpers;
using Application.Interfaces.IArchivo;
using Application.Interfaces.IViatico;
using Domain.Entities.Viaticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ViaticoService : IViaticoService
    {
        private readonly IViaticoRepository _viaticoRepository;
        private readonly IArchivoService _archivoService;

        public ViaticoService(IViaticoRepository viaticoRepository, IArchivoService archivoService)
        {
            _viaticoRepository = viaticoRepository;
            _archivoService = archivoService;
        }

        public async Task<int> CrearViaticoAsync(ViaticoCrearDTO dto, string webRootPath)
        {
            if (dto.Factura == null || string.IsNullOrWhiteSpace(dto.Factura.RutaTemporal))
                throw new InvalidOperationException("La factura y su archivo son obligatorios.");

            var rutaOrigen = Path.Combine(webRootPath, dto.Factura.RutaTemporal);
            if (!File.Exists(rutaOrigen))
                throw new FileNotFoundException("No se encontró el archivo temporal.", rutaOrigen);

            var nombreArchivo = Path.GetFileName(dto.Factura.RutaTemporal);
            var rutaRelativaFinal = RutaArchivoHelper.GenerarRutaRelativaFacturaViatico(
                dto.UsuarioAppId, dto.CicloId, nombreArchivo);
            var rutaDestino = RutaArchivoHelper.GenerarRutaCompleta(webRootPath, rutaRelativaFinal);

            await _archivoService.MoverArchivoAsync(new ArchivoMoverDTO
            {
                RutaOrigen = rutaOrigen,
                RutaDestino = rutaDestino
            });

            var factura = new FacturaViatico
            {
                NumeroFactura = dto.Factura.NumeroFactura,
                FechaFactura = dto.Factura.FechaFactura,
                Subtotal = dto.Factura.Subtotal,
                SubtotalIva = dto.Factura.SubtotalIva,
                Total = dto.Factura.Total,
                RucProveedor = dto.Factura.RucProveedor,
                RutaImagen = rutaRelativaFinal
            };

            var solicitud = await _viaticoRepository.CrearSolicitudViaticoAsync(dto.UsuarioAppId, dto.CicloId);
            solicitud.Monto += dto.Factura.Total;

            var viatico = new Viatico
            {
                CategoriaId = dto.CategoriaId,
                Comentario = dto.Comentario,
                FechaRegistro = DateTime.UtcNow,
                FechaModificado = DateTime.UtcNow,
                EstadoViatico = EstadoViatico.Borrador,
                SolicitudViaticoId = solicitud.Id,
                Factura = factura,
                Monto = dto.Factura.Total,
                PlacaVehiculo = dto.PlacaVehiculo
            };

            return await _viaticoRepository.CrearViaticoAsync(viatico);
        }
    }
}
