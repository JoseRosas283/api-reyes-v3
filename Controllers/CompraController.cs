using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;

[ApiController]
[Route("api/[controller]")]
public class CompraController : ControllerBase
{
    private readonly ICompraService _service;

    public CompraController(ICompraService service)
    {
        _service = service;
    }

    // 1. Obtener todas las compras
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var compras = await _service.GetAllAsync();
            return Ok(compras);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    // 2. Obtener compra por ClaveCompra
    [HttpGet("{claveCompra}")]
    public async Task<IActionResult> GetById(string claveCompra)
    {
        try
        {
            var compra = await _service.GetByIdAsync(claveCompra);
            return Ok(compra);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    // 3. Crear Compra (Recibe Form-Data con Imágenes)
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CompraDTO compraDto)
    {
        try
        {
            if (compraDto == null)
                return BadRequest("Los datos de la compra son nulos.");

            // El Service se encargará de convertir facturaImagen/documentoImagen a URL
            var nuevaCompra = await _service.CreateAsync(compraDto);

            // Retornamos 201 Created con el objeto que ya trae la Clave generada por el SP
            return CreatedAtAction(nameof(GetById), new { claveCompra = nuevaCompra.ClaveCompra }, nuevaCompra);
        }
        catch (ArgumentException ex)
        {
            // Errores de validación de negocio (C#)
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // Errores lanzados por el SP (RAISE EXCEPTION de Postgres)
            return StatusCode(500, $"Error al registrar la compra: {ex.Message}");
        }
    }

    // 4. Actualizar Estado (Operativo o de Compra Directa)
    [HttpPatch("{claveCompra}/estado")]
    public async Task<IActionResult> UpdateEstado(string claveCompra, [FromBody] string nuevoEstado)
    {
        try
        {
            var actualizado = await _service.UpdateEstadoAsync(claveCompra, nuevoEstado);
            return Ok(actualizado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 5. Cancelar Compra
    [HttpDelete("{claveCompra}")]
    public async Task<IActionResult> Cancelar(string claveCompra)
    {
        try
        {
            var cancelada = await _service.CancelarAsync(claveCompra);
            return Ok(cancelada);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}