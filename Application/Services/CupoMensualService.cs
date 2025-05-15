using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.PresupuestoViaticoDTO;
using Application.Interfaces.IPresupuestoViatico;
using Application.Interfaces.ISistema;
using Application.Interfaces.IUsuario;
using Domain.Entities.Viaticos;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CupoMensualService : ICupoMensualService
    {
        private readonly ICupoMensualRepository _repository;
        private readonly ISistemaService _sistemaService;
        private readonly IUsuarioService _usuarioService;

        public CupoMensualService(ICupoMensualRepository repository, ISistemaService sistemaService, IUsuarioService usuarioService)
        {
            _repository = repository;
            _sistemaService = sistemaService;
            _usuarioService = usuarioService;
        }

        public async Task<(int cuposInsertados, List<string> sectoresNoEncontrados)>
                CargarCuposDesdeSectoresAsync(List<CupoMensualDTO> sectores, int cicloId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Paso 1: Borrar cupos anteriores por ciclo
            await _repository.DeleteByCicloAsync(cicloId, cancellationToken);

            var cupos = new List<CupoMensual>();
            var sectoresNoEncontrados = new List<string>();

            foreach (var sector in sectores)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Buscar SeccionId desde Código
                var seccionId = await _sistemaService.ObtenerIdPorCodigoSeccionAsync(sector.Sector);
                if (seccionId == null)
                {
                    sectoresNoEncontrados.Add($"Sección no encontrada: {sector.Sector}");
                    continue;
                }

                // Buscar UsuarioApp.Id desde SeccionId
                var usuarioId = await _usuarioService.ObtenerUsuarioIdPorSeccionIdAsync(seccionId.Value);
                if (usuarioId == null)
                {
                    sectoresNoEncontrados.Add($"Usuario no encontrado para sección: {sector.Sector}");
                    continue;
                }

                // Crear Cupos
                if (sector.CupoMovilidad.HasValue)
                {
                    cupos.Add(new CupoMensual
                    {
                        UsuarioId = usuarioId.Value,
                        CicloId = cicloId,
                        Categoria = "Movilización",
                        MontoAsignado = sector.CupoMovilidad.Value,
                    });
                }

                if (sector.CupoHospedaje.HasValue)
                {
                    cupos.Add(new CupoMensual
                    {
                        UsuarioId = usuarioId.Value,
                        CicloId = cicloId,
                        Categoria = "Hospedaje",
                        MontoAsignado = sector.CupoHospedaje.Value,
                    });
                }

                if (sector.CupoAlimentacion.HasValue)
                {
                    cupos.Add(new CupoMensual
                    {
                        UsuarioId = usuarioId.Value,
                        CicloId = cicloId,
                        Categoria = "Alimentación",
                        MontoAsignado = sector.CupoAlimentacion.Value,
                    });
                }
            }

            // Paso 2: Insertar en base de datos
            await _repository.AddRangeAsync(cupos, cancellationToken);

            return (cupos.Count, sectoresNoEncontrados);
        }
        public async Task<bool> ExisteCargaParaCicloAsync(int cicloId)
        {
            return await _repository.ExisteParaCicloAsync(cicloId);
        }
    }
}
