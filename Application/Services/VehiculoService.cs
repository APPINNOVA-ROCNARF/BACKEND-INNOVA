using Application.DTO.VehiculoDTO;
using Application.Exceptions;
using Application.Interfaces.IUsuario;
using Application.Interfaces.IVehiculo;
using Domain.Entities.Viaticos;

namespace Application.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IUsuarioService _usuarioService;

        public VehiculoService(
            IVehiculoRepository vehiculoRepository,
            IUsuarioService usuarioService)
        {
            _vehiculoRepository = vehiculoRepository;
            _usuarioService = usuarioService;
        }

        public async Task<int> RegistrarVehiculoAsync(RegistrarVehiculoDTO dto)
        {
            // Obtener el ID del usuario dueño del vehículo (por ahora desde el nombre de usuario)
            int usuarioAppId = await _usuarioService.ObtenerIdPorNombreUsuario(dto.NombreUsuario);


            var vehiculo = new Vehiculo
            {
                UsuarioAppId = usuarioAppId,
                Placa = dto.Placa,
                Fabricante = dto.Fabricante,
                Modelo = dto.Modelo,
                Color = dto.Color
            };

            return await _vehiculoRepository.RegistrarVehiculoAsync(vehiculo);
        }

        public async Task CrearSolicitudCambioVehiculoAsync(CrearSolicitudVehiculoPrincipalDTO dto)
        {
            if (!await _vehiculoRepository.ExisteVehiculoAsync(dto.VehiculoIdSolicitado))
                throw new BusinessException("El vehículo solicitado no existe.");

            if (!await _vehiculoRepository.VehiculoPerteneceAlUsuarioAsync(dto.VehiculoIdSolicitado, dto.UsuarioAppId))
                throw new BusinessException("El vehículo solicitado no pertenece al usuario.");

            if (await _vehiculoRepository.ExisteSolicitudPendienteAsync(dto.UsuarioAppId))
                throw new BusinessException("Ya existe una solicitud pendiente para este usuario.");

            if (await _vehiculoRepository.VehiculoYaEsPrincipalAsync(dto.VehiculoIdSolicitado))
                throw new BusinessException("El vehículo solicitado ya está asignado como principal a otro usuario.");


            var solicitud = new SolicitudVehiculoPrincipal
            {
                UsuarioAppId = dto.UsuarioAppId,
                VehiculoIdSolicitado = dto.VehiculoIdSolicitado,
                Motivo = dto.Motivo,
                Estado = EstadoSolicitudVehiculo.Pendiente,
                FechaRegistro = DateTime.Now
            };

            await _vehiculoRepository.AgregarSolicitudCambioVehiculoAsync(solicitud);
        }
    }
}
