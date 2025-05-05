using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Enums.Viatico;

namespace Application.DTO.ViaticoDTO
{
    public class ActualizarEstadoViaticoRequest
    {
        public AccionViatico Accion { get; set; }
        public List<ActualizarViaticoItem> Viaticos { get; set; } = new();
        public int UsuarioId { get; set; }
    }

    public class ActualizarViaticoItem
    {
        public int Id { get; set; }
        public string? Comentario { get; set; }
        public List<CampoRechazadoDTO>? CamposRechazados { get; set; }
    }

    public class CampoRechazadoDTO
    {
        public string Campo { get; set; } = string.Empty;
        public string Comentario { get; set; } = string.Empty;
    }
}
