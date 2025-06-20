using Application.DTO.VehiculoDTO;
using Application.DTO.ViaticoDTO;
using Application.DTO.ViaticoDTO.mobile;
using Application.Helpers;
using Application.Interfaces.IViatico;
using Domain.Common;
using Domain.Entities.Viaticos;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<EstadisticaViaticoDTO>> ObtenerEstadisticaViaticoAsync(int solicitudId)
        {
            var sql = "SELECT * FROM obtener_resumen_viaticos_por_categoria({0})";
            return await _context.Set<EstadisticaViaticoDTO>()
                .FromSqlRaw(sql, solicitudId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ViaticoListDTO>> ObtenerViaticosPorSolicitudAsync(int solicitudId)
        {
            return await _context.Viaticos
                .Include(v => v.Factura)
                    .ThenInclude(f => f.Proveedor)
                .Include(v => v.Categoria)
                .Include(v => v.Vehiculo)
                .Where(v => v.SolicitudViaticoId == solicitudId)
                .OrderByDescending(v => v.FechaModificado)
                .Select(v => new ViaticoListDTO
                {
                    Id = v.Id,
                    FechaFactura = v.Factura != null ? v.Factura.FechaFactura : DateTime.MinValue,
                    NombreCategoria = v.Categoria.Nombre,
                    NombreProveedor = v.Factura != null ? v.Factura.Proveedor.RazonSocial : string.Empty,
                    NumeroFactura = v.Factura != null ? v.Factura.NumeroFactura : string.Empty,
                    Comentario = v.Comentario,
                    Monto = v.Factura != null ? v.Factura.Total : 0,
                    EstadoViatico = v.EstadoViatico.ToString(),
                    RutaImagen = v.Factura != null ? v.Factura.RutaImagen : string.Empty,
                    FacturaId = v.FacturaId,
                    CamposRechazados = v.CamposRechazados,
                    Vehiculo = v.Categoria.Nombre == "Movilización" && v.Vehiculo != null
                        ? new VehiculoViaticoDTO
                        {
                            Placa = v.Vehiculo.Placa,
                            Modelo = v.Vehiculo.Modelo,
                            Color = v.Vehiculo.Color,
                            Fabricante = v.Vehiculo.Fabricante
                        }
                : null
                })
                .ToListAsync();
        }

        public async Task<List<Viatico>> ObtenerViaticosPorIdsAsync(List<int> ids)
        {
            return await _context.Viaticos
                .Where(v => ids.Contains(v.Id))
                .ToListAsync();
        }

        public async Task ActualizarViaticosAsync(List<Viatico> viaticos)
        {
            _context.Viaticos.UpdateRange(viaticos);
            await _context.SaveChangesAsync();
        }

        public async Task<Viatico?> GetIdPorFacturaAsync(int id)
        {
            return await _context.Viaticos
                .Include(v => v.Factura)
                    .ThenInclude(f => f.Proveedor)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public void MarcarModificado<T>(T entidad) where T : class, IModificado
        {
            _context.Entry(entidad).Property(e => e.FechaModificado).IsModified = true;
        }

        public async Task<SubcategoriaViatico?> ObtenerSubcategoriaPorIdAsync(int? id)
        {
            return await _context.SubcategoriaViatico
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }



        public async Task<List<Viatico>> ObtenerConSolicitudYCategoriaPorFiltroAsync(int? cicloId, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = _context.Viaticos
                .Include(v => v.Categoria)
                .Include(v => v.SolicitudViatico)
                .AsQueryable();

            if (cicloId.HasValue)
            {
                query = query.Where(v => v.SolicitudViatico!.CicloId == cicloId);
            }
            else if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                query = query.Where(v => v.FechaRegistro >= fechaInicio && v.FechaRegistro <= fechaFin);
            }

            return await query.ToListAsync();
        }

        public async Task<List<CupoMensual>> ObtenerCuposMensualesAsync(List<int> usuarioIds, List<int> ciclosIds)
        {
            return await _context.CupoMensual
                .Where(c => usuarioIds.Contains(c.UsuarioId) && ciclosIds.Contains(c.CicloId))
                .ToListAsync();
        }

        // APP MOVIL

        public async Task<IEnumerable<AppViaticoListDTO>> ObtenerViaticosApp(int solicitudId)
        {
            return await _context.Viaticos
                .Include(v => v.Factura)
                    .ThenInclude(f => f.Proveedor)
                .Include(v => v.Categoria)
                .Where(v => v.SolicitudViaticoId == solicitudId)
                .OrderByDescending(v => v.FechaModificado)
                .Select(v => new AppViaticoListDTO
                {
                    Id = v.Id,
                    FechaFactura = v.Factura != null ? v.Factura.FechaFactura : DateTime.MinValue,
                    NombreCategoria = v.Categoria.Nombre,
                    NombreSubcategoria = v.Subcategoria.Nombre,
                    NombreProveedor = v.Factura != null ? v.Factura.Proveedor.RazonSocial : string.Empty,
                    Comentario = v.Comentario,
                    Monto = v.Factura != null ? v.Factura.Total : 0,
                    EstadoViatico = v.EstadoViatico.ToString()
                })
                .ToListAsync();
        }
    }


}
