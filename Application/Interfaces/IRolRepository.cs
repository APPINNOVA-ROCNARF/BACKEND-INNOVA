using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.RolDTO;

namespace Application.Interfaces
{
    public interface IRolRepository
    {
        Task<List<RolSimpleDTO>> GetRolesAsync();
        Task<RolDTO> GetRolConModulosAsync(int rolId);
    }
}
