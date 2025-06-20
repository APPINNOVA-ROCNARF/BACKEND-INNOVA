using Application.DTO.VehiculoDTO;
using Application.Interfaces.IVehiculo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers.Vehiculo
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;

        public VehiculoController(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarVehiculo([FromBody] RegistrarVehiculoDTO dto)
        {

            try
            {
                await _vehiculoService.RegistrarVehiculoAsync(dto);
                return Ok(new { mensaje = "Vehículo registrado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

    }
}
