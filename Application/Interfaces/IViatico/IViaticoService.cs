﻿using Application.DTO.ViaticoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IViatico
{
    public interface IViaticoService
    {
        Task<int> CrearViaticoAsync(ViaticoCrearDTO dto, string webRootPath);
    }
}
