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

        public async Task<bool> ExisteSolicitudPendienteAsync(int usuarioAppId)
        {
            return await _context.SolicitudVehiculoPrincipal
                .AnyAsync(s => s.UsuarioAppId == usuarioAppId && s.Estado == EstadoSolicitudVehiculo.Pendiente);
        }

        public async Task<bool> VehiculoYaEsPrincipalAsync(int vehiculoId)
        {
            return await _context.VehiculoPrincipal
                .AnyAsync(vp => vp.VehiculoId == vehiculoId);
        }

        public async Task<bool> ExisteVehiculoAsync(int vehiculoId)
        {
            return await _context.Vehiculos.AnyAsync(v => v.Id == vehiculoId);
        }

        public async Task AgregarSolicitudCambioVehiculoAsync(SolicitudVehiculoPrincipal solicitud)
        {
            _context.SolicitudVehiculoPrincipal.Add(solicitud);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VehiculoPerteneceAlUsuarioAsync(int vehiculoId, int usuarioAppId)
        {
            return await _context.Vehiculos
                .AnyAsync(v => v.Id == vehiculoId && v.UsuarioAppId == usuarioAppId);
        }

        public async Task<List<SolicitudVehiculoPrincipal>> ObtenerSolicitudesConVehiculoAsync()
        {
            return await _context.SolicitudVehiculoPrincipal
                .Include(s => s.Vehiculo)
                .ToListAsync();
        }
    }
}

