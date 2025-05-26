using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Sistema
{
    public class ParrillaPromocional : IModificado
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaModificado { get; set; }
        public string? NombreArchivo { get; set; }
        public string? ExtensionArchivo { get; set; }
        public string? UrlArchivo { get; set; }

    }
}
