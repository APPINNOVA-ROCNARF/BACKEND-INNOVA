using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.MenuDTO;

namespace Application.DTO.RolDTO
{
    public class RolDTO
    {
        public int RolId { get; set; }
        public string NombreRol { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public List<ModuloRolDTO> Modulos { get; set; } = new List<ModuloRolDTO>();
    }
}
