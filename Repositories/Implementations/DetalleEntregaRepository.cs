using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

public class DetalleEntregaRepository : IDetalleEntregaRepository
{
    private readonly ReyesARContext _context;

    public DetalleEntregaRepository(ReyesARContext context)
    {
        _context = context;
    }

    // 1. Obtener listado de la tabla física (Entidad)
    public async Task<IEnumerable<DetalleEntregaEntity>> GetAllAsync()
    {
        return await _context.DetalleEntrega.ToListAsync();
    }

    // 2. Obtener listado DETALLADO (Filtrado por Entrega)
    public async Task<IEnumerable<DetalleEntregaEntity>> GetDetailedListAsync(string claveEntrega)
    {
        return await _context.DetalleEntrega
            .Include(d => d.EquivalenciaUnidad)
            .Where(d => d.claveEntrega == claveEntrega)
            .ToListAsync();
    }

    // 3. Buscar detalle por su Llave Primaria Compuesta (4 campos)
    public async Task<DetalleEntregaEntity?> GetByIdAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra)
    {
        return await _context.DetalleEntrega
            .FirstOrDefaultAsync(d => d.claveEntrega == claveEntrega &&
                                     d.claveProducto == claveProducto &&
                                     d.tipo_detalle == tipo_detalle &&
                                     d.extra == extra);
    }

    // 4. Crear detalle enviando la Entidad (Ejecuta SP)
    public async Task<DetalleEntregaEntity> CreateAsync(DetalleEntregaEntity detalle)
    {
        var p_claveentrega = new NpgsqlParameter("p_claveentrega", detalle.claveEntrega);
        var p_claveproducto = new NpgsqlParameter("p_claveproducto", detalle.claveProducto);
        var p_cantidad = new NpgsqlParameter("p_cantidad", detalle.cantidad);
        var p_preciounitario = new NpgsqlParameter("p_preciounitario", (double)detalle.precioUnitario);
        var p_observaciones = new NpgsqlParameter("p_observaciones", detalle.observaciones ?? (object)DBNull.Value);
        var p_clavecompra = new NpgsqlParameter("p_clavecompra", detalle.claveCompra);
        var p_claveunidadcompra = new NpgsqlParameter("p_claveunidadcompra", detalle.claveUnidadCompra);
        var p_tipo_detalle = new NpgsqlParameter("p_tipo_detalle", detalle.tipo_detalle);

        // Llamada al procedimiento respetando comillas para PostgreSQL
        await _context.Database.ExecuteSqlRawAsync(
            "CALL \"insertar_detalle_entrega\"(@p_claveentrega, @p_claveproducto, @p_cantidad, @p_preciounitario, @p_observaciones, @p_clavecompra, @p_claveunidadcompra, @p_tipo_detalle)",
            p_claveentrega, p_claveproducto, p_cantidad, p_preciounitario, p_observaciones, p_clavecompra, p_claveunidadcompra, p_tipo_detalle
        );

        return detalle;
    }

    // 5. Actualizar datos del detalle
    public async Task UpdateAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra, DetalleEntregaEntity detalle)
    {
        var registro = await GetByIdAsync(claveEntrega, claveProducto, tipo_detalle, extra);

        if (registro != null)
        {
            registro.observaciones = detalle.observaciones;
            registro.cantidad = detalle.cantidad;
            registro.precioUnitario = detalle.precioUnitario;
            // El subtotal se recalcula para mantener consistencia en la entidad
            registro.subtotal = detalle.cantidad * detalle.precioUnitario;

            _context.DetalleEntrega.Update(registro);
            await _context.SaveChangesAsync();
        }
    }

    // 6. Eliminación de un registro específico
    public async Task DeleteAsync(string claveEntrega, string claveProducto, string tipo_detalle, bool extra)
    {
        var registro = await GetByIdAsync(claveEntrega, claveProducto, tipo_detalle, extra);

        if (registro != null)
        {
            _context.DetalleEntrega.Remove(registro);
            await _context.SaveChangesAsync();
        }
    }
}