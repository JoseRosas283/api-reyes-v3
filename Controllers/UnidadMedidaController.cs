using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

namespace ReyesAR.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UnidadMedidaController : ControllerBase
    {
        private readonly IUnidadMedidaService _service;

        // Parametrización mediante el constructor
        public UnidadMedidaController(IUnidadMedidaService service)
        {
            _service = service;
        }

        // GET: api/UnidadMedida
        [HttpGet]
        public async Task<IActionResult> GetUnidades()
        {
            try
            {
                var unidades = await _service.ListarUnidadesAsync();

                // Validamos si la lista es nula o no tiene elementos
                if (unidades == null || !unidades.Any())
                {
                    return Ok(new { Mensaje = "No se encontraron unidades de medida registradas." });
                }

                return Ok(unidades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Ocurrió un error al obtener las unidades.", Error = ex.Message });
            }
        }

        // GET: api/UnidadMedida/getUnidades/{claveUnidad}
        [HttpGet("getUnidades/{claveUnidad}")]
        public async Task<IActionResult> GetUnidadById(string claveUnidad)
        {
            try
            {
                var unidad = await _service.ObtenerPorIdAsync(claveUnidad);
                if (unidad == null)
                    return NotFound(new { Mensaje = $"No se encontró la unidad con clave '{claveUnidad}'." });

                return Ok(unidad);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = ex.Message });
            }
        }

        // POST: api/UnidadMedida
        [HttpPost]
        public async Task<IActionResult> CrearUnidad([FromBody] UnidadMedidaDTO dto)
        {
            try
            {
                var nuevaUnidad = await _service.RegistrarUnidadAsync(dto);
                return Ok(new { Mensaje = "Unidad de medida creada exitosamente.", Datos = nuevaUnidad });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    mensaje = "Error al guardar en la base de datos.",
                    detalles = ex.Message
                });
            }
        }

        // PUT: api/UnidadMedida/putUnidades/{claveUnidad}
        [HttpPut("putUnidades/{claveUnidad}")]
        public async Task<IActionResult> ActualizarUnidad(string claveUnidad, [FromBody] UnidadMedidaUpdateEstadoDto dto)
        {
            try
            {
                await _service.UpdateAsync(claveUnidad, dto);
                return Ok(new { Mensaje = "Unidad de medida actualizada correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Error = "Validación de datos",
                    Detalle = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = ex.Message });
            }
        }

        // DELETE: api/UnidadMedida/deleteUnidades/{claveUnidad}
        [HttpDelete("deleteUnidades/{claveUnidad}")]
        public async Task<IActionResult> EliminarUnidad(string claveUnidad)
        {
            try
            {
                await _service.EliminarUnidadAsync(claveUnidad);
                return Ok(new { Mensaje = "Unidad de medida eliminada correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = ex.Message });
            }
        }
    }
}

