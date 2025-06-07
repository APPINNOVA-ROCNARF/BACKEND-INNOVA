using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GuiaProductoDTO
{
    public class GuiaProductoDetalleDTO
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string UrlVideo { get; set; } = string.Empty;
        public string FuerzaNombre { get; set; } = string.Empty;
        public int FuerzaId { get; set; }
        public Boolean Activo { get; set; }
        public List<ArchivoGuiaDTO> Archivos { get; set; } = new();
    }
}
