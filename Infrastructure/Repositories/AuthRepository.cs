using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context; 
        }

        public async Task<Usuario> GetUsuarioByEmail(string email)
        {
            return await _context.Usuarios
                .Include(u => u.UsuarioWeb)
                    .ThenInclude(uw => uw.Rol)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
