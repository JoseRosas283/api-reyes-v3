using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Service.Interface;

[ApiController]
[Route("api/[controller]")]
public class EntradaController : ControllerBase
{
    private readonly IEntradaService _service;

    public EntradaController(IEntradaService service)
    {
        _service = service;
    }

    // 1. Listado histórico de todas las entradas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntradaEntity>>> GetAll()
    {
        var entradas = await _service.GetAllAsync();
        return Ok(entradas);
    }

    // 2. Obtener entradas de una compra específica
    // GET: api/Entrada/compra/COM-001
    [HttpGet("compra/{claveCompra}")]
    public async Task<ActionResult<IEnumerable<EntradaEntity>>> GetByCompra(string claveCompra)
    {
        var entradas = await _service.GetByCompraAsync(claveCompra);
        return Ok(entradas);
    }

    // 3. Consultar una entrada por su folio único
    // GET: api/Entrada/ENT-1234567890123
    [HttpGet("{claveEntrada}")]
    public async Task<ActionResult<EntradaEntity>> GetById(string claveEntrada)
    {
        var entrada = await _service.GetByIdAsync(claveEntrada);

        if (entrada == null)
            return NotFound($"No se encontró la entrada con folio {claveEntrada}");

        return Ok(entrada);
    }

    // 4. Registrar nueva entrada (Llamada al SP "insertar_entrada")
    [HttpPost]
    public async Task<ActionResult<EntradaEntity>> Create([FromBody] EntradaDTO dto)
    {
        try
        {
            var resultado = await _service.CreateAsync(dto);
            return Ok(resultado);

            // Retornamos 201 Created y el objeto procesado
            /* return CreatedAtAction(nameof(GetById),
                new { claveEntrada = resultado.claveEntrada },
                resultado); */
        }
        catch (ArgumentException ex)
        {
            // Errores de validación del Service (campos vacíos, cantidades <= 0)
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            // Errores de PostgreSQL (Violación de reglas en el SP: Usuario incorrecto, falta caducidad, etc.)
            // Capturamos el mensaje del RAISE EXCEPTION
            return StatusCode(500, new
            {
                error = "Error en validación de almacén",
                detalle = ex.InnerException?.Message ?? ex.Message
            });
        }
    }

    // 5. Eliminar entrada (Anulación)
    [HttpDelete("{claveEntrada}")]
    public async Task<IActionResult> Delete(string claveEntrada)
    {
        try
        {
            await _service.DeleteAsync(claveEntrada);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}