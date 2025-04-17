using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Viaticos;

namespace Application.Interfaces.IViatico
{
    public interface IProveedorViaticoRepository
    {
        Task<bool> ExistePorRucAsync(string ruc);
        Task CrearAsync(ProveedorViatico proveedor);
    }
}
