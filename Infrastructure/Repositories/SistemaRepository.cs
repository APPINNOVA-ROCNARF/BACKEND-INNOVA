using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.GuiaProductoDTO;
using Application.DTO.SistemaDTO;
using Application.Interfaces.ISistema;
using Domain.Entities.Sistema;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SistemaRepository : ISistemaRepository
    {
        private readonly SistemaDbContext _context;

        public SistemaRepository(SistemaDbContext context)
        {
            _context = context;
        }

        public async Task<List<CicloSelectDTO>> ObtenerCiclosSelectAsync()
        {
            return await _context.Ciclos
                .Select(c => new CicloSelectDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Estado = c.Estado
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
    }
}
