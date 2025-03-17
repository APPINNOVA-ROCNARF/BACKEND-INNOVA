using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LogsAuditor
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string TipoEntidad { get; set; }
        public string Accion { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Detalles { get; set; }
        public string IP { get; set; }
    }
}
