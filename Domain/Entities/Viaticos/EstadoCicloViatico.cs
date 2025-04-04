using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Viaticos
{
    public enum EstadoCicloViatico
    {
        NoEnviado = 0,
        Enviado = 1,
        Revisado = 2,
        Finalizado = 3
    }
}
