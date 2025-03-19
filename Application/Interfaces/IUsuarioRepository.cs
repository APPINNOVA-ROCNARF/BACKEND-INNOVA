using Application.DTO.MenuDTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUsuarioRepository
    {
        // Métodos para Usuarios
        Task<IEnumerable<Usuario>> GetAllUsersAsync();
        Task<Usuario> GetUserByIdAsync(int id);
        Task<Usuario> CreateUserAsync(Usuario usuario);
        Task UpdateUserAsync(Usuario usuario);
        Task DeleteUserAsync(int id);

        // Métodos para Usuarios Web
        Task<UsuarioWeb> GetWebUserByIdAsync(int id);
        Task<UsuarioWeb> CreateWebUserAsync(UsuarioWeb usuarioWeb);
        Task UpdateWebUserAsync(UsuarioWeb usuarioWeb);

        // Obtener Menú de Usuario
        Task<List<ModuloDTO>> GetModulosUsuarioAsync(string email);
    }
}
