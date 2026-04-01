using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;

[ApiController]
[Route("api/[controller]")]
public class RepresentanteController : ControllerBase
{
    private readonly IRepresentanteService _service;

    // Corregido: Ahora inyecta IRepresentanteService
    public RepresentanteController(IRepresentanteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var representantes = await _service.GetAllRepresentantesAsync();

        if (representantes == null || !representantes.Any())
        {
            return Ok(new { mensaje = "No se encontraron registros de representantes." });
        }

        return Ok(representantes);
    }

    [HttpGet("{claveRepresentante}")]
    public async Task<IActionResult> GetById(string claveRepresentante)
    {
        var representante = await _service.GetRepresentanteByIdAsync(claveRepresentante);

        if (representante == null)
            return NotFound($"No se encontró el representante con clave: {claveRepresentante}");

        return Ok(representante);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RepresentanteDTO representanteDto)
    {
        try
        {
            var resultado = await _service.CreateRepresentanteAsync(representanteDto);

            return CreatedAtAction(
                nameof(GetById),
                new { claveRepresentante = resultado.claveRepresentante },
                resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // Captura errores del Procedimiento Almacenado de Postgres (RAISE EXCEPTION)
            return StatusCode(500, $"Error en Base de Datos: {ex.Message}");
        }
    }

    [HttpPut("{claveRepresentante}")]
    public async Task<IActionResult> Update(string claveRepresentante, [FromBody] RepresentanteDTO representanteDto)
    {
        try
        {
            var actualizado = await _service.UpdateRepresentanteAsync(claveRepresentante, representanteDto);
            return Ok(actualizado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al actualizar: {ex.Message}");
        }
    }

    [HttpDelete("{claveRepresentante}")]
    public async Task<IActionResult> Delete(string claveRepresentante)
    {
        try
        {
            var eliminado = await _service.DeleteRepresentanteAsync(claveRepresentante);
            return Ok(new { mensaje = "Registro eliminado con éxito", datos = eliminado });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al eliminar: {ex.Message}");
        }
    }
}