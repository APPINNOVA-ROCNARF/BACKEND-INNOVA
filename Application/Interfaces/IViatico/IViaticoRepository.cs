using Application.DTO.ViaticoDTO;
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
        Task<int> CrearViaticoAsync(CrearViaticoDTO dto);
    }
}
