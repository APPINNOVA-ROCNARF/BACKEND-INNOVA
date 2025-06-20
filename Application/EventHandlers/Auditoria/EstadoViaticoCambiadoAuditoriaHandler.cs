using Application.Audit;
using Domain.Common;
using Domain.Entities.Auditoria;
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
            try
            {
                var datos = new
                {
                    ViaticoId = domainEvent.ViaticoId,
                    Campo = domainEvent.Campo,
                    ValorAnterior = domainEvent.ValorAnterior.ToString(),
                    ValorNuevo = domainEvent.ValorNuevo.ToString(),
                    FechaEvento = domainEvent.FechaEvento
                };

                await _auditoriaService.RegistrarEventoAsync(new AuditoriaEventoDTO
                {
                    Modulo = ModulosAuditoria.Viaticos,
                    TipoEvento = "CambioEstadoViatico",
                    Datos = datos,
                    Entidad = "Viatico",
                    EntidadId = domainEvent.ViaticoId
                });

                _logger.LogInformation("Auditoría registrada para cambio de estado del viático {Id}", domainEvent.ViaticoId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo registrar evento de auditoría para cambio de estado del viático {Id}", domainEvent.ViaticoId);
            }
        }
    }
}
