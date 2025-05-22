using Application.DTO.VehiculoDTO;
using Application.DTO.ViaticoDTO;
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
            var facturasValidadas = new List<FacturaViatico>();

            foreach (var factura in dto.Facturas)
            {
                var facturaExistente = await _context.FacturasViatico
                    .FirstOrDefaultAsync(f =>
                        f.NumeroFactura == factura.NumeroFactura &&
                        f.RucProveedor == factura.RucProveedor);

                if (facturaExistente != null)
                {
                    // También podrías validar si ya fue usada en otro viático
                    throw new InvalidOperationException($"Ya existe una factura con número {factura.NumeroFactura} y RUC {factura.RucProveedor}.");
                }

                facturasValidadas.Add(factura);
                _context.FacturasViatico.Add(factura);
            }

            var nuevoViatico = dto.Viatico;
            nuevoViatico.Monto = dto.Monto;

            _context.Viaticos.Add(nuevoViatico);
            await _context.SaveChangesAsync(); // Para obtener el ID del viático y las facturas

            foreach (var factura in facturasValidadas)
            {
                _context.RelacionViaticoFacturas.Add(new RelacionViaticoFactura
                {
                    ViaticoId = nuevoViatico.Id,
                    FacturaId = factura.Id
                });
            }

            await _context.SaveChangesAsync();

            return nuevoViatico.Id;
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
            var viaticos = await _context.Viaticos
                .Include(v => v.RelacionViaticoFacturas)
                    .ThenInclude(vf => vf.Factura)
                        .ThenInclude(f => f.Proveedor)
                .Include(v => v.Categoria)
                .Include(v => v.Subcategoria)
                .Include(v => v.Vehiculo)
                .Where(v => v.SolicitudViaticoId == solicitudId)
                .OrderByDescending(v => v.FechaModificado)
                .ToListAsync();

            return viaticos.Select(v => new ViaticoListDTO
            {
                Id = v.Id,
                NombreCategoria = v.Categoria.Nombre,
                NombreSubcategoria = v.Subcategoria?.Nombre,
                Comentario = v.Comentario,
                EstadoViatico = v.EstadoViatico.ToString(),
                CamposRechazados = v.CamposRechazados,
                Vehiculo = v.Categoria.Nombre == "Movilización" && v.Vehiculo != null
                    ? new VehiculoViaticoDTO
                    {
                        Placa = v.Vehiculo.Placa,
                        Modelo = v.Vehiculo.Modelo,
                        Color = v.Vehiculo.Color,
                        Fabricante = v.Vehiculo.Fabricante
                    }
                    : null,

                Facturas = v.RelacionViaticoFacturas?
                    .Select(vf => vf.Factura)
                    .Select(f => new FacturaDTO
                    {
                        Id = f.Id,
                        NumeroFactura = f.NumeroFactura,
                        FechaFactura = f.FechaFactura,
                        ProveedorNombre = f.Proveedor?.RazonSocial ?? "",
                        RucProveedor = f.RucProveedor,
                        Monto = f.Total,
                        RutaImagen = f.RutaImagen
                    })
                    .ToList() ?? new List<FacturaDTO>()
            }).ToList();
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

        public async Task<RelacionViaticoFactura?> ObtenerRelacionConFacturaYViaticoAsync(int facturaId)
        {
            return await _context.RelacionViaticoFacturas
                .Include(vf => vf.Factura)
                    .ThenInclude(f => f.Proveedor)
                .Include(vf => vf.Viatico)
                    .ThenInclude(v => v.SolicitudViatico)
                .FirstOrDefaultAsync(vf => vf.FacturaId == facturaId);
        }

        public void MarcarModificado<T>(T entidad) where T : class, IModificado
        {
            _context.Entry(entidad).Property(e => e.FechaModificado).IsModified = true;
        }

        public async Task<SubcategoriaViatico?> ObtenerPorIdAsync(int? id)
        {
            return await _context.SubcategoriaViatico
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<int>> ObtenerIdsFacturasPorViaticoAsync(int viaticoId)
        {
            return await _context.RelacionViaticoFacturas
                .Where(vf => vf.ViaticoId == viaticoId)
                .Select(vf => vf.FacturaId)
                .ToListAsync();
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
    }
}
