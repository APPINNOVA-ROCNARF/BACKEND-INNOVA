using Application.DTO.MenuDTO;
using Application.DTO.UsuarioDTO;
using Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IUsuario
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

        // Métodos para Usuarios App
        Task<List<UsuarioAppSelectDTO>> ObtenerUsuariosAppSelectDTOAsync();

        // Obtener Menú de Usuario
        Task<List<ModuloMenuDTO>> GetModulosUsuarioAsync(string email);
        // Obtener nombre de usuario desde UsuarioApp
        Task<string> ObtenerNombreCompletoAsync(int usuarioAppId);
        // Obtener Id por Nombre de Usuario
        Task<int> ObtenerIdPorNombreUsuarioAsync(string nombreUsuario);
    }
}
