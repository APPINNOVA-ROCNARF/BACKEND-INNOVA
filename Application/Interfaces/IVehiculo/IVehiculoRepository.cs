using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Viaticos;

namespace Application.Interfaces.IVehiculo
{
    public interface IVehiculoRepository
    {
        Task<int> RegistrarVehiculoAsync(Vehiculo vehiculo);
        Task<bool> ExisteSolicitudPendienteAsync(int usuarioAppId);
        Task<bool> VehiculoYaEsPrincipalAsync(int vehiculoId);
        Task AgregarSolicitudCambioVehiculoAsync(SolicitudVehiculoPrincipal solicitud);
        Task<bool> ExisteVehiculoAsync(int vehiculoId);
        Task<bool> VehiculoPerteneceAlUsuarioAsync(int vehiculoId, int usuarioAppId);
        Task<List<SolicitudVehiculoPrincipal>> ObtenerSolicitudesConVehiculoAsync();


    }
}
