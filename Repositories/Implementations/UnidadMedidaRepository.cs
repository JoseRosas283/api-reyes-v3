using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using System;
using System.Data;

namespace ReyesAR.Repositories.Implementations
{
    public class UnidadMedidaRepository : IUnidadMedidaRepository
    {
        private readonly ReyesARContext _context;

        public UnidadMedidaRepository(ReyesARContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UnidadMedidaEntity>> GetAllAsync()
        {
            return await _context.UnidadMedidas.ToListAsync();
        }

        public async Task<UnidadMedidaEntity?> GetByIdAsync(string claveUnidad)
        {
            return await _context.UnidadMedidas
                .FirstOrDefaultAsync(u => u.claveUnidad == claveUnidad);
        }

        public async Task<UnidadMedidaEntity> CreateAsync(UnidadMedidaEntity unidadMedida)
        {
            var parameterClave = new NpgsqlParameter
            {
                ParameterName = "p_nueva_clave",
                NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar,
                Size = 18,
                Direction = ParameterDirection.InputOutput,
                Value = DBNull.Value
            };

            await _context.Database.ExecuteSqlRawAsync(
                "CALL insertar_unidad_medida(@p_nombreUnidad, @p_abreviatura, @p_tipoStock, @p_nueva_clave, @p_estado)",
                new NpgsqlParameter("p_nombreUnidad", unidadMedida.nombreUnidad),
                new NpgsqlParameter("p_abreviatura", unidadMedida.abreviatura),
                new NpgsqlParameter("p_tipoStock", unidadMedida.tipoStock),
                parameterClave,
                new NpgsqlParameter("p_estado", unidadMedida.estado)
            );

            // Recuperar la clave generada desde el parámetro INOUT
            unidadMedida.claveUnidad = parameterClave.Value?.ToString();

            return unidadMedida;
        }

        public async Task UpdateAsync(string claveUnidad, UnidadMedidaUpdateEstadoDto dto)
        {
            var sql = "CALL \"actualizar_estado_unidad\"(@p_claveUnidad, @p_estadoTexto);";

            await _context.Database.ExecuteSqlRawAsync(
                sql,
                new NpgsqlParameter("p_claveUnidad", claveUnidad),
                new NpgsqlParameter("p_estadoTexto", dto.EstadoTexto)
            );
        }

        public async Task DeleteAsync(string claveUnidad)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL eliminar_unidad_medida({0})",
                claveUnidad
            );
        }
    }
}
