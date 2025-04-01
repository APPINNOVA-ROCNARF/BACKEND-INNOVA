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
            var rolEntity = await _context.Roles.FirstOrDefaultAsync(r => r.Id == rolId);
            if (rolEntity == null)
                return null;

            // Traer relaciones del rol de una sola vez
            var rolModulos = await _context.RolModulos
                .Where(rm => rm.RolId == rolId)
                .Select(rm => rm.ModuloId)
                .ToListAsync();

            var rolPermisos = await _context.RolPermisos
                .Where(rp => rp.RolId == rolId)
                .ToListAsync();

            var modulos = await _context.Modulos.ToListAsync();
            var permisos = await _context.Permisos.ToListAsync();
            var acciones = await _context.Acciones.ToListAsync();

            var rolDto = new RolDTO
            {
                RolId = rolEntity.Id,
                NombreRol = rolEntity.Nombre,
                Descripcion = rolEntity.Descripcion,
                Estado = rolEntity.Estado,
                Modulos = modulos.Select(m => new ModuloRolDTO
                {
                    ModuloId = m.Id,
                    NombreModulo = m.Nombre,
                    Seleccionado = rolModulos.Contains(m.Id),
                    Permisos = permisos
                        .Where(p => p.ModuloId == m.Id)
                        .Select(p => new PermisoRolDTO
                        {
                            PermisoId = p.Id,
                            NombrePermiso = p.Nombre,
                            Seleccionado = rolPermisos.Any(rp => rp.PermisoId == p.Id),
                            Acciones = acciones.Select(a => new AccionRolDTO
                            {
                                AccionId = a.Id,
                                NombreAccion = a.Nombre,
                                Seleccionado = rolPermisos.Any(rp =>
                                    rp.PermisoId == p.Id &&
                                    rp.AccionId == a.Id)
                            }).ToList()
                        }).ToList()
                }).ToList()
            };

            return rolDto;
        }
    }
}

