using Microsoft.AspNetCore.Mvc;
using ReyesAR.DTO;
using ReyesAR.Service.Interface;

namespace ReyesAR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoService _departamentoService;

        public DepartamentoController(IDepartamentoService departamentoService)
        {
            _departamentoService = departamentoService;
        }

        // GET: api/departamento
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departamentos = await _departamentoService.GetAllAsync();

            if (departamentos == null || !departamentos.Any())
                return NotFound("No se encontraron departamentos registrados.");

            return Ok(departamentos);
        }


        // GET: api/departamento/{clave}
        [HttpGet("getDepartamento/{claveDepartamento}")]
        public async Task<IActionResult> GetById(string claveDepartamento)
        {
            var departamento = await _departamentoService.GetByIdAsync(claveDepartamento);
            if (departamento == null)
                return NotFound($"Departamento con clave {claveDepartamento} no encontrado.");

            return Ok(departamento);
        }

        // POST: api/departamento
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartamentoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var departamento = await _departamentoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { claveDepartamento = departamento.claveDepartamento }, departamento);
        }

        // PUT: api/departamento/{clave}
        [HttpPut("putDepartamento/{claveDepartamento}")]
        public async Task<IActionResult> Update(string claveDepartamento, [FromBody] DepartamentoUpdateEstadoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _departamentoService.UpdateAsync(claveDepartamento, dto);
            return NoContent();
        }

        // DELETE: api/departamento/{clave}
        [HttpDelete("deleteDepartamento/{claveDepartamento}")]
        public async Task<IActionResult> Delete(string claveDepartamento)
        {
            await _departamentoService.DeleteAsync(claveDepartamento);
            return NoContent();
        }
    }
}
