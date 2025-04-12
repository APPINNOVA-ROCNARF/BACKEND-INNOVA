using Application.DTO.RolDTO;
using Application.Interfaces.IRol;
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
                    Tipo = r.Tipo.ToString(),
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

        public async Task<List<ModuloRolDTO>> GetModulosAsync()
        {
            var modulos = await _context.Modulos.ToListAsync();
            var permisos = await _context.Permisos.ToListAsync();
            var acciones = await _context.Acciones.ToListAsync();

            var modulosDto = modulos.Select(m => new ModuloRolDTO
            {
                ModuloId = m.Id,
                NombreModulo = m.Nombre,
                Seleccionado = false,
                Permisos = permisos
                    .Where(p => p.ModuloId == m.Id)
                    .Select(p => new PermisoRolDTO
                    {
                        PermisoId = p.Id,
                        NombrePermiso = p.Nombre,
                        Seleccionado = false,
                        Acciones = acciones.Select(a => new AccionRolDTO
                        {
                            AccionId = a.Id,
                            NombreAccion = a.Nombre,
                            Seleccionado = false
                        }).ToList()
                    }).ToList()
            }).ToList();

            return modulosDto;
        }

    

    public async Task CrearRolAsync(CrearRolRequestDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var nuevoRol = new Domain.Entities.Usuarios.Rol
                {
                    Nombre = dto.NombreRol,
                    Descripcion = dto.Descripcion,
                    Estado = true
                };

                _context.Roles.Add(nuevoRol);
                await _context.SaveChangesAsync();

                foreach (var moduloDto in dto.Modulos.Where(m => m.Seleccionado))
                {
                    var rolModulo = new Domain.Entities.Usuarios.RolModulos
                    {
                        RolId = nuevoRol.Id,
                        ModuloId = moduloDto.ModuloId
                    };
                    _context.RolModulos.Add(rolModulo);

                    foreach (var permisoDto in moduloDto.Permisos.Where(p => p.Seleccionado))
                    {
                        foreach (var accionDto in permisoDto.Acciones.Where(a => a.Seleccionado))
                        {
                            var rolPermiso = new Domain.Entities.Usuarios.RolPermisos
                            {
                                RolId = nuevoRol.Id,
                                PermisoId = permisoDto.PermisoId,
                                AccionId = accionDto.AccionId
                            };
                            _context.RolPermisos.Add(rolPermiso);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}

