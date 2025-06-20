using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Audit;
using Domain.Common;
using Domain.Entities.Auditoria;
using Domain.Entities.Viaticos;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers.Auditoria
{
    public class ViaticoCreadoAuditoriaHandler : IDomainEventHandler<ViaticoCreadoEvent>
    {
        private readonly IAuditoriaService _auditoriaService;
        private readonly ILogger<ViaticoCreadoAuditoriaHandler> _logger;

        public ViaticoCreadoAuditoriaHandler(
            IAuditoriaService auditoriaService,
            ILogger<ViaticoCreadoAuditoriaHandler> logger)
        {
            _auditoriaService = auditoriaService;
            _logger = logger;
        }

        public async Task Handle(ViaticoCreadoEvent domainEvent)
        {
            try
            {
                var datos = new
                {
                    ViaticoId = domainEvent.ViaticoId,
                    CategoriaId = domainEvent.CategoriaId,
                    SubcategoriaId = domainEvent.SubcategoriaId,
                    Monto = domainEvent.Monto,
                    FechaEvento = domainEvent.FechaEvento,
                };

                await _auditoriaService.RegistrarEventoAsync(new AuditoriaEventoDTO
                {
                    Modulo = ModulosAuditoria.Viaticos,
                    TipoEvento = "ViaticoCreado",
                    Datos = datos,
                    Entidad = "Viatico",
                    EntidadId = domainEvent.ViaticoId
                });

                _logger.LogInformation("Auditoria registrada para creación de viático {Id}", domainEvent.ViaticoId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo registrar evento de auditoría para creación del viático");
            }
        }
    }
}
