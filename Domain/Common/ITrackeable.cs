using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public interface ITrackeable
    {
        DateTime FechaRegistro { get; set; }
        DateTime FechaModificado { get; set; }
    }
}
