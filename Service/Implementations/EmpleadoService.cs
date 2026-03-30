using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.Service.Interface;

public class EmpleadoService : IEmpleadoService
{
    private readonly IEmpleadoRepository _empleadoRepository;

    public EmpleadoService(IEmpleadoRepository empleadoRepository)
    {
        _empleadoRepository = empleadoRepository;
    }

    // Validación básica de campos
    private void ValidarEmpleado(EmpleadoDTO empleado)
    {
        if (empleado == null)
            throw new ArgumentNullException(nameof(empleado), "El empleado no puede ser nulo.");

        if (string.IsNullOrWhiteSpace(empleado.nombre))
            throw new ArgumentException("El nombre del empleado es obligatorio.");

        if (string.IsNullOrWhiteSpace(empleado.apellido_paterno))
            throw new ArgumentException("El apellido paterno es obligatorio.");

        if (string.IsNullOrWhiteSpace(empleado.telefono))
            throw new ArgumentException("El teléfono es obligatorio.");

        if (string.IsNullOrWhiteSpace(empleado.curp))
            throw new ArgumentException("El CURP es obligatorio.");
    }

    public async Task<IEnumerable<EmpleadoEntity>> GetAllAsync()
    {
        var empleados = await _empleadoRepository.GetAllAsync();
        if (!empleados.Any())
            throw new InvalidOperationException("No se encontraron empleados registrados.");
        return empleados;
    }

    public async Task<EmpleadoEntity?> GetByIdAsync(string claveEmpleado)
    {
        var empleado = await _empleadoRepository.GetByIdAsync(claveEmpleado);
        if (empleado == null)
            throw new KeyNotFoundException($"No se encontró el empleado con clave {claveEmpleado}.");
        return empleado;
    }

    public async Task<EmpleadoEntity> CreateAsync(EmpleadoDTO empleado)
    {
        ValidarEmpleado(empleado);
        return await _empleadoRepository.CreateAsync(empleado);
    }

    public async Task UpdateAsync(string claveEmpleado, EmpleadoDTO empleado)
    {
        ValidarEmpleado(empleado);
        var existente = await _empleadoRepository.GetByIdAsync(claveEmpleado);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el empleado con clave {claveEmpleado}.");
        await _empleadoRepository.UpdateAsync(claveEmpleado, empleado);
    }

    public async Task DeleteAsync(string claveEmpleado)
    {
        var existente = await _empleadoRepository.GetByIdAsync(claveEmpleado);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el empleado con clave {claveEmpleado}.");
        await _empleadoRepository.DeleteAsync(claveEmpleado);
    }
}