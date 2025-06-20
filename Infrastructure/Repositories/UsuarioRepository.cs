﻿using Application.DTO.MenuDTO;
using Application.DTO.UsuarioDTO;
using Application.Exceptions;
using Application.Interfaces.IUsuario;
using Domain.Entities.Usuarios;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        // Métodos para User
        public async Task<List<Usuario>> GetAllUsersAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Rol) 
                .ToListAsync();
        }
        public async Task<Usuario> GetUserByIdAsync(int id) => await _context.Usuarios.FindAsync(id);
        public async Task<Usuario> CreateUserAsync(Usuario usuario) { _context.Usuarios.Add(usuario); await _context.SaveChangesAsync(); return usuario; }
        public async Task UpdateUserAsync(Usuario usuario) { _context.Usuarios.Update(usuario); await _context.SaveChangesAsync(); }
        public async Task DeleteUserAsync(int id) { var usuario = await _context.Usuarios.FindAsync(id); if (usuario != null) { _context.Usuarios.Remove(usuario); await _context.SaveChangesAsync(); } }

        // Métodos para WebUser
        public async Task<UsuarioWeb> GetWebUserByIdAsync(int id) => await _context.UsuariosWeb.FindAsync(id);
        public async Task<UsuarioWeb> CreateWebUserAsync(UsuarioWeb usuarioWeb) { _context.UsuariosWeb.Add(usuarioWeb); await _context.SaveChangesAsync(); return usuarioWeb; }
        public async Task UpdateWebUserAsync(UsuarioWeb usuarioWeb) { _context.UsuariosWeb.Update(usuarioWeb); await _context.SaveChangesAsync(); }

        // Métodos para Usuarios App
        public async Task<List<UsuarioAppSelectDTO>> ObtenerUsuariosAppSelectDTOAsync()
        {
            return await _context.UsuariosApp
                .Where(ua => ua.Usuario.Estado)
                .Include(ua => ua.Usuario)
                .OrderBy(ua => ua.Usuario.Nombre)
                .Select(ua => new UsuarioAppSelectDTO
                {
                    Id = ua.Id,
                    Nombre = ua.Usuario.Nombre
                })
                .AsNoTracking()
                .ToListAsync();
        }

        // Obtener Menú de Usuario
        public async Task<List<ModuloMenuDTO>> GetModulosUsuarioAsync(string email)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return new List<ModuloMenuDTO>();

            var rolId = usuario.RolId;

            var modulos = await _context.Modulos
                    .Where(m => m.RolModulos.Any(rm => rm.RolId == rolId))
                    .OrderBy(m => m.Orden)
                    .Select(m => new ModuloMenuDTO
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        Icono = m.Icono,
                        Permisos = m.Permisos
                            .Where(p => p.RolPermisos.Any(rp => rp.RolId == rolId))
                            .Select(p => new PermisoMenuDTO
                            {
                                Id = p.Id,
                                Nombre = p.Nombre,
                                Ruta = p.Ruta
                            }).ToList()
                    }).ToListAsync();

            // Agregar `Dashboard` para todos los usuarios
            modulos.Insert(0, new ModuloMenuDTO
            {
                Id = 0,
                Nombre = "Dashboard",
                Icono = "dashboard",
                Permisos = new List<PermisoMenuDTO>
                {
                    new PermisoMenuDTO
                    {
                        Id = 0, 
                        Nombre = "Welcome",
                        Ruta = "/welcome"
                    }
                }
            });

            return modulos;
        }

        // Obtener nombre de usuario desde UsuariosApp
        public async Task<string> ObtenerNombreCompletoAsync(int usuarioAppId)
        {
            var usuarioApp = await _context.UsuariosApp
                .Include(u => u.Usuario)
                .FirstOrDefaultAsync(u => u.Id == usuarioAppId);

            return usuarioApp?.Usuario?.Nombre ?? "Desconocido";
        }

        public async Task<int> ObtenerIdPorNombreUsuarioAsync(string nombreUsuario)
        {
            var usuarioApp = await _context.UsuariosApp
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

            if (usuarioApp == null)
                throw new BusinessException($"No se encontró un usuario con el nombre '{nombreUsuario}'.");

            return usuarioApp.Id;
        }

        public async Task<int?> ObtenerUsuarioIdPorSeccionIdAsync(int seccionId)
        {
            return await _context.UsuariosApp
                .Where(u => u.SeccionId == seccionId)
                .Select(u => (int?)u.Id)
                .FirstOrDefaultAsync();
        }

    }

}
