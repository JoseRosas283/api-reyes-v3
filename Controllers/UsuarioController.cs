using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuarioController(IUsuarioService service)
    {
        _service = service;
    }

    // GET: api/Usuario
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _service.GetAllUsuariosAsync();

        if (usuarios == null || !usuarios.Any())
        {
            return Ok(new { mensaje = "No hay usuarios registrados en el sistema." });
        }

        return Ok(usuarios);
    }

    // GET: api/Usuario/{claveUsuario}
    [HttpGet("{claveUsuario}")]
    public async Task<IActionResult> GetById(string claveUsuario)
    {
        var usuario = await _service.GetUsuarioByIdAsync(claveUsuario);
        if (usuario == null) return NotFound(new { mensaje = $"El usuario {claveUsuario} no existe." });

        return Ok(usuario);
    }

    // GET: api/Usuario/username/{nombre_usuario}
    [HttpGet("username/{nombre_usuario}")]
    public async Task<IActionResult> GetByUsername(string nombre_usuario)
    {
        var usuario = await _service.GetUsuarioByUsernameAsync(nombre_usuario);
        if (usuario == null) return NotFound(new { mensaje = $"Nombre de usuario '{nombre_usuario}' no encontrado." });

        return Ok(usuario);
    }

    // POST: api/Usuario
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UsuarioDTO dto)
    {
        try
        {
            var resultado = await _service.CreateUsuarioAsync(dto);

            return CreatedAtAction(
                actionName: nameof(GetById),
                controllerName: "Usuario",
                routeValues: new { claveUsuario = resultado.claveUsuario },
                value: new { mensaje = "Usuario creado con éxito", data = resultado });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = "Error de validación", detalle = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = "Regla de negocio violada", detalle = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
        }
    }

    // PUT: api/Usuario/actualizar/{claveUsuario}
    [HttpPut("actualizar/{claveUsuario}")]
    public async Task<IActionResult> Update(string claveUsuario, [FromBody] UsuarioDTO dto)
    {
        try
        {
            await _service.UpdateUsuarioAsync(claveUsuario, dto);
            return NoContent(); // 204
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al actualizar", detalle = ex.Message });
        }
    }

    // DELETE: api/Usuario/{claveUsuario}
    [HttpDelete("{claveUsuario}")]
    public async Task<IActionResult> Delete(string claveUsuario)
    {
        try
        {
            await _service.DeleteUsuarioAsync(claveUsuario);
            return Ok(new { mensaje = $"Usuario {claveUsuario} eliminado correctamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = "No se pudo eliminar el usuario", detalle = ex.Message });
        }
    }
}