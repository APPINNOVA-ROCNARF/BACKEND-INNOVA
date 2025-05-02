using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public interface ICreado
    {
        DateTime FechaRegistro { get; set; }
    }

    public interface IModificado
    {
        DateTime FechaModificado { get; set; }
    }
}
