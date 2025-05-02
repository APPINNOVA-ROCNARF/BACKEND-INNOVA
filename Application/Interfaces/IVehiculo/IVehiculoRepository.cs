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
    }
}
