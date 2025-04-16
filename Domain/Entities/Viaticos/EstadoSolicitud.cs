﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Viaticos
{
    public enum EstadoSolicitud
    {
        NoEnviada = 0,
        EnRevision = 1,
        Aprobado = 2,
        Rechazado = 3,
        ParaCorregir = 4
    }
}
