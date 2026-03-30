using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

namespace ReyesAR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadVentaController : ControllerBase
    {
        private readonly IUnidadVentaService _service;

        // Parametrización mediante el constructor
        public UnidadVentaController(IUnidadVentaService service)
        {
            _service = service;
        }

        // GET: api/UnidadVenta
        [HttpGet]
        public async Task<IActionResult> GetUnidades()
        {
            try
            {
                var unidades = await _service.ListarUnidadesAsync();

                if (unidades == null || !unidades.Any())
                {
                    return Ok(new { Mensaje = "No se encontraron unidades de venta registradas." });
                }

                return Ok(unidades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Ocurrió un error al obtener las unidades de venta.", Error = ex.Message });
            }
        }

        // GET: api/UnidadVenta/getUnidades/{claveUnidadVenta}
        [HttpGet("getUnidades/{claveUnidadVenta}")]
        public async Task<IActionResult> GetUnidadById(string claveUnidadVenta)
        {
            try
            {
                var unidad = await _service.ObtenerPorIdAsync(claveUnidadVenta);
                if (unidad == null)
                    return NotFound(new { Mensaje = $"No se encontró la unidad de venta con clave '{claveUnidadVenta}'." });

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

        // POST: api/UnidadVenta
        [HttpPost]
        public async Task<IActionResult> CrearUnidad([FromBody] UnidadVentaDTO dto)
        {
            try
            {
                var nuevaUnidad = await _service.RegistrarUnidadAsync(dto);
                return Ok(new { Mensaje = "Unidad de venta creada exitosamente.", Datos = nuevaUnidad });
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

        // PUT: api/UnidadVenta/putUnidades/{claveUnidadVenta}
        [HttpPut("putUnidadVenta/{claveUnidadVenta}")]
        public async Task<IActionResult> ActualizarUnidad(string claveUnidadVenta, [FromBody] UnidadVentaUpdateEstadoDto dto)
        {
            try
            {
                await _service.UpdateAsync(claveUnidadVenta, dto);
                return Ok(new { Mensaje = "Unidad de venta actualizada correctamente" });
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

        // DELETE: api/UnidadVenta/deleteUnidades/{claveUnidadVenta}
        [HttpDelete("deleteUnidades/{claveUnidadVenta}")]
        public async Task<IActionResult> EliminarUnidad(string claveUnidadVenta)
        {
            try
            {
                await _service.EliminarUnidadAsync(claveUnidadVenta);
                return Ok(new { Mensaje = "Unidad de venta eliminada correctamente" });
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
