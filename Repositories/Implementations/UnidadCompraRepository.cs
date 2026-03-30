using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using System.Data;

namespace ReyesAR.Repositories.Implementations
{
    public class UnidadCompraRepository : IUnidadCompraRepository
    {
        private readonly ReyesARContext _context;

        public UnidadCompraRepository(ReyesARContext context)
        {
            _context = context;
        }

        // GET → Consultar todas las unidades de compra
        public async Task<IEnumerable<UnidadCompraEntity>> GetAllAsync()
        {
            return await _context.UnidadCompras.ToListAsync();
        }

        // GET → Consultar una unidad de compra por su clave
        public async Task<UnidadCompraEntity?> GetByIdAsync(string claveUnidadCompra)
        {
            return await _context.UnidadCompras
                .FirstOrDefaultAsync(u => u.claveUnidadCompra == claveUnidadCompra);
        }

        // POST → Insertar nueva unidad de compra (capturando la PK generada)
        public async Task<UnidadCompraEntity> CreateAsync(UnidadCompraEntity unidadCompra)
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
                "CALL insertar_unidad_compra(@p_nombre_unidad, @p_abreviatura, @p_tipo_stock, @p_nueva_clave, @p_estado)",
                new NpgsqlParameter("p_nombre_unidad", unidadCompra.nombre_unidad),
                new NpgsqlParameter("p_abreviatura", unidadCompra.abreviatura),
                new NpgsqlParameter("p_tipo_stock", unidadCompra.tipo_stock),
                parameterClave,
                new NpgsqlParameter("p_estado", unidadCompra.estado)
            );

            // Recuperar la clave generada desde el parámetro INOUT
            unidadCompra.claveUnidadCompra = parameterClave.Value?.ToString();

            return unidadCompra;
        }


        // PUT → Actualizar estado de la unidad de compra
        public async Task UpdateAsync(string claveUnidadCompra, UnidadCompraUpdateEstadoDto dto)
        {
            var sql = "CALL \"actualizar_estado_unidadCompra\"(@p_claveUnidadCompra, @p_estadoTexto);";

            await _context.Database.ExecuteSqlRawAsync(
                sql,
                new NpgsqlParameter("p_claveUnidadCompra", claveUnidadCompra),
                new NpgsqlParameter("p_estadoTexto", dto.EstadoTexto)
            );
        }

        // DELETE → Eliminar unidad de compra por clave
        public async Task DeleteAsync(string claveUnidadCompra)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL eliminar_unidadCompra({0})",
                claveUnidadCompra
            );
        }
    }

}
