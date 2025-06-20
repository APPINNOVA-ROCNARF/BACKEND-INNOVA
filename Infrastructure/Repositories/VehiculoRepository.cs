 using Application.Exceptions;
using Application.Interfaces.IVehiculo;
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

        public async Task<int> RegistrarVehiculoAsync(Vehiculo vehiculo)
        {
            // Validar placa unica
            bool placaExiste = await _context.Vehiculos
                .AnyAsync(v => v.Placa == vehiculo.Placa);

            if (placaExiste)
                throw new BusinessException("La placa ya está registrada en el sistema.");

            vehiculo.FechaRegistro = DateTime.Now;
            _context.Vehiculos.Add(vehiculo);

            await _context.SaveChangesAsync();
            return vehiculo.Id;
        }
        public async Task<bool> ExisteVehiculoAsync(int vehiculoId)
        {
            return await _context.Vehiculos.AnyAsync(v => v.Id == vehiculoId);
        }

    }
}

