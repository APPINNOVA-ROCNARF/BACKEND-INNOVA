﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.RolDTO;
using Application.Interfaces.IRol;

namespace Application.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepository;

        public RolService(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<List<RolSimpleDTO>> GetRolesAsync()
        {
            return await _rolRepository.GetRolesAsync();
        }

        public async Task<RolDTO> GetRolConModulosAsync(int rolId)
        {
            return await _rolRepository.GetRolConModulosAsync(rolId);
        }

        public async Task<List<ModuloRolDTO>> GetModulosAsync()
        {
            return await _rolRepository.GetModulosAsync();
        }

        public async Task CrearRolAsync(CrearRolRequestDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NombreRol))
                throw new ArgumentException("El nombre del rol no puede estar vacío.");
            await _rolRepository.CrearRolAsync(dto);
        }
    }
}

