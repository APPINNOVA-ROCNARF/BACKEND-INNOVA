using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Audit;
using Domain.Common;
using Domain.Entities.Auditoria;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers.Auditoria
{
    public class FacturaEditadaAuditoriaHandler : IDomainEventHandler<FacturaEditadaEvent>
    {
        private readonly IAuditoriaService _auditoriaService;
        private readonly ILogger<FacturaEditadaAuditoriaHandler> _logger;

        public FacturaEditadaAuditoriaHandler(IAuditoriaService auditoriaService, ILogger<FacturaEditadaAuditoriaHandler> logger)
        {
            _auditoriaService = auditoriaService;
            _logger = logger;
        }

        public async Task Handle(FacturaEditadaEvent domainEvent)
        {
            try
            {
                await _auditoriaService.RegistrarEventoAsync(new AuditoriaEventoDTO
                {
                    Modulo = ModulosAuditoria.Viaticos,
                    TipoEvento = "EdicionFactura",
                    Entidad = "Factura",
                    EntidadId = domainEvent.FacturaId,
                    Datos = new
                    {
                        FacturaId = domainEvent.FacturaId,
                        Campo = domainEvent.Campo,
                        ValorAnterior = domainEvent.ValorAnterior,
                        ValorNuevo = domainEvent.ValorNuevo,
                        FechaEvento = domainEvent.FechaEvento
                    }
                });

                _logger.LogInformation("Auditoría registrada para modificación de campo de factura {Id}", domainEvent.FacturaId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo registrar evento de auditoría de modificación de campo de factura {Id}", domainEvent.FacturaId);
            }
        }
    }
}
