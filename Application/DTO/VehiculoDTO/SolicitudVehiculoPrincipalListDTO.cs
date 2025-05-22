using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.VehiculoDTO
{
    public class SolicitudVehiculoPrincipalListDTO
    {
        public int Id { get; set; }
        public int UsuarioAppId { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }
}
