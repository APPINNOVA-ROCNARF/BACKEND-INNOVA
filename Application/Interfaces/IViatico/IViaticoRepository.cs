using Domain.Entities.Viaticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IViatico
{
    public interface IViaticoRepository
    {
        Task<SolicitudViatico> CrearSolicitudViaticoAsync(int usuarioAppId, int cicloId);
        Task<int> CrearViaticoAsync(Viatico viatico);
    }
}
