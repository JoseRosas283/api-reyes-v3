using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

[ApiController]
[Route("api/[controller]")]
public class DetalleEntregaController : ControllerBase
{
    private readonly IDetalleEntregaService _service;

    public DetalleEntregaController(IDetalleEntregaService service)
    {
        _service = service;
    }

    // 1. Obtener todos los registros (Físicos)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DetalleEntregaEntity>>> GetAll()
    {
        var detalles = await _service.GetAllAsync();
        return Ok(detalles);
    }

    // 2. Obtener listado detallado por folio de Entrega
    // Ejemplo: GET api/DetalleEntrega/por-entrega/ENT-2026-001
    [HttpGet("por-entrega/{claveEntrega}")]
    public async Task<ActionResult<IEnumerable<DetalleEntregaEntity>>> GetByEntrega(string claveEntrega)
    {
        var detalles = await _service.GetDetailedListAsync(claveEntrega);
        return Ok(detalles);
    }

    // 3. Obtener un registro único (Usa los 4 identificadores)
    [HttpGet("{claveEntrega}/{claveProducto}/{tipo_detalle}/{extra}")]
    public async Task<ActionResult<DetalleEntregaEntity>> GetById(
        string claveEntrega, string claveProducto, string tipo_detalle, bool extra)
    {
        var detalle = await _service.GetByIdAsync(claveEntrega, claveProducto, tipo_detalle, extra);

        if (detalle == null) return NotFound("El detalle solicitado no existe.");

        return Ok(detalle);
    }

    // 4. Crear nuevo detalle (Llama al SP a través del Service)
    [HttpPost]
    public async Task<ActionResult<DetalleEntregaEntity>> Create([FromBody] DetalleEntregaDTO dto)
    {
        try
        {
            var resultado = await _service.CreateAsync(dto);

            // Retornamos 201 Created con la ruta para consultar el nuevo recurso
            return CreatedAtAction(nameof(GetById),
                new
                {
                    claveEntrega = resultado.claveEntrega,
                    claveProducto = resultado.claveProducto,
                    tipo_detalle = resultado.tipo_detalle,
                    extra = false // El SP determina si es extra, pero por defecto iniciamos consulta
                }, resultado);
        }
        catch (ArgumentException ex)
        {
            // Atrapa las validaciones de nulos/vacíos que pusimos en el Service
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // Atrapa errores de PostgreSQL (RAISE EXCEPTION del SP)
            return StatusCode(500, $"Error interno en Base de Datos: {ex.Message}");
        }
    }

    // 5. Actualizar registro existente
    [HttpPut("{claveEntrega}/{claveProducto}/{tipo_detalle}/{extra}")]
    public async Task<IActionResult> Update(
        string claveEntrega, string claveProducto, string tipo_detalle, bool extra,
        [FromBody] DetalleEntregaDTO dto)
    {
        try
        {
            await _service.UpdateAsync(claveEntrega, claveProducto, tipo_detalle, extra, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 6. Eliminar registro
    [HttpDelete("{claveEntrega}/{claveProducto}/{tipo_detalle}/{extra}")]
    public async Task<IActionResult> Delete(
        string claveEntrega, string claveProducto, string tipo_detalle, bool extra)
    {
        await _service.DeleteAsync(claveEntrega, claveProducto, tipo_detalle, extra);
        return NoContent();
    }
}