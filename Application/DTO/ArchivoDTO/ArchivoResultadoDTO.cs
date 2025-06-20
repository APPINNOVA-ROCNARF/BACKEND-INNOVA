using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ArchivoDTO
{
    public class ArchivoResultadoDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Mime { get; set; } = string.Empty;
        public byte[] Contenido { get; set; } = Array.Empty<byte>();
        public string Modo { get; set; } = "ver";
    }
}
