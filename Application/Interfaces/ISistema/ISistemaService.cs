using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.SistemaDTO;

namespace Application.Interfaces.ISistema
{
    public interface ISistemaService
    {
        Task<List<CicloSelectDTO>> ObtenerCiclosSelectAsync();
    }
}
