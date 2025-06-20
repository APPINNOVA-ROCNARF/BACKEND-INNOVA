using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.VehiculoDTO;
using Domain.Entities.Viaticos;

namespace Application.DTO.ViaticoDTO.mobile
{
    public class AppViaticoListDTO
    {
        public int Id { get; set; }
        public DateTime FechaFactura { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string NombreSubcategoria { get; set; } = string.Empty;
        public string NombreProveedor { get; set; } = string.Empty;
        public string? Comentario { get; set; }
        public decimal Monto { get; set; }
        public string EstadoViatico { get; set; } = string.Empty;
    }
}
