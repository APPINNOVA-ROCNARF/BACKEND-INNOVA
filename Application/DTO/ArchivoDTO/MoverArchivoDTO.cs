using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ArchivoDTO
{
    public class MoverArchivoDTO
    {
        public string RutaRelativaTemporal { get; set; } = string.Empty;
        public int UsuarioAppId { get; set; }
        public int CicloId { get; set; }
        public DateTime FechaFactura { get; set; }
        public string PrefijoNombre { get; set; } = "factura";
    }
}
