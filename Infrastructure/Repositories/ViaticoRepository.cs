using Application.DTO.ViaticoDTO;
using Application.Helpers;
using Application.Interfaces.IViatico;
using Domain.Entities.Viaticos;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ViaticoRepository : IViaticoRepository
    {
        private readonly ViaticosDbContext _context;

        public ViaticoRepository(ViaticosDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearViaticoAsync(CrearViaticoDTO dto)
        {
            // Validar que no exista una factura con el mismo número y RUC
            bool facturaDuplicada = await _context.FacturasViatico.AnyAsync(f =>
                f.NumeroFactura == dto.Factura.NumeroFactura &&
                f.RucProveedor == dto.Factura.RucProveedor);

            if (facturaDuplicada)
            {
                throw new InvalidOperationException("Ya existe una factura con ese número y RUC.");
            }

            dto.Viatico.Factura = dto.Factura;

            dto.Viatico.Monto = dto.Monto;

            _context.Viaticos.Add(dto.Viatico);
            await _context.SaveChangesAsync();

            return dto.Viatico.Id;
        }

        public async Task<EstadisticaSolicitudViaticoDTO?> ObtenerEstadisticaSolicitudViaticoAsync(int cicloId)
        {
            var sql = "SELECT * FROM fn_dashboard_solicitudes_viatico({0})";
            return await _context.Set<EstadisticaSolicitudViaticoDTO>()
                .FromSqlRaw(sql, cicloId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ViaticoListDTO>> ObtenerViaticosPorSolicitudAsync(int solicitudId)
        {
            return await _context.Viaticos
                .Include(v => v.Factura)
                    .ThenInclude(f => f.Proveedor)
                .Include(v => v.Categoria)
                .Where(v => v.SolicitudViaticoId == solicitudId)
                .Select(v => new ViaticoListDTO
                {
                    Id = v.Id,
                    FechaFactura = v.Factura != null ? v.Factura.FechaFactura : DateTime.MinValue,
                    NombreCategoria = v.Categoria.Nombre,
                    NombreProveedor = v.Factura != null ? v.Factura.Proveedor.RazonSocial : string.Empty,
                    NumeroFactura = v.Factura != null ? v.Factura.NumeroFactura : string.Empty,
                    Comentario = v.Comentario,
                    Monto = v.Factura != null ? v.Factura.Total : 0,
                    EstadoViatico = v.EstadoViatico.ToFriendlyString(),
                    RutaImagen = v.Factura != null ? v.Factura.RutaImagen : string.Empty,
                    CamposRechazados = v.CamposRechazados
                })
                .ToListAsync();
        }
    }
}
