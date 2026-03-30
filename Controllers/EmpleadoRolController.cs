using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

[ApiController]
[Route("api/[controller]")]
public class EmpleadoRolController : ControllerBase
{
    private readonly IEmpleadoRolService _service;

    public EmpleadoRolController(IEmpleadoRolService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var historial = await _service.GetAllHistorialAsync();

        if (historial == null || !historial.Any())
        {
            return Ok(new { mensaje = "No se encontraron registros en el historial de roles." });
        }

        return Ok(historial);
    }

    // Obtener historial de un empleado específico
    [HttpGet("empleado/{claveEmpleado}")]
    public async Task<IActionResult> GetByEmpleado(string claveEmpleado)
    {
        var historial = await _service.GetHistorialByEmpleadoAsync(claveEmpleado);
        if (!historial.Any()) return NotFound($"No hay historial para el empleado {claveEmpleado}");

        return Ok(historial);
    }

    // Ruta con terna de llaves: api/EmpleadoRol/empleado/EMP01/rol/ADMIN/fecha/2024-03-30
    [HttpGet("empleado/{claveEmpleado}/rol/{claveRol}/fecha/{fechaInicio}")]
    public async Task<IActionResult> GetById(string claveEmpleado, string claveRol, DateTime fechaInicio)
    {
        var registro = await _service.GetByKeysAsync(claveEmpleado, claveRol, fechaInicio);
        if (registro == null) return NotFound("Registro de historial no encontrado.");

        return Ok(registro);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRol([FromBody] EmpleadoRolDTO dto)
    {
        try
        {
            var resultado = await _service.AssignRolAsync(dto);

            // Retornamos 201 Created y la ruta para consultar este registro específico
            return CreatedAtAction(nameof(GetById),
                new
                {
                    claveEmpleado = resultado.claveEmpleado,
                    claveRol = resultado.claveRol,
                    fechaInicio = resultado.fecha_inicio.ToString("yyyy-MM-dd")
                },
                resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            // Aquí caen los errores del SP (ej: "Ya tiene un rol activo")
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    [HttpPut("empleado/{claveEmpleado}/rol/{claveRol}/fecha/{fechaInicio}")]
    public async Task<IActionResult> Update(string claveEmpleado, string claveRol, DateTime fechaInicio, [FromBody] EmpleadoRolDTO dto)
    {
        try
        {
            // En el service definimos que esto no devuelve bool, sino Task (promesa)
            await _service.UpdateHistorialAsync(claveEmpleado, claveRol, fechaInicio, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("empleado/{claveEmpleado}/rol/{claveRol}/fecha/{fechaInicio}")]
    public async Task<IActionResult> Delete(string claveEmpleado, string claveRol, DateTime fechaInicio)
    {
        try
        {
            await _service.DeleteHistorialAsync(claveEmpleado, claveRol, fechaInicio);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}