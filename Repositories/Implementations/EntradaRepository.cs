using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

public class EntradaRepository : IEntradaRepository
{
    private readonly ReyesARContext _context;

    public EntradaRepository(ReyesARContext context)
    {
        _context = context;
    }

    // 1. Obtener todas las entradas con sus relaciones (Eager Loading)
    public async Task<IEnumerable<EntradaEntity>> GetAllAsync()
    {
        return await _context.Entradas
            // .Include(e => e.Producto)
            // .Include(e => e.Usuario)
            .AsNoTracking() // añadir 
            .OrderByDescending(e => e.fecha)
            .ToListAsync();
    }

    // 2. Obtener entradas de una compra específica
    public async Task<IEnumerable<EntradaEntity>> GetByCompraAsync(string claveCompra)
    {
        return await _context.Entradas
            // .Include(e => e.Producto)
            .AsNoTracking() // añadir 
            .Where(e => e.claveCompra == claveCompra.Trim())
            .ToListAsync();
    }

    // 3. Buscar una entrada por su folio único
    public async Task<EntradaEntity?> GetByIdAsync(string claveEntrada)
    {
        return await _context.Entradas
            // .Include(e => e.Producto)
            // .Include(e => e.Compra)
            .FirstOrDefaultAsync(e => e.claveEntrada == claveEntrada.Trim());
    }

    // 4. Implementación del Procedimiento Almacenado "insertar_entrada"

    public async Task<EntradaEntity> CreateAsync(EntradaEntity entrada)
    {
        var p_cantidad = new NpgsqlParameter("p_cantidad_recibida", entrada.cantidad_recibida);

        // ← CORREGIDO: tipo Date explícito
        var p_fecha = new NpgsqlParameter("p_fecha", NpgsqlTypes.NpgsqlDbType.Date)
        {
            Value = entrada.fecha.Date
        };

        var p_tipo = new NpgsqlParameter("p_tipo_entrada", entrada.tipo_entrada.Trim());
        var p_producto = new NpgsqlParameter("p_claveProducto", entrada.claveProducto.Trim());
        var p_compra = new NpgsqlParameter("p_claveCompra", entrada.claveCompra.Trim());
        var p_usuario = new NpgsqlParameter("p_claveUsuario", entrada.claveUsuario.Trim());

        // ← CORREGIDO: tipo Date explícito para caducidad
        var p_caducidad = new NpgsqlParameter("p_fecha_caducidad", NpgsqlTypes.NpgsqlDbType.Date)
        {
            Value = entrada.fecha_caducidad.HasValue
                ? (object)entrada.fecha_caducidad.Value.Date
                : DBNull.Value
        };

        var p_obs = new NpgsqlParameter("p_observaciones",
            (object)entrada.observaciones?.Trim() ?? DBNull.Value);

        await _context.Database.ExecuteSqlRawAsync(
            "CALL \"insertar_entrada\"(@p_cantidad_recibida, @p_fecha, @p_tipo_entrada, @p_claveProducto, @p_claveCompra, @p_claveUsuario, @p_fecha_caducidad, @p_observaciones)",
            p_cantidad, p_fecha, p_tipo, p_producto, p_compra, p_usuario, p_caducidad, p_obs
        );

        return entrada;
    }

    /* public async Task<EntradaEntity> CreateAsync(EntradaEntity entrada)
    {
        // Definición de parámetros según la firma del SP
        var p_cantidad = new NpgsqlParameter("p_cantidad_recibida", entrada.cantidad_recibida);
        var p_fecha = new NpgsqlParameter("p_fecha", entrada.fecha);
        var p_tipo = new NpgsqlParameter("p_tipo_entrada", entrada.tipo_entrada.Trim());
        var p_producto = new NpgsqlParameter("p_claveProducto", entrada.claveProducto.Trim());
        var p_compra = new NpgsqlParameter("p_claveCompra", entrada.claveCompra.Trim());
        var p_usuario = new NpgsqlParameter("p_claveUsuario", entrada.claveUsuario.Trim());

        // Parámetros con posibilidad de Nulos (DEFAULT NULL en Postgres)
        var p_caducidad = new NpgsqlParameter("p_fecha_caducidad",
            entrada.fecha_caducidad.HasValue ? (object)entrada.fecha_caducidad.Value : DBNull.Value);

        var p_obs = new NpgsqlParameter("p_observaciones",
            (object)entrada.observaciones?.Trim() ?? DBNull.Value);

        // Ejecución del CALL con comillas dobles para evitar errores de case-sensitivity
        await _context.Database.ExecuteSqlRawAsync(
            "CALL \"insertar_entrada\"(@p_cantidad_recibida, @p_fecha, @p_tipo_entrada, @p_claveProducto, @p_claveCompra, @p_claveUsuario, @p_fecha_caducidad, @p_observaciones)",
            p_cantidad, p_fecha, p_tipo, p_producto, p_compra, p_usuario, p_caducidad, p_obs
        );

        return entrada;
    } */

    // 5. Eliminar entrada
    public async Task DeleteAsync(string claveEntrada)
    {
        var entrada = await GetByIdAsync(claveEntrada);
        if (entrada != null)
        {
            _context.Entradas.Remove(entrada);
            await _context.SaveChangesAsync();
        }
    }
}