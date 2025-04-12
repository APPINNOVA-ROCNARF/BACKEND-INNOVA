using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.SistemaDTO;
using Application.Interfaces.ISistema;
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
                .Where(c => c.Estado)
                .Select(c => new CicloSelectDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                })
                .ToListAsync();
        }
    }
}
