﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ArchivoDTO;

namespace Application.Interfaces.IArchivo
{
    public interface IArchivoRepository
    {
        Task<string> GuardarArchivoTempAsync(ArchivoUploadDTO archivoDto, string webRootPath);
        Task<string> MoverArchivoFinalAsync(MoverArchivoDTO dto, string webRootPath);

    }
}
