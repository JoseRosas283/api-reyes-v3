using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

namespace ReyesAR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly IProveedorService _service;

        public ProveedorController(IProveedorService service)
        {
            _service = service;
        }

        // GET: api/proveedor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorBaseDTO>>> GetAll()
        {
            var proveedores = await _service.GetAllAsync();

            if (proveedores == null || !proveedores.Any())
                return NotFound("No se encontraron proveedores registrados.");

            return Ok(proveedores);
        }

        // GET: api/proveedor/{id}
        [HttpGet("ConsultaProveedor/{claveProveedor}")]
        public async Task<ActionResult<ProveedorBaseDTO>> GetById(string claveProveedor)
        {
            var proveedor = await _service.GetByIdAsync(claveProveedor);
            if (proveedor == null)
                return NotFound($"Proveedor con clave {claveProveedor} no encontrado.");

            return Ok(proveedor);
        }

        // POST: api/proveedor
        [HttpPost]
        public async Task<ActionResult<ProveedorBaseDTO>> Create([FromBody] ProveedorCreateDTO dto)
        {
            var nuevoProveedor = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { claveProveedor = nuevoProveedor.ClaveProveedor }, nuevoProveedor);
        }

        // PUT: api/proveedor/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ProveedorCreateDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // DELETE: api/proveedor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
    
