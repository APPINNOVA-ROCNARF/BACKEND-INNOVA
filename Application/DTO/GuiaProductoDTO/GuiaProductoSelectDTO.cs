using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GuiaProductoDTO
{
    public class SelectItemDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class GuiaProductoSelectsDTO
    {
        public IEnumerable<SelectItemDTO> Nombres { get; set; }
        public IEnumerable<SelectItemDTO> Marcas { get; set; }
    }
}
