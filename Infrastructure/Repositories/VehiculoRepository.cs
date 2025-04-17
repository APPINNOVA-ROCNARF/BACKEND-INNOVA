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
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly ViaticosDbContext _context;

        public VehiculoRepository(ViaticosDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistePorPlacaAsync(string placa)
        {
            return await _context.Vehiculos.AnyAsync(v => v.Placa == placa);
        }

        public async Task CrearAsync(Vehiculo vehiculo)
        {
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
        }
    }
}
