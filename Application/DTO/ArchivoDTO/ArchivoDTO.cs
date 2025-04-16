using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ArchivoDTO
{
    public class ArchivoDTO
    {
        public string Extension { get; set; } = string.Empty;
        public byte[] Contenido { get; set; } = Array.Empty<byte>();
    }
}
