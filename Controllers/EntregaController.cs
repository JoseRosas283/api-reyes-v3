using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

[Route("api/[controller]")]
[ApiController]
public class EntregaController : ControllerBase
{
    private readonly IEntregaService _service;

    public EntregaController(IEntregaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntregaEntity>>> GetAll()
    {
        var entregas = await _service.GetAllEntregasAsync();
        return Ok(entregas);
    }

    [HttpGet("{claveEntrega}")]
    public async Task<ActionResult<EntregaEntity>> GetByClave(string claveEntrega)
    {
        var entrega = await _service.GetEntregaByClaveAsync(claveEntrega);
        if (entrega == null) return NotFound($"No se encontró la entrega con clave: {claveEntrega}");

        return Ok(entrega);
    }

    [HttpPost]
    public async Task<ActionResult<EntregaEntity>> Create([FromBody] EntregaDTO entregaDTO)
    {
        try
        {
            var nuevaEntrega = await _service.CreateEntregaAsync(entregaDTO);
            if (nuevaEntrega == null) return BadRequest("No se pudo registrar la entrega.");

            return CreatedAtAction(nameof(GetByClave), new { claveEntrega = nuevaEntrega.claveEntrega }, nuevaEntrega);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    [HttpPut("{claveEntrega}")]
    public async Task<ActionResult<EntregaEntity>> Update(string claveEntrega, [FromBody] EntregaDTO entregaDTO)
    {
        try
        {
            var actualizada = await _service.UpdateEntregaAsync(claveEntrega, entregaDTO);
            if (actualizada == null) return NotFound("Entrega no encontrada para actualizar.");

            return Ok(actualizada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{claveEntrega}")]
    public async Task<IActionResult> Delete(string claveEntrega)
    {
        var eliminado = await _service.DeleteEntregaAsync(claveEntrega);
        if (!eliminado) return NotFound("No se pudo eliminar porque la entrega no existe.");

        return NoContent();
    }
}