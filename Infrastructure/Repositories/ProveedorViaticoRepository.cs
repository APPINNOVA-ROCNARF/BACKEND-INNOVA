using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.IViatico;
using Domain.Entities.Viaticos;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProveedorViaticoRepository : IProveedorViaticoRepository
    {
        private readonly ViaticosDbContext _context;

        public ProveedorViaticoRepository(ViaticosDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistePorRucAsync(string ruc)
        {
            return await _context.ProveedoresViatico.AnyAsync(p => p.Ruc == ruc);
        }

        public async Task CrearAsync(ProveedorViatico proveedor)
        {
            _context.ProveedoresViatico.Add(proveedor);
            await _context.SaveChangesAsync();
        }
    }
}
