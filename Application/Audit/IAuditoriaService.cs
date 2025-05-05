using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Audit
{
    public interface IAuditoriaService
    {
        Task RegistrarEventoAsync(
            string tipoEvento,
            object datos,
            string? entidad = null,
            int? entidadId = null,
            int? usuarioId = null);
    }
}
