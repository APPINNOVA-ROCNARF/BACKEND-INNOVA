using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Viaticos;

namespace Application.Interfaces.IPresupuestoViatico
{
    public interface ICupoMensualRepository
    {
        Task DeleteByCicloAsync(int cicloId, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<CupoMensual> cupos, CancellationToken cancellationToken = default);
        Task<bool> ExisteParaCicloAsync(int cicloId);

    }
}
