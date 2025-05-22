using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.VehiculoDTO
{
    public class CrearSolicitudVehiculoPrincipalDTO
    {
        public int UsuarioAppId { get; set; }
        public int VehiculoIdSolicitado { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
