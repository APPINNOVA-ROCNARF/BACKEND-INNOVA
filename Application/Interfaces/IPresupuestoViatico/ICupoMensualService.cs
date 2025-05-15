using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.PresupuestoViaticoDTO;

namespace Application.Interfaces.IPresupuestoViatico
{
    public interface ICupoMensualService
    {
        Task<(int cuposInsertados, List<string> sectoresNoEncontrados)>
            CargarCuposDesdeSectoresAsync(List<CupoMensualDTO> sectores, int cicloId, CancellationToken cancellationToken);
        Task<bool> ExisteCargaParaCicloAsync(int cicloId);

    }
}
