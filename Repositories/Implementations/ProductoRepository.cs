using Npgsql;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using ReyesAR.BasedeDatos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace ReyesAR.Repositories.Implementations
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly ReyesARContext _context;

        public ProductoRepository(ReyesARContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoEntity>> GetAllAsync()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Include(p => p.UnidadMedida)
                .ToListAsync();
        }

        public async Task<ProductoEntity?> GetByIdAsync(string clave)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Include(p => p.UnidadMedida)
                .FirstOrDefaultAsync(p => p.claveProducto == clave);
        }

        public async Task<ProductoEntity> CreateAsync(ProductoDTO p)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. El parámetro INOUT debe ser el primero (según la firma del SP que ajustamos)
                var parameterClave = new NpgsqlParameter("p_claveProducto", NpgsqlTypes.NpgsqlDbType.Varchar, 18)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = DBNull.Value
                };

                var parameters = new[]
                {
            parameterClave, // Posición 1
            new NpgsqlParameter("p_nombre", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = p.nombre },
            new NpgsqlParameter("p_descripcion", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = p.descripcion },
            new NpgsqlParameter("p_precio", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = p.precio },
            new NpgsqlParameter("p_codigo_barras", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)p.codigo_barras ?? DBNull.Value },
            new NpgsqlParameter("p_tipo_producto", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = p.tipo_producto },
            new NpgsqlParameter("p_cantidad", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = p.cantidad },
            new NpgsqlParameter("p_caducidad", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = p.caducidad },
            new NpgsqlParameter("p_claveCategoria", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = p.claveCategoria },
            new NpgsqlParameter("p_claveUnidadMedida", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = p.claveUnidadMedida },
            new NpgsqlParameter("p_claveUnidadVenta", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = p.claveUnidadVenta },
            new NpgsqlParameter("p_claveProveedor", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)p.claveProveedor ?? DBNull.Value },
            new NpgsqlParameter("p_imagen", NpgsqlTypes.NpgsqlDbType.Text) { Value = (object)p.ImagenUrl ?? DBNull.Value },
            new NpgsqlParameter("p_margen_ganancia", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = (object)p.margen_ganancia ?? 1.30m },
            new NpgsqlParameter("p_precio_venta", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = (object)p.precio_venta ?? DBNull.Value },
            new NpgsqlParameter("p_estado", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = p.estado }
        };

                _context.ChangeTracker.Clear();

                // 2. Ajustamos el CALL para que p_claveProducto sea el primer argumento
                await _context.Database.ExecuteSqlRawAsync(
                    @"CALL ""insertar_producto""(
                @p_claveProducto, 
                @p_nombre, 
                @p_descripcion, 
                @p_precio, 
                @p_codigo_barras, 
                @p_tipo_producto, 
                @p_cantidad, 
                @p_caducidad, 
                @p_claveCategoria, 
                @p_claveUnidadMedida, 
                @p_claveUnidadVenta, 
                @p_claveProveedor, 
                @p_imagen, 
                @p_margen_ganancia, 
                @p_precio_venta, 
                @p_estado)",
                    parameters
                );

                await transaction.CommitAsync();

                return new ProductoEntity
                {
                    claveProducto = parameterClave.Value.ToString(),
                    nombre = p.nombre,
                    descripcion = p.descripcion,
                    precio = p.precio,
                    codigo_barras = p.codigo_barras,
                    tipo_producto = p.tipo_producto,
                    cantidad = p.cantidad,
                    caducidad = p.caducidad,
                    claveCategoria = p.claveCategoria,
                    claveUnidadMedida = p.claveUnidadMedida,
                    claveUnidadVenta = p.claveUnidadVenta,
                    claveProveedor = p.claveProveedor,
                    margen_ganancia = p.margen_ganancia,
                    precio_venta = p.precio_venta,
                    Imagen = p.ImagenUrl,
                    estado = p.estado
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error en CreateAsync: {ex.Message}", ex);
            }
        }

        public async Task UpdateAsync(string clave, ProductoDTO p)
        {
            var parameters = new[]
            {
            new NpgsqlParameter("p_claveProducto", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = clave },
            new NpgsqlParameter("p_nombre", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = p.nombre },
            new NpgsqlParameter("p_descripcion", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)p.descripcion ?? DBNull.Value },
            new NpgsqlParameter("p_precio_costo", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = p.precio },
            new NpgsqlParameter("p_margen_ganancia", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = p.margen_ganancia },
            new NpgsqlParameter("p_precio_venta", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = (object)p.precio_venta ?? DBNull.Value },
            new NpgsqlParameter("p_codigo_barras", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)p.codigo_barras ?? DBNull.Value },
            new NpgsqlParameter("p_claveCategoria", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = p.claveCategoria },
            new NpgsqlParameter("p_claveUnidadMedida", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = p.claveUnidadMedida },
            new NpgsqlParameter("p_claveUnidadVenta", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = p.claveUnidadVenta },
            new NpgsqlParameter("p_estado", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = p.estado },
            new NpgsqlParameter("p_imagen", NpgsqlTypes.NpgsqlDbType.Text) { Value = (object)p.ImagenUrl ?? DBNull.Value }
        };

            try
            {
                // Se asume que el SP "actualizar_producto" sigue la misma lógica de comillas
                await _context.Database.ExecuteSqlRawAsync(
                    @"CALL public.""actualizar_producto""(
                    @p_claveProducto, @p_nombre, @p_descripcion, @p_precio_costo, 
                    @p_margen_ganancia, @p_precio_venta, @p_codigo_barras, 
                    @p_claveCategoria, @p_claveUnidadMedida, @p_claveUnidadVenta, 
                    @p_estado, @p_imagen)",
                    parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar producto {clave}: {ex.Message}");
            }
        }

        public async Task DeleteAsync(string clave)
        {
            var parameter = new NpgsqlParameter("p_claveProducto", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = clave };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(@"CALL public.""eliminar_producto""(@p_claveProducto)", parameter);
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error de base de datos al eliminar: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error general al eliminar: {ex.Message}");
            }
        }
    }
}
