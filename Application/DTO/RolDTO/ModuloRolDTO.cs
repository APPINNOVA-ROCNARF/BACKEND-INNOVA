using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.MenuDTO;

namespace Application.DTO.RolDTO
{
    public class ModuloRolDTO
    {
        public int ModuloId { get; set; }
        public string NombreModulo { get; set; }
        public bool Seleccionado { get; set; }
        public List<PermisoRolDTO> Permisos { get; set; } = new List<PermisoRolDTO>();
    }
}
