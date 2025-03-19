using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.RolDTO
{
    public class PermisoRolDTO
    {
        public int PermisoId { get; set; }
        public string NombrePermiso { get; set; }
        public bool Seleccionado { get; set; }
        public List<AccionRolDTO> Acciones { get; set; } = new List<AccionRolDTO>();
    }
}
