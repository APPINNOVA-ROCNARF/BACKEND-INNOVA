using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.IPresupuestoViatico;
using Domain.Entities.Viaticos;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CupoMensualRepository : ICupoMensualRepository
    {
        private readonly ViaticosDbContext _context;

        public CupoMensualRepository(ViaticosDbContext context)
        {
            _context = context;
        }

        public async Task DeleteByCicloAsync(int cicloId, CancellationToken cancellationToken)
        {
            var cupos = await _context.CupoMensual
                .Where(c => c.CicloId == cicloId)
                .ToListAsync(cancellationToken);

            _context.CupoMensual.RemoveRange(cupos);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<CupoMensual> cupos, CancellationToken cancellationToken = default)
        {
            await _context.CupoMensual.AddRangeAsync(cupos, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExisteParaCicloAsync(int cicloId)
        {
            return await _context.CupoMensual
                .AnyAsync(c => c.CicloId == cicloId);
        }
    }
}
