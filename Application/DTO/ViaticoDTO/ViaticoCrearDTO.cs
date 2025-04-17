using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ViaticoDTO
{
    public class ViaticoCrearDTO
    {
        public int UsuarioAppId { get; set; }     
        public int CicloId { get; set; }         
        public int CategoriaId { get; set; }
        public string? PlacaVehiculo { get; set; }

        public string? Comentario { get; set; }      
        public FacturaCrearDTO Factura { get; set; }  
    }
}
