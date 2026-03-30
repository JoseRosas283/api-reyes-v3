using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;

[ApiController]
[Route("api/[controller]")]
public class EquivalenciaUnidadController : ControllerBase
{
    private readonly IEquivalenciaUnidadService _service;

    public EquivalenciaUnidadController(IEquivalenciaUnidadService service)
    {
        _service = service;
    }

    // GET: api/EquivalenciaUnidad
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var equivalencias = await _service.GetAllEquivalenciasAsync();

        if (equivalencias == null || !equivalencias.Any())
        {
            return Ok(new { mensaje = "No hay equivalencias de unidades registradas." });
        }

        return Ok(equivalencias);
    }

    // GET: api/EquivalenciaUnidad/producto/{claveProducto}/unidad/{claveUnidadCompra}
    [HttpGet("producto/{claveProducto}/unidad/{claveUnidadCompra}")]
    public async Task<IActionResult> GetById(string claveProducto, string claveUnidadCompra)
    {
        var equivalencia = await _service.GetEquivalenciaByIdAsync(claveProducto, claveUnidadCompra);

        if (equivalencia == null)
            return NotFound(new { mensaje = $"No existe la equivalencia para el producto {claveProducto} con la unidad {claveUnidadCompra}" });

        return Ok(equivalencia);
    }

    // POST: api/EquivalenciaUnidad
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EquivalenciaUnidadDTO dto)
    {
        try
        {
            var resultado = await _service.CreateEquivalenciaAsync(dto);

            // Retorna 201 Created con la ruta para consultar el nuevo registro
            return CreatedAtAction(nameof(GetById),
                new { claveProducto = resultado.claveProducto, claveUnidadCompra = resultado.claveUnidadCompra },
                resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = "Error de validación", detalle = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Captura los RAISE EXCEPTION del Store Procedure (Incompatibilidad de unidades, etc.)
            return Conflict(new { mensaje = "Error en reglas de negocio de la base de datos", detalle = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
        }
    }

    // PUT: api/EquivalenciaUnidad/producto/{claveProducto}/unidad/{claveUnidadCompra}
    [HttpPut("producto/{claveProducto}/unidad/{claveUnidadCompra}")]
    public async Task<IActionResult> Update(string claveProducto, string claveUnidadCompra, [FromBody] EquivalenciaUnidadDTO dto)
    {
        try
        {
            await _service.UpdateEquivalenciaAsync(claveProducto, claveUnidadCompra, dto);
            return NoContent(); // 204 Exitoso
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = "No se pudo actualizar la equivalencia", detalle = ex.Message });
        }
    }

    // DELETE: api/EquivalenciaUnidad/producto/{claveProducto}/unidad/{claveUnidadCompra}
    [HttpDelete("producto/{claveProducto}/unidad/{claveUnidadCompra}")]
    public async Task<IActionResult> Delete(string claveProducto, string claveUnidadCompra)
    {
        try
        {
            await _service.DeleteEquivalenciaAsync(claveProducto, claveUnidadCompra);
            return Ok(new { mensaje = "Equivalencia eliminada correctamente." });
        }
        catch (Exception ex)
        {
            return NotFound(new { mensaje = "Error al intentar eliminar", detalle = ex.Message });
        }
    }
}