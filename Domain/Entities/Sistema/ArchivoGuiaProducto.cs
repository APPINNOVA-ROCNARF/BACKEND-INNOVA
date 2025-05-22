using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities.Sistema
{
    public class ArchivoGuiaProducto : ICreado
    {
        public int Id { get; set; }

        public int GuiaProductoId { get; set; }

        public GuiaProducto GuiaProducto { get; set; } = null!;

        public string NombreOriginal { get; set; } = string.Empty;

        public string RutaRelativa { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; }
    }
}
