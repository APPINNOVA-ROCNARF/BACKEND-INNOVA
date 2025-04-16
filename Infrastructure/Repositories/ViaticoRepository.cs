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

        public async Task<int> CrearViaticoConFacturaAsync(
            Viatico viatico,
            FacturaViatico factura,
            int usuarioAppId,
            int cicloId,
            decimal monto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Insertar factura
                _context.FacturasViatico.Add(factura);
                await _context.SaveChangesAsync();

                // Verificar/crear solicitud
                var solicitud = await _context.SolicitudesViatico
                    .FirstOrDefaultAsync(s => s.UsuarioAppId == usuarioAppId && s.CicloId == cicloId);

                if (solicitud == null)
                {
                    solicitud = new SolicitudViatico
                    {
                        UsuarioAppId = usuarioAppId,
                        CicloId = cicloId,
                        Estado = EstadoSolicitud.NoEnviada,
                        Monto = 0,
                        FechaRegistro = DateTime.UtcNow,
                        FechaModificado = DateTime.UtcNow
                    };
                    _context.SolicitudesViatico.Add(solicitud);
                    await _context.SaveChangesAsync();
                }

                // Crear viático con la relación a solicitud y factura
                viatico.FacturaId = factura.Id;
                viatico.SolicitudViaticoId = solicitud.Id;
                _context.Viaticos.Add(viatico);

                // Sumar monto
                solicitud.Monto += monto;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return viatico.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
