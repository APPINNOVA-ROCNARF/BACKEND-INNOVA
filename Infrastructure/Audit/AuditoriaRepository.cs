using Application.Audit;
using DocumentFormat.OpenXml.InkML;
using Domain.Entities.Auditoria;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Audit
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly IDbContextFactory<AuditoriaDbContext> _contextFactory;

        private readonly Dictionary<string, Action<AuditoriaDbContext, AuditoriaBase>> _accionesGuardar;

        public AuditoriaRepository(IDbContextFactory<AuditoriaDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            _accionesGuardar = new Dictionary<string, Action<AuditoriaDbContext, AuditoriaBase>>(StringComparer.OrdinalIgnoreCase)
            {
                ["viaticos"] = (ctx, a) => ctx.AuditoriaViaticos.Add((AuditoriaViaticos)a),
                ["sistema"] = (ctx, a) => ctx.AuditoriaSistema.Add((AuditoriaSistema)a),
            };
        }

        public async Task AgregarAsync(AuditoriaBase auditoria, string modulo)
        {
            if (!_accionesGuardar.TryGetValue(modulo, out var accionGuardar))
                throw new ArgumentException($"Módulo de auditoría no soportado: {modulo}");

            await using var context = await _contextFactory.CreateDbContextAsync();
            accionGuardar(context, auditoria);
            await context.SaveChangesAsync();
        }

        public async Task<IList<HistorialAuditoriaDTO>> ObtenerHistorialAsync(string entidad, int entidadId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var logs = await context.AuditoriaViaticos
                .Where(a => a.EntidadAfectada == entidad && a.EntidadId == entidadId)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();

            var historial = logs
                .Select(a =>
                {
                    var datos = JObject.Parse(a.Datos);
                    return new HistorialAuditoriaDTO
                    {
                        Campo = datos.Value<string>("Campo"),
                        TipoEvento = a.TipoEvento,
                        ValorAnterior = datos.Value<string>("ValorAnterior"),
                        ValorNuevo = datos.Value<string>("ValorNuevo"),
                        Fecha = datos.Value<DateTime>("FechaEvento"),
                        Usuario = a.UsuarioNombre,
                    };
                })
                .OrderByDescending(h => h.Fecha)
                .ToList();

            return historial;
        }
        public async Task<Dictionary<int, bool>> FacturasVistas(
            string entidad,
            List<int> entidadIds,
            int usuarioId,
            string tipoEvento)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var idsVistos = await context.AuditoriaViaticos
                .Where(a =>
                    a.EntidadAfectada == entidad &&
                    entidadIds.Contains(a.EntidadId) &&
                    a.UsuarioId == usuarioId &&
                    a.TipoEvento == tipoEvento)
                .Select(a => a.EntidadId)
                .Distinct()
                .ToListAsync();

            var result = entidadIds.ToDictionary(id => id, id => idsVistos.Contains(id));
            return result;
        }
    }
}
