using Application.DTO.ArchivoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ParrillaPromocionalDTO
{
    public class CrearParrillaPromocionalDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public ArchivoTemporalGuardadoDTO? Archivo { get; set; }
    }
}
