using Application.Audit;
using Application.Interfaces.IUsuario;
using Domain.Entities.Auditoria;
using Domain.Entities.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IAuditoriaRepository _repository;
        private readonly IUsuarioActualService _usuarioActual;
        private readonly IHttpContextAccessor _http;
        private readonly ILogger<AuditoriaService> _logger;

        public AuditoriaService(IAuditoriaRepository repository, IUsuarioActualService usuarioActual, IHttpContextAccessor http, ILogger<AuditoriaService> logger)
        {
            _repository = repository;
            _usuarioActual = usuarioActual;
            _http = http;
            _logger = logger;
        }

        public async Task RegistrarEventoAsync(AuditoriaEventoDTO evento)
        {
            try
            {
                var usuario = _usuarioActual.Obtener();
                var context = _http.HttpContext;

                var datosJson = JsonSerializer.Serialize(evento.Datos);
                var ip = context?.Connection?.RemoteIpAddress?.ToString();
                var metodo = context?.Request?.Method;
                var ruta = context?.Request?.Path.Value;

                var hash = CalcularHash(
                    evento.TipoEvento,
                    DateTime.Now.ToString("O"),
                    evento.Entidad ?? "",
                    evento.EntidadId?.ToString() ?? "",
                    usuario.Id.ToString(),
                    usuario.Nombre,
                    datosJson,
                    ip ?? "",
                    metodo ?? "",
                ruta ?? ""
                );

                var auditoria = CrearInstanciaAuditoria(evento.Modulo);
                auditoria.TipoEvento = evento.TipoEvento;
                auditoria.Fecha = DateTime.UtcNow;
                auditoria.Datos = datosJson;
                auditoria.EntidadAfectada = evento.Entidad;
                auditoria.EntidadId = (int)evento.EntidadId;
                auditoria.UsuarioId = usuario.Id;
                auditoria.UsuarioNombre = usuario.Nombre;
                auditoria.IpCliente = ip;
                auditoria.MetodoHttp = metodo;
                auditoria.RutaAccedida = ruta;
                auditoria.Hash = hash;

                await _repository.AgregarAsync(auditoria, evento.Modulo);

                _logger.LogInformation("Auditoría registrada | Evento: {Evento} | Entidad: {Entidad} | ID: {EntidadId} | Usuario: {Usuario}",
                    evento.TipoEvento, evento.Entidad, evento.EntidadId, usuario.Nombre);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo registrar evento de auditoría para el módulo '{Modulo}'", evento.Modulo);

            }
        }

        private string CalcularHash(params string[] valores)
        {
            using var sha256 = SHA256.Create();
            var textoPlano = string.Join("|", valores);
            var bytes = Encoding.UTF8.GetBytes(textoPlano);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        private AuditoriaBase CrearInstanciaAuditoria(string modulo)
        {
            return modulo.ToLowerInvariant() switch
            {
                "viaticos" => new AuditoriaViaticos(),
                _ => throw new ArgumentException($"Módulo de auditoría no soportado: {modulo}")
            };
        }
    }
}
