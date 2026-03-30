using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

namespace AbarrotesRS.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ruta base: api/Roles
    public class RolesController : ControllerBase
    {
        private readonly IRolService _service;

        public RolesController(IRolService service)
        {
            _service = service;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _service.GetAllAsync();
            return Ok(roles);
        }

        // GET: api/Roles/{claveRol}
        [HttpGet("{claveRol}")]
        public async Task<IActionResult> GetById(string claveRol)
        {
            var rol = await _service.GetByIdAsync(claveRol);
            if (rol == null) return NotFound($"No se encontró el rol con clave: {claveRol}");

            return Ok(rol);
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RolDTO dto)
        {
            try
            {
                var resultado = await _service.CreateAsync(dto);
                // Retorna 201 Created
                return CreatedAtAction(nameof(GetById), new { claveRol = resultado.ClaveRol },
                  new { mensaje = "Rol registrado correctamente", rol = resultado });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT: api/Roles/{claveRol}
        [HttpPut("{claveRol}")]
        public async Task<IActionResult> Update(string claveRol, [FromBody] RolDTO dto)
        {
            try
            {
                dto.ClaveRol = claveRol; // asegurar que la clave se use
                var actualizado = await _service.UpdateAsync(dto);
                if (actualizado == null) return NotFound($"No se encontró el rol con clave: {claveRol}");

                return Ok(new { mensaje = "Rol actualizado correctamente", rol = actualizado });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = "Error de validación", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
            }
        }

        // DELETE: api/Roles/{claveRol}
        [HttpDelete("{claveRol}")]
        public async Task<IActionResult> Delete(string claveRol)
        {
            try
            {
                var eliminado = await _service.DeleteAsync(claveRol);
                if (!eliminado) return NotFound($"No se encontró el rol con clave: {claveRol}");

                return Ok(new { mensaje = $"Rol {claveRol} eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "No se pudo eliminar el rol", detalle = ex.Message });
            }
        }
    }
}