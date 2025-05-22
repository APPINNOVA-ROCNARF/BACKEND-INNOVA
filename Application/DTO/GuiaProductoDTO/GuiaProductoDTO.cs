using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GuiaProductoDTO
{
    public class GuiaProductoDTO
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Fuerza { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
