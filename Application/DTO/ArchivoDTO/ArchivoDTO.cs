using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ArchivoDTO
{
    public class ArchivoDTO
    {
        public string Nombre { get; set; } = default!;
        public string Extension { get; set; } = default!;
        public byte[] Contenido { get; set; } = default!;
    }
}
