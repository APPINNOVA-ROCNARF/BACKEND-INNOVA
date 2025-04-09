using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.RolDTO
{
    public class CrearRolRequestDTO
    {
        public string NombreRol { get; set; }
        public string Descripcion { get; set; }
        public List<ModuloRolDTO> Modulos { get; set; } = new();
    }
}
