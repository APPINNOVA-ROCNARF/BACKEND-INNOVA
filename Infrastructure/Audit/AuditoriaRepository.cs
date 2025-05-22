using Application.Audit;
using Domain.Entities.Auditoria;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Audit
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly AuditoriaDbContext _context;
        private readonly Dictionary<string, Action<AuditoriaBase>> _accionesGuardar;

        public AuditoriaRepository(AuditoriaDbContext context)
        {
            _context = context;

            _accionesGuardar = new Dictionary<string, Action<AuditoriaBase>>(StringComparer.OrdinalIgnoreCase)
            {
                ["viaticos"] = a => _context.AuditoriaViaticos.Add((AuditoriaViaticos)a),
            };
        }

        public async Task AgregarAsync(AuditoriaBase auditoria, string modulo)
        {
            if (!_accionesGuardar.TryGetValue(modulo, out var accionGuardar))
                throw new ArgumentException($"Módulo de auditoría no soportado: {modulo}");

            accionGuardar(auditoria);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<HistorialAuditoriaDTO>> ObtenerHistorialAsync(string entidad, int entidadId)
        {
            var logs = await _context.AuditoriaViaticos
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
    }
}
