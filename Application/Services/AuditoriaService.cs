using Application.Audit;
using Application.Interfaces.IUsuario;
using Domain.Entities.Auditoria;
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

        public async Task RegistrarEventoAsync(
            string tipoEvento,
            object datos,
            string? entidad = null,
            int? entidadId = null,
            int? usuarioId = null)
        {
            var usuario = _usuarioActual.Obtener();
            var context = _http.HttpContext;

            var datosJson = JsonSerializer.Serialize(datos);
            var ip = context?.Connection?.RemoteIpAddress?.ToString();
            var metodo = context?.Request?.Method;
            var ruta = context?.Request?.Path.Value;

            var hash = CalcularHash(
                tipoEvento,
                DateTime.UtcNow.ToString("O"),
                entidad ?? "",
                entidadId?.ToString() ?? "",
                usuario.Id.ToString(),
                usuario.Nombre,
                datosJson,
                ip ?? "",
                metodo ?? "",
                ruta ?? ""
            );

            var registro = new AuditoriaRegistro
            {
                TipoEvento = tipoEvento,
                Fecha = DateTime.UtcNow,
                Datos = datosJson,
                EntidadAfectada = entidad,
                EntidadId = entidadId,
                UsuarioId = usuario.Id,
                UsuarioNombre = usuario.Nombre,
                IpCliente = ip,
                MetodoHttp = metodo,
                RutaAccedida = ruta,
                Hash = hash
            };

            await _repository.AgregarAsync(registro);

            _logger.LogInformation("Auditoría registrada | Evento: {Evento} | Entidad: {Entidad} | ID: {EntidadId} | Usuario: {Usuario}",
                tipoEvento, entidad, entidadId, usuario.Nombre);
        }

        private string CalcularHash(params string[] valores)
        {
            using var sha256 = SHA256.Create();
            var textoPlano = string.Join("|", valores);
            var bytes = Encoding.UTF8.GetBytes(textoPlano);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
