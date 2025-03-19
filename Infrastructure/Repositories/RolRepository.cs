using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.RolDTO;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly AppDbContext _context;

        public RolRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RolSimpleDTO>> GetRolesAsync()
        {
            return await _context.Roles
                .Select(r => new RolSimpleDTO
                {
                    RolId = r.Id,
                    NombreRol = r.Nombre,
                    Descripcion = r.Descripcion,
                    Estado = r.Estado
                })
                .ToListAsync();
        }

        public async Task<RolDTO> GetRolConModulosAsync(int rolId)
        {
            var rol = await _context.Roles
            .Where(r => r.Id == rolId)
                .Select(r => new RolDTO
                {
                    RolId = r.Id,
                    NombreRol = r.Nombre,
                    Descripcion = r.Descripcion,
                    Estado = r.Estado,
                    Modulos = _context.Modulos
                        .Select(m => new ModuloRolDTO
                        {
                            ModuloId = m.Id,
                            NombreModulo = m.Nombre,
                            Seleccionado = _context.RolModulos.Any(rm => rm.RolId == rolId && rm.ModuloId == m.Id),
                            Permisos = _context.Permisos
                                .Where(p => p.ModuloId == m.Id)
                                .Select(p => new PermisoRolDTO
                                {
                                    PermisoId = p.Id,
                                    NombrePermiso = p.Nombre,
                                    Seleccionado = _context.RolPermisos.Any(rp => rp.RolId == rolId && rp.PermisoId == p.Id),
                                    Acciones = _context.Acciones
                                        .Select(a => new AccionRolDTO
                                        {
                                            AccionId = a.Id,
                                            NombreAccion = a.Nombre,
                                            Seleccionado = _context.RolPermisos.Any(rp => rp.RolId == rolId && rp.PermisoId == p.Id && rp.AccionId == a.Id)
                                        }).ToList()
                                }).ToList()
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            return rol;
        }
    }
}

