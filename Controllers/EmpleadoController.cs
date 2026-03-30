using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

[ApiController]
[Route("api/[controller]")]
public class EmpleadoController : ControllerBase
{
    private readonly IEmpleadoService _empleadoService;

    public EmpleadoController(IEmpleadoService empleadoService)
    {
        _empleadoService = empleadoService;
    }

    // GET: api/Empleado
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var empleados = await _empleadoService.GetAllAsync();
            return Ok(empleados);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
        }
    }

    // GET: api/Empleado/{clave}
    [HttpGet("ConsultarEmpleado/{claveEmpleado}")]
    public async Task<IActionResult> GetById(string claveEmpleado)
    {
        try
        {
            var empleado = await _empleadoService.GetByIdAsync(claveEmpleado);
            return Ok(empleado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
        }
    }

    // POST: api/Empleado
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmpleadoDTO empleado)
    {
        if (empleado == null)
            return BadRequest(new { mensaje = "El objeto empleado no puede ser nulo." });

        try
        {
            var nuevoEmpleado = await _empleadoService.CreateAsync(empleado);

            return CreatedAtAction(
                nameof(GetById),
                new { claveEmpleado = nuevoEmpleado.claveEmpleado },
                nuevoEmpleado
            );
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
        }
    }


    // PUT: api/Empleado/{clave}
    [HttpPut("actualizarEmpleado/{claveEmpleado}")]
    public async Task<IActionResult> Update(string claveEmpleado, [FromBody] EmpleadoDTO empleado)
    {
        try
        {
            await _empleadoService.UpdateAsync(claveEmpleado, empleado);
            return Ok(new { mensaje = $"Empleado {claveEmpleado} actualizado correctamente." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
        }
    }

    // DELETE: api/Empleado/{clave}
    [HttpDelete("eliminarEmpleado{claveEmpleado}")]
    public async Task<IActionResult> Delete(string claveEmpleado)
    {
        try
        {
            await _empleadoService.DeleteAsync(claveEmpleado);
            return Ok(new { mensaje = $"Empleado {claveEmpleado} eliminado correctamente." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
        }
    }
}