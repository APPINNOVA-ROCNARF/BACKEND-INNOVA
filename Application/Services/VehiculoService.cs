using Application.DTO.VehiculoDTO;
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
    }
}
