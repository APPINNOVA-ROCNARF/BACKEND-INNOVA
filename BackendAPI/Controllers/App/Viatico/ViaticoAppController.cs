using Application.Interfaces.IArchivo;
using Application.Interfaces.IPresupuestoViatico;
using Application.Interfaces.IViatico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers.App.Viatico
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViaticoAppController : ControllerBase
    {
        private readonly IViaticoService _viaticoService;
        private readonly ISolicitudViaticoService _solicitudViaticoService;
        private readonly ICupoMensualService _cupoMensualService;
        private readonly IArchivoService _archivoService;
        private readonly IWebHostEnvironment _env;

        public ViaticoAppController(IViaticoService viaticoService, ISolicitudViaticoService solicitudViaticoService, ICupoMensualService cupoMensualService, IArchivoService archivoService, IWebHostEnvironment env)
        {
            _viaticoService = viaticoService;
            _solicitudViaticoService = solicitudViaticoService;
            _cupoMensualService = cupoMensualService;
            _archivoService = archivoService;
            _env = env;
        }

        [HttpGet("usuario")]
        public async Task<IActionResult> ObtenerPorUsuarioYCiclo(
            [FromQuery] string nombreUsuario,
            [FromQuery] int cicloId)
        {
            var viaticos = await _viaticoService.ObtenerViaticosPorUsuarioApp(nombreUsuario, cicloId);
            return Ok(viaticos);
        }

    }
}
