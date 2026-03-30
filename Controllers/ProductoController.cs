using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Service.Interface;

namespace ReyesAR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductoController(IProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _service.GetAllAsync();
            return Ok(productos ?? new List<ProductoEntity>());
        }

        // GET: api/Producto/ConsultarProducto/PROD-001
        [HttpGet("ConsultarProducto/{claveProducto}")] // Se agregó '/' para separar la ruta del parámetro
        public async Task<IActionResult> GetByClave(string claveProducto)
        {
            var producto = await _service.GetByClaveAsync(claveProducto);
            if (producto == null)
                return NotFound(new { Mensaje = $"No existe el producto con clave {claveProducto}" });

            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductoDTO dto, [FromForm] IFormFile Imagen)
        {
            try
            {
                // La validación de la imagen ya la hace el Service, pero dejarla aquí como primer filtro es buena práctica
                if (Imagen == null || Imagen.Length == 0)
                {
                    return BadRequest(new { Mensaje = "La imagen del producto es obligatoria." });
                }

                var nuevoProducto = await _service.CreateAsync(dto, Imagen);

                // Redirige a la acción de consulta para devolver el recurso creado
                return CreatedAtAction(nameof(GetByClave),
                    new { claveProducto = nuevoProducto.claveProducto }, nuevoProducto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = $"Error interno: {ex.Message}" });
            }
        }

        // PUT: api/Producto/actualizarProducto/PROD-001
        [HttpPut("actualizarProducto/{claveProducto}")]
        public async Task<IActionResult> Update(string claveProducto, [FromForm] ProductoDTO dto, [FromForm] IFormFile? archivoImagen)
        {
            try
            {
                // archivoImagen es opcional en la actualización
                var productoActualizado = await _service.UpdateAsync(claveProducto, dto, archivoImagen);
                return Ok(productoActualizado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = $"Error interno: {ex.Message}" });
            }
        }

        // DELETE: api/Producto/eliminarProducto/PROD-001
        [HttpDelete("eliminarProducto/{claveProducto}")]
        public async Task<IActionResult> Delete(string claveProducto)
        {
            try
            {
                await _service.DeleteAsync(claveProducto);
                return NoContent(); // 204 No Content es lo ideal para eliminaciones exitosas
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }
    }
}
