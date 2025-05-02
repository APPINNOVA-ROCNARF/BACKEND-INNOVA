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

            vehiculo.FechaRegistro = DateTime.UtcNow;
            _context.Vehiculos.Add(vehiculo);

            // Verificar si es el primer vehículo del usuario 
            int totalVehiculosExistentes = await _context.Vehiculos
                .CountAsync(v => v.UsuarioAppId == vehiculo.UsuarioAppId);

            if (totalVehiculosExistentes == 0)
            {
                // Se asignará como vehículo principal
                _context.VehiculoPrincipal.Add(new VehiculoPrincipal
                {
                    UsuarioAppId = vehiculo.UsuarioAppId,
                    Vehiculo = vehiculo, 
                });

            }

            await _context.SaveChangesAsync();
            return vehiculo.Id;
        }
    }
}

