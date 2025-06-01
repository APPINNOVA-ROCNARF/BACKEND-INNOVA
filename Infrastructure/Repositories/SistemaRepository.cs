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
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SistemaRepository : ISistemaRepository
    {
        private readonly SistemaDbContext _context;
        private readonly IArchivoRepository _archivoRepository;

        public SistemaRepository(SistemaDbContext context, IArchivoRepository archivoRepository)
        {
            _context = context;
            _archivoRepository=archivoRepository;
        }

        public async Task<List<CicloSelectDTO>> ObtenerCiclosSelectAsync()
        {
            return await _context.Ciclos
                .Select(c => new CicloSelectDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Estado = c.Estado,
                    FechaInicio = c.FechaInicio,
                    FechaFin = c.FechaFin ?? DateTime.Today
                })
                .ToListAsync();
        }

        public async Task<List<FuerzaSelectDTO>> ObtenerFuerzasSelectAsync()
        {
            return await _context.Fuerzas
                .Select(c => new FuerzaSelectDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                })
                .ToListAsync();
        }
        public async Task<string> ObtenerNombreCicloAsync(int cicloId)
        {
            var ciclo = await _context.Ciclos
                .FirstOrDefaultAsync(c => c.Id == cicloId);

            return ciclo?.Nombre ?? "Desconocido";
        }

        public async Task<int?> ObtenerIdPorCodigoSeccionAsync(string codigo)
        {
            return await _context.Secciones
                .Where(s => s.Codigo == codigo)
                .Select(s => (int?)s.Id)
                .FirstOrDefaultAsync();
        }

        //GUIA PRODUCTOS
        public async Task<int> InsertarAsync(GuiaProducto guia)
        {
            _context.GuiasProducto.Add(guia);
            await _context.SaveChangesAsync();
            return guia.Id;
        }

        public async Task InsertarArchivosAsync(IEnumerable<ArchivoGuiaProducto> archivos)
        {
            _context.ArchivoGuiaProducto.AddRange(archivos);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GuiaProductoDTO>> ObtenerGuiasProductoAsync()
        {
            return await _context.GuiasProducto
                .Where(g => g.Activo)
                .Include(g => g.Fuerza)
                .Select(g => new GuiaProductoDTO
                {
                    Id = g.Id,
                    Marca = g.Marca,
                    Nombre = g.Nombre,
                    Fuerza = g.Fuerza.Nombre,
                    Activo = g.Activo
                })
                .ToListAsync();
        }

        public async Task<GuiaProductoDetalleDTO?> ObtenerGuiaDetalleAsync(int id)
        {
            var guia = await _context.GuiasProducto
                .Include(g => g.Fuerza)
                .Include(g => g.Archivos)
                .FirstOrDefaultAsync(g => g.Id == id && g.Activo);

            if (guia == null) return null;

            return new GuiaProductoDetalleDTO
            {
                Id = guia.Id,
                Marca = guia.Marca,
                Nombre = guia.Nombre,
                FuerzaNombre = guia.Fuerza?.Nombre ?? "",
                FuerzaId = guia.FuerzaId,
                UrlVideo = guia.UrlVideo,
                Archivos = guia.Archivos
                .Where(a => a.Activo)
                .Select(a => new ArchivoGuiaDTO
                {
                    Id = a.Id,
                    Nombre = a.NombreOriginal,
                    Ruta = a.RutaRelativa,
                    Extension = a.Extension,
                    Activo = a.Activo,
                    FechaRegistro = a.FechaRegistro
                })
                .ToList()
            };
        }

        public async Task EliminarGuiaAsync(int id, string rutaBase)
        {
            var guia = await _context.GuiasProducto
              .Include(g => g.Archivos)
              .FirstOrDefaultAsync(g => g.Id == id);

            if (guia == null)
                throw new KeyNotFoundException("Guía no encontrada.");

            // Eliminar archivos físicos
            foreach (var archivo in guia.Archivos)
            {
                var rutaCompleta = Path.Combine(rutaBase, archivo.RutaRelativa);
                if (File.Exists(rutaCompleta))
                {
                    File.Delete(rutaCompleta);
                }
            }

            _context.GuiasProducto.Remove(guia);
            await _context.SaveChangesAsync();
        }

        public async Task<GuiaProducto?> ObtenerGuiaPorIdAsync(int id)
        {
            return await _context.GuiasProducto.FindAsync(id);
        }

        public async Task ActualizarGuiaAsync(GuiaProducto guia)
        {
            _context.GuiasProducto.Update(guia);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarArchivoAsync(int archivoId, string rutaBase)
        {
            var archivo = await _context.ArchivoGuiaProducto.FindAsync(archivoId);
            if (archivo == null)
                throw new KeyNotFoundException("Archivo no encontrado.");

            var rutaCompleta = Path.Combine(rutaBase, archivo.RutaRelativa);
            if (File.Exists(rutaCompleta))
            {
                File.Delete(rutaCompleta);
            }

            _context.ArchivoGuiaProducto.Remove(archivo);
            await _context.SaveChangesAsync();
        }

        // PARRILLA PROMOCIONAL

        public async Task<int> GuardarParrillaPromocionalAsync(CrearParrillaPromocionalDTO dto, string rutaBase)
        {
            string? rutaFinal = null;

            try
            {
                var existente = await _context.ParrillasPromocional.FirstOrDefaultAsync();
                var esNuevo = existente == null;

                var parrilla = existente ?? new ParrillaPromocional();

                parrilla.Nombre = dto.Nombre;
                parrilla.Descripcion = dto.Descripcion;
                parrilla.FechaModificado = DateTime.UtcNow;

                if (dto.Archivo != null)
                {
                    // Si ya hay archivo anterior, eliminarlo
                    if (!esNuevo && !string.IsNullOrEmpty(parrilla.UrlArchivo))
                    {
                        var pathAnterior = Path.Combine(rutaBase, parrilla.UrlArchivo);
                        if (File.Exists(pathAnterior))
                        {
                            File.Delete(pathAnterior);
                        }
                    }

                    // Mover nuevo archivo
                    var moverDto = new MoverArchivoGuiaDTO
                    {
                        RutaTemporal = dto.Archivo.RutaTemporal,
                        NombreOriginal = dto.Archivo.NombreOriginal
                    };

                    rutaFinal = await _archivoRepository.MoverArchivosParrillaPromocionalAsync(
                        moverDto, parrilla.Id == 0 ? 1 : parrilla.Id, rutaBase);

                    parrilla.NombreArchivo = dto.Archivo.NombreOriginal;
                    parrilla.ExtensionArchivo = dto.Archivo.Extension;
                    parrilla.UrlArchivo = rutaFinal;
                }

                if (esNuevo)
                    _context.ParrillasPromocional.Add(parrilla);
                else
                    _context.ParrillasPromocional.Update(parrilla);

                await _context.SaveChangesAsync();
                return parrilla.Id;
            }
            catch (Exception ex)
            {
                // Si algo falla y ya se movió el archivo, lo eliminamos
                if (!string.IsNullOrEmpty(rutaFinal))
                {
                    var pathCompleto = Path.Combine(rutaBase, rutaFinal);
                    if (File.Exists(pathCompleto))
                        File.Delete(pathCompleto);
                }

                throw new Exception("Error al guardar la parrilla promocional.", ex);
            }
        }

        public async Task<ParrillaPromocional?> ObtenerParrillaAsync()
        {
            return await _context.ParrillasPromocional.FirstOrDefaultAsync();
        }

        public async Task EliminarArchivoParrillaAsync(string rutaBase)
        {
            var parrilla = await _context.ParrillasPromocional.FirstOrDefaultAsync();

            if (parrilla == null)
                throw new InvalidOperationException("No existe una parrilla registrada.");

            if (string.IsNullOrWhiteSpace(parrilla.UrlArchivo))
                throw new FileNotFoundException("No hay archivo registrado para eliminar.");

            var rutaCompleta = Path.Combine(rutaBase, parrilla.UrlArchivo);

            if (!File.Exists(rutaCompleta))
                throw new FileNotFoundException("El archivo no existe en el sistema de archivos.");

            File.Delete(rutaCompleta);

            parrilla.NombreArchivo = null;
            parrilla.ExtensionArchivo = null;
            parrilla.UrlArchivo = null;

            _context.ParrillasPromocional.Update(parrilla);
            await _context.SaveChangesAsync();
        }
    }
}
