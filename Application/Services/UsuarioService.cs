﻿using Application.DTO;
using Application.DTO.MenuDTO;
using Application.DTO.UsuarioDTO;
using Application.Interfaces.IUsuario;
using Domain.Entities.Usuarios;
using Microsoft.Extensions.Caching.Memory;
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

        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);

        public UsuarioService(IUsuarioRepository userRepository, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        // Métodos para User
        public async Task<IEnumerable<UsuarioListDTO>> GetAllUsersAsync()
        {
            var usuarios = await _userRepository.GetAllUsersAsync();

            var usuariosDTO = usuarios.Select(u => new UsuarioListDTO
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                Rol = u.Rol.Nombre,
                Estado = u.Estado,
                CreadoEn = u.CreadoEn,
                ModificadoEn = u.ModificadoEn
            }).ToList();

            return usuariosDTO;
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

        public async Task<UsuarioWeb> GetWebUserByIdAsync(int id)
        {
            return await _userRepository.GetWebUserByIdAsync(id);
        }


        public async Task UpdateWebUserAsync(UsuarioWeb usuarioWeb)
        {
            await _userRepository.UpdateWebUserAsync(usuarioWeb);
        }

        // Métodos para Usuario App
        public async Task<List<UsuarioAppSelectDTO>> ObtenerUsuariosAppSelectDTOAsync()
        {
            return await _userRepository.ObtenerUsuariosAppSelectDTOAsync();
        }

        // Obtener Menú de Usuario
        public async Task<List<ModuloMenuDTO>> GetModulosUsuarioAsync(string email)
        {
            // Clave para el cache
            var cacheKey = $"ModulosUsuario-{email}";

            if(!_cache.TryGetValue(cacheKey, out List<ModuloMenuDTO> modulos))
            {
                modulos = await _userRepository.GetModulosUsuarioAsync(email);

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheExpiration
                };

                // Guardar en cache
                _cache.Set(cacheKey, modulos, _cacheExpiration);
            }

            return modulos;
        }

        // Obtener nombre de usuario desde UsuarioApp
        public async Task<String> ObtenerNombreCompletoAsync(int usuarioAppId)
        {
            return await _userRepository.ObtenerNombreCompletoAsync(usuarioAppId);
        }

        public async Task<int> ObtenerIdPorNombreUsuario(string NombreUsuario)
        {
            return await _userRepository.ObtenerIdPorNombreUsuarioAsync(NombreUsuario);
        }

        public async Task<int?> ObtenerUsuarioIdPorSeccionIdAsync(int seccionId)
        {
            return await _userRepository.ObtenerUsuarioIdPorSeccionIdAsync(seccionId);
        }

    }
}
