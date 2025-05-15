using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO.PresupuestoViaticoDTO
{
    public class CupoMensualDTO
    {
        public string Sector { get; set; } = string.Empty;

        [JsonPropertyName("CUPO MOVILIDAD")]
        public decimal? CupoMovilidad { get; set; }

        [JsonPropertyName("CUPO HOSPEDAJE")]
        public decimal? CupoHospedaje { get; set; }

        [JsonPropertyName("CUPO ALIMENTACION")]
        public decimal? CupoAlimentacion { get; set; }
    }
}
