using Application.DTO;
using Application.DTO.MenuDTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUsuarioService
    {
        // Métodos para Usuarios
        Task<IEnumerable<Usuario>> GetAllUsersAsync();
        Task<Usuario> GetUserByIdAsync(int id);
        Task<Usuario> CreateUserAsync(Usuario usuario);
        Task UpdateUserAsync(Usuario usuario);
        Task DeleteUserAsync(int id);

        // Métodos para Usuarios Web
        Task<UsuarioWeb> GetWebUserByIdAsync(int id);
        Task UpdateWebUserAsync(UsuarioWeb usuarioWeb);

        // Método para obtener el menú de usuario
        Task<List<ModuloMenuDTO>> GetModulosUsuarioAsync(string email);
    }
}
