using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _userRepository;

        public UsuarioService(IUsuarioRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Métodos para User
        public async Task<IEnumerable<Usuario>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<Usuario> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<Usuario> CreateUserAsync(Usuario usuario)
        {
            return await _userRepository.CreateUserAsync(usuario);
        }

        public async Task UpdateUserAsync(Usuario usuario)
        {
            await _userRepository.UpdateUserAsync(usuario);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        // Métodos para Usuario Web

        public async Task<UsuarioWeb> CrearUsuarioWebAsync(NewUsuarioWebDTO usuarioDto)
        {
            // 1️⃣ Crear Usuario
            var nuevoUsuario = new Usuario
            {
                Nombre = usuarioDto.Nombre,
                Email = usuarioDto.Email,
                Password = usuarioDto.Password
            };

            nuevoUsuario = await _userRepository.CreateUserAsync(nuevoUsuario);

            // 2️⃣ Crear UsuarioWeb vinculado al Usuario
            var nuevoUsuarioWeb = new UsuarioWeb
            {
                UsuarioId = nuevoUsuario.Id,
                RolId = usuarioDto.RolId
            };

            nuevoUsuarioWeb = await _userRepository.CreateWebUserAsync(nuevoUsuarioWeb);

            return nuevoUsuarioWeb;
        }

        public async Task<UsuarioWeb> GetWebUserByIdAsync(int id)
        {
            return await _userRepository.GetWebUserByIdAsync(id);
        }

        public async Task<UsuarioWeb> CreateWebUserAsync(UsuarioWeb usuarioWeb)
        {
            return await _userRepository.CreateWebUserAsync(usuarioWeb);
        }

        public async Task UpdateWebUserAsync(UsuarioWeb usuarioWeb)
        {
            await _userRepository.UpdateWebUserAsync(usuarioWeb);
        }

    }
}
