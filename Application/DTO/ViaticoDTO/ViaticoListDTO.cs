using Application.DTO.VehiculoDTO;
using Domain.Entities.Viaticos;

namespace Application.DTO.ViaticoDTO
{
    public class ViaticoListDTO
    {
        public int Id { get; set; }
        public DateTime FechaFactura { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string NombreProveedor { get; set; } = string.Empty;
        public string NumeroFactura { get; set; } = string.Empty;
        public string? Comentario { get; set; }
        public decimal Monto { get; set; }
        public string EstadoViatico { get; set; } = string.Empty;
        public string RutaImagen { get; set; } = string.Empty;
        public int FacturaId { get; set; }
        public List<CampoRechazado>? CamposRechazados { get; set; }
        public VehiculoViaticoDTO? Vehiculo { get; set; }
    }
}
