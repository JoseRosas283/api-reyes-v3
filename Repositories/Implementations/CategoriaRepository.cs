using Npgsql;
using ReyesAR.Models;
using ReyesAR.BasedeDatos;
using ReyesAR.Repositories.Interface;
using System;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace ReyesAR.Repositories.Implementations
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ReyesARContext _context;

        public CategoriaRepository(ReyesARContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaEntity>> GetAllAsync()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<CategoriaEntity?> GetByIdAsync(string clave)
        {
            return await _context.Categorias
                .FirstOrDefaultAsync(c => c.claveCategoria == clave);
        }

        public async Task<CategoriaEntity> CreateAsync(CategoriaEntity categoria)
        {
            var claveParam = new NpgsqlParameter
            {
                ParameterName = "p_claveCategoria",
                Direction = ParameterDirection.InputOutput,
                NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar,
                Size = 18,
                Value = string.Empty
            };

            var nombreParam = new NpgsqlParameter<string>("p_nombre", categoria.nombre);
            var descripcionParam = new NpgsqlParameter<string>("p_descripcion", categoria.descripcion);
            var departamentoParam = new NpgsqlParameter<string>("p_claveDepartamento", categoria.claveDepartamento);
            var estadoParam = new NpgsqlParameter<bool>("p_estado", categoria.estado);

            await _context.Database.ExecuteSqlRawAsync(
                @"CALL insertar_categoria(@p_nombre, @p_descripcion, @p_claveCategoria, @p_claveDepartamento, @p_estado);",
                nombreParam, descripcionParam, claveParam, departamentoParam, estadoParam
            );

            // Recuperar la clave generada
            categoria.claveCategoria = claveParam.Value?.ToString() ?? string.Empty;

            return categoria;
        }

        public async Task UpdateAsync(string clave, CategoriaEntity categoria)
        {
            // Aquí deberías tener un SP llamado actualizar_categoria que acepte también el estado
            await _context.Database.ExecuteSqlRawAsync(
                @"CALL actualizar_categoria(@p_claveCategoria, @p_nombre, @p_descripcion, @p_estado);",
                new NpgsqlParameter<string>("p_claveCategoria", clave),
                new NpgsqlParameter<string>("p_nombre", categoria.nombre),
                new NpgsqlParameter<string>("p_descripcion", categoria.descripcion),
                new NpgsqlParameter<bool>("p_estado", categoria.estado)
            );
        }

        public async Task DeleteAsync(string clave)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"CALL eliminar_categoria(@p_claveCategoria);",
                new NpgsqlParameter<string>("p_claveCategoria", clave)
            );
        }
    }

}
