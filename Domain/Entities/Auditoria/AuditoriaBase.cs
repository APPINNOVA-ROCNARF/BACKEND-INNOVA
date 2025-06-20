using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Auditoria
{
    public abstract class AuditoriaBase
    {
        public int Id { get; set; }

        public string TipoEvento { get; set; } = null!;

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }

        public string EntidadAfectada { get; set; }

        public int EntidadId { get; set; }

        public string Datos { get; set; } = null!;

        public string? Hash { get; set; }
        public string? IpCliente { get; set; }
        public string? MetodoHttp { get; set; }
        public string? RutaAccedida { get; set; }
    }
}
