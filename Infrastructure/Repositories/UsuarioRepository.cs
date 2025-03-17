using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<Usuario>> GetAllUsersAsync() => await _context.Usuarios.ToListAsync();
        public async Task<Usuario> GetUserByIdAsync(int id) => await _context.Usuarios.FindAsync(id);
        public async Task<Usuario> CreateUserAsync(Usuario usuario) { _context.Usuarios.Add(usuario); await _context.SaveChangesAsync(); return usuario; }
        public async Task UpdateUserAsync(Usuario usuario) { _context.Usuarios.Update(usuario); await _context.SaveChangesAsync(); }
        public async Task DeleteUserAsync(int id) { var usuario = await _context.Usuarios.FindAsync(id); if (usuario != null) { _context.Usuarios.Remove(usuario); await _context.SaveChangesAsync(); } }

        // Métodos para WebUser
        public async Task<UsuarioWeb> GetWebUserByIdAsync(int id) => await _context.UsuariosWeb.FindAsync(id);
        public async Task<UsuarioWeb> CreateWebUserAsync(UsuarioWeb usuarioWeb) { _context.UsuariosWeb.Add(usuarioWeb); await _context.SaveChangesAsync(); return usuarioWeb; }
        public async Task UpdateWebUserAsync(UsuarioWeb usuarioWeb) { _context.UsuariosWeb.Update(usuarioWeb); await _context.SaveChangesAsync(); }
    }
}
