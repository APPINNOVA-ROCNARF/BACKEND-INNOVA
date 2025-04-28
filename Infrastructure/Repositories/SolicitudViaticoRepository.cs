using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ViaticoDTO;
using Application.Interfaces.IViatico;
using Domain.Entities.Viaticos;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SolicitudViaticoRepository : ISolicitudViaticoRepository
    {
        private readonly ViaticosDbContext _context;

        public SolicitudViaticoRepository(ViaticosDbContext context)
        {
            _context = context;
        }

        public async Task<SolicitudViatico?> ObtenerPorCicloUsuarioAsync(int cicloId, int usuarioAppId)
        {
            return await _context.SolicitudesViatico
                .FirstOrDefaultAsync(s => s.CicloId == cicloId && s.UsuarioAppId == usuarioAppId);
        }

        public async Task CrearAsync(SolicitudViatico solicitud)
        {
            _context.SolicitudesViatico.Add(solicitud);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarMontoAsync(SolicitudViatico solicitud)
        {
            _context.SolicitudesViatico.Update(solicitud);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SolicitudViatico>> ObtenerSolicitudPorCicloAsync(int cicloId)
        {
            return await _context.SolicitudesViatico
                .Where(s => s.CicloId == cicloId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SolicitudViatico?> ObtenerDetalleSolicitud(int solicitudId)
        {
            return await _context.SolicitudesViatico
                .FirstOrDefaultAsync(s => s.Id == solicitudId);
        }

        public async Task<SolicitudViatico> ObtenerViaticosPorIdAsync(int solicitudId)
        {
            return await _context.SolicitudesViatico
                .Include(s => s.Viaticos) 
                .FirstOrDefaultAsync(s => s.Id == solicitudId);
        }

        public async Task ActualizarEstadoAsync(SolicitudViatico solicitud)
        {
            _context.SolicitudesViatico.Update(solicitud);
            await _context.SaveChangesAsync();
        }

    }
}
