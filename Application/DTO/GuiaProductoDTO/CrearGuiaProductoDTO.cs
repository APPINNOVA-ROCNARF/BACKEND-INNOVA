using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ArchivoDTO;

namespace Application.DTO.GuiaProductoDTO
{
    public class CrearGuiaProductoDTO
    {
        public string Marca { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string UrlVideo { get; set; } = string.Empty;
        public int FuerzaId { get; set; }
        public List<ArchivoTemporalGuardadoDTO> Archivos { get; set; } = new();
    }
}
