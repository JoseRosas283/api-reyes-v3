using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Service.Interface;

[ApiController]
[Route("api/[controller]")]
public class DetalleCompraDirectaController : ControllerBase
{
    private readonly IDetalleCompraDirectaService _service;

    public DetalleCompraDirectaController(IDetalleCompraDirectaService service)
    {
        _service = service;
    }

    // 1. Obtener todos los detalles (General)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DetalleCompraDirectaEntity>>> GetAll()
    {
        var detalles = await _service.GetAllAsync();
        return Ok(detalles);
    }

    // 2. Obtener artículos de una compra específica
    // Ejemplo: GET api/DetalleCompraDirecta/por-compra/COM-2026-001
    [HttpGet("por-compra/{claveCompra}")]
    public async Task<ActionResult<IEnumerable<DetalleCompraDirectaEntity>>> GetByCompra(string claveCompra)
    {
        var detalles = await _service.GetByCompraAsync(claveCompra);
        return Ok(detalles);
    }

    // 3. Obtener un registro único por su llave compuesta
    // Ejemplo: GET api/DetalleCompraDirecta/COM-100/PROD-01
    [HttpGet("{claveCompra}/{claveProducto}")]
    public async Task<ActionResult<DetalleCompraDirectaEntity>> GetById(string claveCompra, string claveProducto)
    {
        var detalle = await _service.GetByIdAsync(claveCompra, claveProducto);

        if (detalle == null)
            return NotFound($"No se encontró el producto {claveProducto} en la compra {claveCompra}.");

        return Ok(detalle);
    }

    // 4. Crear nuevo detalle (Ejecuta el SP a través del Service)
    [HttpPost]
    public async Task<ActionResult<DetalleCompraDirectaEntity>> Create([FromBody] DetalleCompraDirectaDTO dto)
    {
        try
        {
            var resultado = await _service.CreateAsync(dto);

            // Retornamos 201 Created y la ruta para consultar este nuevo recurso
            return CreatedAtAction(nameof(GetById),
                new { claveCompra = resultado.claveCompra, claveProducto = resultado.claveProducto },
                resultado);
        }
        catch (ArgumentException ex)
        {
            // Errores de validación (campos vacíos, cantidades <= 0)
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // Errores de PostgreSQL (RAISE EXCEPTION del SP)
            return StatusCode(500, $"Error en el servidor o base de datos: {ex.Message}");
        }
    }

    // 5. Actualizar registro existente
    [HttpPut("{claveCompra}/{claveProducto}")]
    public async Task<IActionResult> Update(string claveCompra, string claveProducto, [FromBody] DetalleCompraDirectaDTO dto)
    {
        try
        {
            await _service.UpdateAsync(claveCompra, claveProducto, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 6. Eliminar registro
    [HttpDelete("{claveCompra}/{claveProducto}")]
    public async Task<IActionResult> Delete(string claveCompra, string claveProducto)
    {
        try
        {
            await _service.DeleteAsync(claveCompra, claveProducto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}