using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ViaticoDTO;
using Application.Helpers;
using Application.Interfaces.ISistema;
using Application.Interfaces.IUsuario;
using Application.Interfaces.IViatico;

namespace Application.Services
{
    public class SolicitudViaticoService : ISolicitudViaticoService
    {
        private readonly ISolicitudViaticoRepository _solicitudRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly ISistemaService _cicloService;

        public SolicitudViaticoService(
            ISolicitudViaticoRepository repo,
            IUsuarioService usuarioAppService,
            ISistemaService cicloService)
        {
            _solicitudRepository = repo;
            _usuarioService = usuarioAppService;
            _cicloService = cicloService;
        }

        public async Task<List<SolicitudViaticoListDTO>> ObtenerSolicitudPorCicloAsync(int cicloId)
        {
            var solicitudes = await _solicitudRepository.ObtenerSolicitudPorCicloAsync(cicloId);

            var result = new List<SolicitudViaticoListDTO>();

            foreach (var solicitud in solicitudes)
            {
                var nombreUsuario = await _usuarioService.ObtenerNombreCompletoAsync(solicitud.UsuarioAppId);
                var nombreCiclo = await _cicloService.ObtenerNombreCicloAsync(solicitud.CicloId);

                result.Add(new SolicitudViaticoListDTO
                {
                    Id = solicitud.Id,
                    UsuarioNombre = nombreUsuario,
                    FechaRegistro = solicitud.FechaRegistro,
                    FechaModificacion = solicitud.FechaModificado,
                    Monto = solicitud.Monto,
                    Estado = solicitud.Estado.ToFriendlyString()
                });
            }

            return result;
        }
    }
}
