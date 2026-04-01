using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

[ApiController]
[Route("api/[controller]")]
public class DetallePedidoController : ControllerBase
{
    private readonly IDetallePedidoService _service;

    public DetallePedidoController(IDetallePedidoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var detalles = await _service.GetAllDetailsAsync();

        if (detalles == null || !detalles.Any())
        {
            return Ok(new { mensaje = "No se encontraron registros de detalles de pedidos en la base de datos." });
        }

        return Ok(detalles);
    }

    [HttpGet("consultarpedido/{clavePedido}/producto/{claveProducto}")]
    public async Task<IActionResult> GetById(string clavePedido, string claveProducto)
    {
        var detalle = await _service.GetDetailByIdAsync(clavePedido, claveProducto);

        if (detalle == null)
            return NotFound($"No se encontró el detalle para el pedido {clavePedido} y producto {claveProducto}");

        return Ok(detalle);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DetallePedidoDTO detalleDto)
    {
        try
        {
            var resultado = await _service.CreateDetailAsync(detalleDto);

            // Usamos los nombres de propiedad de la Entidad retornada
            return CreatedAtAction(nameof(GetById),
                new { clavePedido = resultado.Clavepedido, claveProducto = resultado.Claveproducto },
                resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error en Base de Datos: {ex.Message}");
        }
    }

    [HttpPut("pedido/{clavePedido}/producto/{claveProducto}")]
    public async Task<IActionResult> Update(string clavePedido, string claveProducto, [FromBody] DetallePedidoDTO detalleDto)
    {
        try
        {
            // Tu service ahora retorna la Entidad, no un bool
            var actualizado = await _service.UpdateDetailAsync(clavePedido, claveProducto, detalleDto);

            return Ok(actualizado); // Retornamos el objeto actualizado
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

    [HttpDelete("pedido/{clavePedido}/producto/{claveProducto}")]
    public async Task<IActionResult> Delete(string clavePedido, string claveProducto)
    {
        try
        {
            // Tu service ahora retorna la Entidad eliminada
            var eliminado = await _service.DeleteDetailAsync(clavePedido, claveProducto);

            return Ok(new { mensaje = "Registro eliminado con éxito", datos = eliminado });
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
            return StatusCode(500, $"Error al eliminar: {ex.Message}");
        }
    }
}