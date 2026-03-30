using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

namespace ReyesAR.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ruta base: api/Categoria
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriaController(ICategoriaService service)
        {
            _service = service;
        }

        // GET: api/Categoria
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _service.ListarCategoriasAsync();

            if (categorias == null || !categorias.Any())
            {
                return Ok(new { mensaje = "No hay categorías registradas." });
            }

            return Ok(categorias);
        }
        // GET: api/Categoria/{claveCategoria}
        [HttpGet("{claveCategoria}")]
        public async Task<IActionResult> GetById(string claveCategoria)
        {
            var categoria = await _service.ObtenerPorIdAsync(claveCategoria);
            if (categoria == null)
                return NotFound(new { mensaje = $"No se encontró la categoría con clave: {claveCategoria}" });

            return Ok(categoria);
        }

        // POST: api/Categoria
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoriaDTO dto)
        {
            try
            {
                var resultado = await _service.RegistrarCategoriaAsync(dto);

                // Retorna 201 Created con la clave generada
                return CreatedAtAction(
                    nameof(GetById),
                    new { claveCategoria = resultado.claveCategoria },
                    resultado
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
            }
        }

        // PUT: api/Categoria/actualizarCategoria/{claveCategoria}
        [HttpPut("actualizarCategoria/{claveCategoria}")]
        public async Task<IActionResult> Update(string claveCategoria, [FromBody] CategoriaDTO dto)
        {
            try
            {
                await _service.ActualizarCategoriaAsync(claveCategoria, dto);
                return NoContent(); // 204 Exitoso pero sin contenido en el cuerpo
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = "Error de validación", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
            }
        }

        // DELETE: api/Categoria/{claveCategoria}
        [HttpDelete("{claveCategoria}")]
        public async Task<IActionResult> Delete(string claveCategoria)
        {
            try
            {
                await _service.EliminarCategoriaAsync(claveCategoria);
                return Ok(new { mensaje = $"Categoría {claveCategoria} eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "No se pudo eliminar la categoría", detalle = ex.Message });
            }
        }
    }

}
