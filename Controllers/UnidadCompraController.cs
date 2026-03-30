using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

namespace ReyesAR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadCompraController : ControllerBase
    {
        private readonly IUnidadCompraService _service;

        // Inyección del servicio por constructor
        public UnidadCompraController(IUnidadCompraService service)
        {
            _service = service;
        }

        // GET: api/UnidadCompra
        [HttpGet]
        public async Task<IActionResult> GetUnidades()
        {
            try
            {
                var unidades = await _service.ListarUnidadesAsync();

                if (unidades == null || !unidades.Any())
                {
                    return Ok(new { Mensaje = "No se encontraron unidades de compra registradas." });
                }

                return Ok(unidades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Ocurrió un error al obtener las unidades.", Error = ex.Message });
            }
        }

        // GET: api/UnidadCompra/getUnidades/{claveUnidadCompra}
        [HttpGet("getUnidades/{claveUnidadCompra}")]
        public async Task<IActionResult> GetUnidadById(string claveUnidadCompra)
        {
            try
            {
                var unidad = await _service.ObtenerPorIdAsync(claveUnidadCompra);
                if (unidad == null)
                    return NotFound(new { Mensaje = $"No se encontró la unidad de compra con clave '{claveUnidadCompra}'." });

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

        // POST: api/UnidadCompra
        [HttpPost]
        public async Task<IActionResult> CrearUnidad([FromBody] UnidadCompraDTO dto)
        {
            try
            {
                var nuevaUnidad = await _service.RegistrarUnidadAsync(dto);
                return Ok(new { Mensaje = "Unidad de compra creada exitosamente.", Datos = nuevaUnidad });
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

        // PUT: api/UnidadCompra/putUnidadCompra/{claveUnidadCompra}
        [HttpPut("putUnidadCompra/{claveUnidadCompra}")]
        public async Task<IActionResult> ActualizarUnidad(string claveUnidadCompra, [FromBody] UnidadCompraUpdateEstadoDto dto)
        {
            try
            {
                await _service.UpdateAsync(claveUnidadCompra, dto);
                return Ok(new { Mensaje = "Unidad de compra actualizada correctamente" });
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

        // DELETE: api/UnidadCompra/deleteUnidades/{claveUnidadCompra}
        [HttpDelete("deleteUnidades/{claveUnidadCompra}")]
        public async Task<IActionResult> EliminarUnidad(string claveUnidadCompra)
        {
            try
            {
                await _service.EliminarUnidadAsync(claveUnidadCompra);
                return Ok(new { Mensaje = "Unidad de compra eliminada correctamente" });
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
