using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ArchivoDTO;

namespace Application.DTO.TablaBonificacionesDTO
{
    public class CrearTablaBonificacionesDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public ArchivoTemporalGuardadoDTO? Archivo { get; set; }
    }
}
