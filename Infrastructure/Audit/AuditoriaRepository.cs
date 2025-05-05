using Application.Audit;
using Domain.Entities.Auditoria;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Audit
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly AuditoriaDbContext _context;

        public AuditoriaRepository(AuditoriaDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(AuditoriaRegistro registro)
        {
            _context.Auditorias.Add(registro);
            await _context.SaveChangesAsync();
        }
    }
}
