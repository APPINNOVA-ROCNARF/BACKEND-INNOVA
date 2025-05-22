using Application.DTO.VehiculoDTO;
using Domain.Entities.Viaticos;

namespace Application.DTO.ViaticoDTO
{
    public class ViaticoListDTO
    {
        public int Id { get; set; }
        public string NombreCategoria { get; set; }
        public string NombreSubcategoria { get; set; }
        public string? Comentario { get; set; }
        public List<FacturaDTO> Facturas { get; set; }
        public string EstadoViatico { get; set; } = string.Empty;
        public List<CampoRechazado>? CamposRechazados { get; set; }
        public VehiculoViaticoDTO? Vehiculo { get; set; }
    }
}
