using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Audit;
using Domain.Common;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers.Auditoria
{
    public class ArchivoAccedidoAuditoriaHandler : IDomainEventHandler<ArchivoAccedidoEvent>
    {
        private readonly IAuditoriaService _auditoriaService;
        private readonly ILogger<ArchivoAccedidoAuditoriaHandler> _logger;

        public ArchivoAccedidoAuditoriaHandler(
            IAuditoriaService auditoriaService,
            ILogger<ArchivoAccedidoAuditoriaHandler > logger)
        {
            _auditoriaService = auditoriaService;
            _logger = logger;
        }

        public async Task Handle(ArchivoAccedidoEvent domainEvent)
        {
            try
            {
                var datos = new
                {
                    NombreArchivo = domainEvent.NombreArchivo,
                    RutaRelativa = domainEvent.RutaRelativa,
                    ModoAcceso = domainEvent.ModoAcceso,
                    FechaEvento = domainEvent.FechaEvento,
                };

                await _auditoriaService.RegistrarEventoAsync(new AuditoriaEventoDTO
                {
                    Modulo = domainEvent.Modulo,
                    TipoEvento = "ArchivoAccedido",
                    Datos = datos,
                    Entidad = domainEvent.Entidad,
                    EntidadId = domainEvent.EntidadId,
                });

                _logger.LogInformation("Auditoría registrada para el acceso de un archivo de la entidad {entidad} con el id {Id}", domainEvent.Entidad, domainEvent.EntidadId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo registrar evento de auditoría para el módulo '{Modulo}'", domainEvent.Modulo);
            }
        }
    }
}
