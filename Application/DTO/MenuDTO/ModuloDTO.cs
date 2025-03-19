using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.MenuDTO
{
    public class ModuloDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Icono { get; set; }
        public List<PermisoDTO> Permisos { get; set; } = new List<PermisoDTO>();
    }
}
