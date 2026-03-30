using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using System.Data;

namespace ReyesAR.Repositories.Implementations
{

    public class UnidadVentaRepository : IUnidadVentaRepository
    {
        private readonly ReyesARContext _context;

        public UnidadVentaRepository(ReyesARContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UnidadVentaEntity>> GetAllAsync()
        {
            return await _context.UnidadVentas.ToListAsync();
        }

        public async Task<UnidadVentaEntity?> GetByIdAsync(string claveUnidadVenta)
        {
            return await _context.UnidadVentas
                .FirstOrDefaultAsync(u => u.claveUnidadVenta == claveUnidadVenta);
        }

        public async Task<UnidadVentaEntity> CreateAsync(UnidadVentaEntity unidadVenta)
        {
            var parameterClave = new NpgsqlParameter
            {
                ParameterName = "p_nueva_clave",
                NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar,
                Size = 18, // coincide con la definición en Postgres
                Direction = ParameterDirection.InputOutput,
                Value = DBNull.Value
            };

            await _context.Database.ExecuteSqlRawAsync(
                "CALL insertar_unidad_venta(@p_nombreUnidad, @p_abreviatura, @p_tipoValor, @p_nueva_clave, @p_estado)",
                new NpgsqlParameter("p_nombreUnidad", unidadVenta.nombreUnidad),
                new NpgsqlParameter("p_abreviatura", unidadVenta.abreviatura),
                new NpgsqlParameter("p_tipoValor", unidadVenta.tipoValor),
                parameterClave,
                new NpgsqlParameter("p_estado", unidadVenta.estado)
            );

            // Recuperar la clave generada desde el parámetro INOUT
            unidadVenta.claveUnidadVenta = parameterClave.Value?.ToString();

            return unidadVenta;
        }

        public async Task UpdateAsync(string claveUnidadVenta, UnidadVentaUpdateEstadoDto dto)
        {
            var sql = "CALL \"actualizar_estado_unidadVenta\"(@p_claveUnidadVenta, @p_estadoTexto);";

            await _context.Database.ExecuteSqlRawAsync(
                sql,
                new NpgsqlParameter("p_claveUnidadVenta", claveUnidadVenta),
                new NpgsqlParameter("p_estadoTexto", dto.EstadoTexto)
            );
        }

        public async Task DeleteAsync(string claveUnidadVenta)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL eliminar_unidadVenta({0})",
                claveUnidadVenta
            );
        }
    }



}
