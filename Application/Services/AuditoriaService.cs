using Application.Audit;
using Domain.Entities.Auditoria;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IAuditoriaRepository _repository;
        private readonly ILogger<AuditoriaService> _logger;

        public AuditoriaService(IAuditoriaRepository repository, ILogger<AuditoriaService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task RegistrarEventoAsync(
            string tipoEvento,
            object datos,
            string? entidad = null,
            int? entidadId = null,
            int? usuarioId = null)
        {
            var registro = new AuditoriaRegistro
            {
                TipoEvento = tipoEvento,
                Fecha = DateTime.UtcNow,
                Datos = JsonSerializer.Serialize(datos),
                EntidadAfectada = entidad,
                EntidadId = entidadId,
                UsuarioId = usuarioId
            };

            await _repository.AgregarAsync(registro);

            _logger.LogInformation("Auditoría registrada | Evento: {Evento} | Entidad: {Entidad} | ID: {EntidadId}",
                tipoEvento, entidad, entidadId);
        }
    }
}
