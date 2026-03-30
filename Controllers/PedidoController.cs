using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    // GET: api/pedido
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pedidos = await _pedidoService.GetAllAsync();

        if (pedidos == null || !pedidos.Any())
        {
            return NotFound(new { mensaje = "No se encontraron pedidos en el sistema." });
        }

        return Ok(pedidos);
    }

    // GET: api/pedido/ConsultarPedido/PED-1
    // Se agregó '/' para separar la acción del parámetro
    [HttpGet("ConsultarPedido/{clavePedido}")]
    public async Task<IActionResult> GetById(string clavePedido)
    {
        var pedido = await _pedidoService.GetByIdAsync(clavePedido);
        if (pedido == null)
            return NotFound(new { mensaje = $"No se encontró el pedido con clave {clavePedido}" });

        return Ok(pedido);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PedidoDTO pedido)
    {
        try
        {
            var entity = await _pedidoService.CreateAsync(pedido);

            // Importante: Usamos 'clavePedido' que es el nombre en tu Entity
            return CreatedAtAction(nameof(GetById), new { clavePedido = entity.clavePedido }, entity);
        }
        catch (PostgresException ex)
        {
            // Captura el RAISE EXCEPTION de tu procedimiento en ReyesAR
            return BadRequest(new
            {
                error = ex.MessageText,
                detalle = "Error en Base de Datos ReyesAR",
                codigo = ex.SqlState
            });
        }
        catch (ArgumentException ex)
        {
            // Captura las validaciones de campos vacíos del Service
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "Ocurrió un error interno en el servidor." });
        }
    }

    // PUT: api/pedido/actualizarPedido/PED-1
    [HttpPut("actualizarPedido/{clavePedido}")]
    public async Task<IActionResult> Update(string clavePedido, [FromBody] PedidoDTO pedido)
    {
        try
        {
            await _pedidoService.UpdateAsync(clavePedido, pedido);
            return Ok(new { mensaje = "Pedido actualizado correctamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // DELETE: api/pedido/eliminarPedido/PED-1
    [HttpDelete("eliminarPedido/{clavePedido}")]
    public async Task<IActionResult> Delete(string clavePedido)
    {
        try
        {
            await _pedidoService.DeleteAsync(clavePedido);
            return Ok(new { mensaje = $"Pedido {clavePedido} eliminado (lógicamente) con éxito." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}