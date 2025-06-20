using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.VehiculoDTO;

namespace Application.Interfaces.IVehiculo
{
    public interface IVehiculoService
    {
        Task<int> RegistrarVehiculoAsync(RegistrarVehiculoDTO dto);
    }
}
