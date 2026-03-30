using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using System.Data;
using System;
using ReyesAR.BasedeDatos;

public class EmpleadoRepository : IEmpleadoRepository
{
    private readonly ReyesARContext _context;

    public EmpleadoRepository(ReyesARContext context)
    {
        _context = context;
    }

    // Obtener listado de empleados
    public async Task<IEnumerable<EmpleadoEntity>> GetAllAsync()
    {
        return await _context.Empleados.ToListAsync();
    }

    // Buscar empleado por clave
    public async Task<EmpleadoEntity?> GetByIdAsync(string claveEmpleado)
    {
        return await _context.Empleados
            .FirstOrDefaultAsync(e => e.claveEmpleado == claveEmpleado);
    }

    // Crear un nuevo empleado usando SP
    public async Task<EmpleadoEntity> CreateAsync(EmpleadoDTO dto)
    {
        using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand("CALL insertar_empleado(@p0, @p1, @p2, @p3, @p4, @p5)", conn);
        cmd.Parameters.Add(new NpgsqlParameter("p0", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = dto.nombre });
        cmd.Parameters.Add(new NpgsqlParameter("p1", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = dto.apellido_paterno });
        cmd.Parameters.Add(new NpgsqlParameter("p2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object?)dto.apellido_materno ?? DBNull.Value });
        cmd.Parameters.Add(new NpgsqlParameter("p3", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = dto.telefono });
        cmd.Parameters.Add(new NpgsqlParameter("p4", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = dto.curp });

        var claveParam = new NpgsqlParameter("p5", NpgsqlTypes.NpgsqlDbType.Varchar, 18)
        {
            Direction = ParameterDirection.InputOutput,
            Value = DBNull.Value
        };
        cmd.Parameters.Add(claveParam);

        await cmd.ExecuteNonQueryAsync();

        string nuevaClave = claveParam.Value == DBNull.Value
            ? string.Empty
            : (string)claveParam.Value;

        // Buscar por la clave que regresó el SP
        var entity = await _context.Empleados
            .FirstOrDefaultAsync(e => e.claveEmpleado == nuevaClave);

        if (entity == null)
            throw new KeyNotFoundException($"No se encontró el empleado recién creado con clave {nuevaClave}");

        return entity;
    }

    // Actualizar datos del empleado
    public async Task UpdateAsync(string claveEmpleado, EmpleadoDTO dto)
    {
        // Ejecutamos el procedimiento almacenado "seguro"
        // Nota: El orden de los parámetros debe coincidir con el PROCEDURE de SQL
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
        CALL actualizar_empleado_seguro(
            {claveEmpleado}, 
            {dto.nombre}, 
            {dto.apellido_paterno}, 
            {dto.apellido_materno}, 
            {dto.telefono}, 
            {dto.estado},
             {dto.curp}
        )");
        }
        catch (Exception ex)
        {
            // Si el SP lanza un RAISE EXCEPTION (ej. el empleado no existe o teléfono inválido), 
            // llegará aquí con el mensaje descriptivo de Postgres.
            throw new Exception(ex.Message);
        }
    }

    // Eliminación lógica (la BD se encarga de marcar estado)
    public async Task DeleteAsync(string claveEmpleado)
    {
        var entity = await _context.Empleados.FirstOrDefaultAsync(e => e.claveEmpleado == claveEmpleado);
        if (entity == null)
            throw new KeyNotFoundException($"Empleado {claveEmpleado} no existe.");

        // Aquí simplemente eliminas con EF, la BD maneja el estado
        _context.Empleados.Remove(entity);
        await _context.SaveChangesAsync();
    }
}