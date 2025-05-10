using Application.Audit;
using Domain.Common;
using Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EventHandlers.Auditoria
{
    public class EstadoViaticoCambiadoAuditoriaHandler : IDomainEventHandler<EstadoViaticoCambiadoEvent>
    {
        private readonly IAuditoriaService _auditoriaService;
        private readonly ILogger<EstadoViaticoCambiadoAuditoriaHandler> _logger;

        public EstadoViaticoCambiadoAuditoriaHandler(
            IAuditoriaService auditoriaService,
            ILogger<EstadoViaticoCambiadoAuditoriaHandler> logger)
        {
            _auditoriaService = auditoriaService;
            _logger = logger;
        }

        public async Task Handle(EstadoViaticoCambiadoEvent domainEvent)
        {
            var datos = new
            {
                ViaticoId = domainEvent.ViaticoId,
                EstadoAnterior = domainEvent.EstadoAnterior.ToString(),
                EstadoNuevo = domainEvent.EstadoNuevo.ToString(),
                FechaEvento = domainEvent.FechaEvento
            };

            await _auditoriaService.RegistrarEventoAsync(
                tipoEvento: "CambioEstadoViatico",
                datos: datos,
                entidad: "Viatico",
                entidadId: domainEvent.ViaticoId
                );

            _logger.LogInformation("Auditoría registrada para cambio de estado del viático {Id}", domainEvent.ViaticoId);
        }
    }
}
