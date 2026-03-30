using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;

namespace ReyesAR.Repositories.Implementations
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly ReyesARContext _context;

        public DepartamentoRepository(ReyesARContext context)
        {
            _context = context;
        }

        // Consultar todos los departamentos
        public async Task<IEnumerable<DepartamentoEntity>> GetAllAsync()
        {
            return await _context.Departamentos.ToListAsync();
        }

        // Consultar un departamento por su clave
        public async Task<DepartamentoEntity?> GetByIdAsync(string claveDepartamento)
        {
            return await _context.Departamentos
                .FirstOrDefaultAsync(d => d.claveDepartamento == claveDepartamento);
        }


        // Insertar un nuevo departamento (capturando la PK generada)
        public async Task<DepartamentoEntity> CreateAsync(DepartamentoEntity departamento)
        {
            var claveDepartamentoParam = new NpgsqlParameter
            {
                ParameterName = "p_claveDepartamento",
                Direction = System.Data.ParameterDirection.InputOutput,
                NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar,
                Size = 18,
                Value = string.Empty
            };

            var nombreParam = new NpgsqlParameter<string>("p_nombre", departamento.nombre);
            var descripcionParam = new NpgsqlParameter<string>("p_descripcion", departamento.descripcion);
            var estadoParam = new NpgsqlParameter<bool>("p_estado", departamento.estado);

            await _context.Database.ExecuteSqlRawAsync(
                @"CALL insertar_departamento(@p_nombre, @p_descripcion, @p_claveDepartamento, @p_estado);",
                nombreParam, descripcionParam, claveDepartamentoParam, estadoParam
            );

            // Conversión explícita de object a string
            departamento.claveDepartamento = claveDepartamentoParam.Value?.ToString() ?? string.Empty;

            return departamento;
        }



        // Actualizar datos del departamento (ej. estado)
        public async Task UpdateAsync(string claveDepartamento, DepartamentoUpdateEstadoDto departamentoDto)
        {
            // Parámetros para el procedimiento
            var claveParam = new NpgsqlParameter<string>("p_claveDepartamento", claveDepartamento);
            var estadoTextoParam = new NpgsqlParameter<string>("p_estadoTexto", departamentoDto.EstadoTexto);

            // Ejecutar el procedimiento almacenado
            await _context.Database.ExecuteSqlRawAsync(
                @"CALL actualizar_estado_departamento(@p_claveDepartamento, @p_estadoTexto);",
                claveParam, estadoTextoParam
            );
        }

        // Eliminar por clave
        public async Task DeleteAsync(string claveDepartamento)
        {
            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(d => d.claveDepartamento == claveDepartamento);

            if (departamento == null)
                throw new KeyNotFoundException($"Departamento con clave {claveDepartamento} no encontrado.");

            _context.Departamentos.Remove(departamento);
            await _context.SaveChangesAsync();
        }
    }

}
