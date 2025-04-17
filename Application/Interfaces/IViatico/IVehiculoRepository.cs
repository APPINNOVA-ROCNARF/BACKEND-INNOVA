using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Viaticos;

namespace Application.Interfaces.IViatico
{
    public interface IVehiculoRepository
    {
        Task<bool> ExistePorPlacaAsync(string placa);
        Task CrearAsync(Vehiculo vehiculo);
    }
}
