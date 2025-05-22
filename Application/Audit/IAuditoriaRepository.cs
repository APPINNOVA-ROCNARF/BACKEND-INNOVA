using Domain.Entities.Auditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Audit
{
    public interface IAuditoriaRepository
    {
        Task AgregarAsync(AuditoriaBase auditoria, string modulo);
        Task<IList<HistorialAuditoriaDTO>> ObtenerHistorialAsync(string entidad, int entidadId);
    }
}
