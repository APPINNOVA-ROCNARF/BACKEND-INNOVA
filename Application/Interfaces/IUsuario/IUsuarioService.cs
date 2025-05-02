using Application.DTO;
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

        // Métodos para Usuarios App
        Task<List<UsuarioAppSelectDTO>> ObtenerUsuariosAppSelectDTOAsync();
        // Método para obtener el menú de usuario
        Task<List<ModuloMenuDTO>> GetModulosUsuarioAsync(string email);
        // Método para obtener nombre del usuario desde UsuarioApp
        Task<string> ObtenerNombreCompletoAsync(int usuarioAppId);
        // Método para obtener Id por Nombre Usuario
        Task<int> ObtenerIdPorNombreUsuario(string NombreUsuario);

    }
}
