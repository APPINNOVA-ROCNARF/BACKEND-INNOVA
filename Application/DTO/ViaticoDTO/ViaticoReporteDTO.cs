using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ViaticoDTO
{
    public class ViaticoReporteDTO
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;

        public decimal MovilizacionAcreditado { get; set; }
        public decimal MovilizacionAprobado { get; set; }
        public decimal MovilizacionRechazado { get; set; }
        public decimal MovilizacionDiferencia => MovilizacionRechazado + (MovilizacionAcreditado - MovilizacionAprobado);

        public decimal AlimentacionAcreditado { get; set; }
        public decimal AlimentacionAprobado { get; set; }
        public decimal AlimentacionRechazado { get; set; }
        public decimal AlimentacionDiferencia => AlimentacionRechazado + (AlimentacionAcreditado - AlimentacionAprobado);

        public decimal HospedajeAcreditado { get; set; }
        public decimal HospedajeAprobado { get; set; }
        public decimal HospedajeRechazado { get; set; }
        public decimal HospedajeDiferencia => HospedajeRechazado + (HospedajeAcreditado - HospedajeAprobado);
    }
}
