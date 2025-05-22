using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities.Sistema
{
    public class GuiaProducto : ICreado
    {
        public int Id { get; set; }

        public string Marca { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string UrlVideo { get; set; } = string.Empty;

        public int FuerzaId { get; set; }

        public Fuerza Fuerza { get; set; } = null!;

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; }
        public ICollection<ArchivoGuiaProducto> Archivos { get; set; } = new List<ArchivoGuiaProducto>();
    }
}
