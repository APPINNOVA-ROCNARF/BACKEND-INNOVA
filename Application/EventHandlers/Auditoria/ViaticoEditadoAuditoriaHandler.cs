using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Application.Audit;
using Domain.Common;
using Domain.Entities.Auditoria;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers.Auditoria
{
    public class ViaticoEditadoAuditoriaHandler : IDomainEventHandler<ViaticoEditadoEvent>
    {
        private readonly IAuditoriaService _auditoriaService;
        private readonly ILogger<ViaticoEditadoAuditoriaHandler> _logger;
        public ViaticoEditadoAuditoriaHandler(IAuditoriaService auditoriaService, ILogger<ViaticoEditadoAuditoriaHandler> logger)
        {
            _auditoriaService = auditoriaService;
            _logger = logger;
        }

        public async Task Handle(ViaticoEditadoEvent domainEvent)
        {
            await _auditoriaService.RegistrarEventoAsync(new AuditoriaEventoDTO
            {
                Modulo = ModulosAuditoria.Viaticos,
                TipoEvento = "CampoEditado",
                Entidad = "Viatico",
                EntidadId = domainEvent.ViaticoId,
                Datos = new
                {
                    Campo = domainEvent.CampoEditado,
                    ValorAnterior = domainEvent.ValorAnterior,
                    ValorNuevo = domainEvent.ValorNuevo,
                    FechaEdicion = domainEvent.FechaEvento
                }
            });

            _logger.LogInformation("Auditoría registrada para modificación de campo viático {Id}", domainEvent.ViaticoId);
        }
    }
}
