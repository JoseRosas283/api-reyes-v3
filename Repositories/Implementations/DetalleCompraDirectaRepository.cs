using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

public class DetalleCompraDirectaRepository : IDetalleCompraDirectaRepository
{
    private readonly ReyesARContext _context;

    public DetalleCompraDirectaRepository(ReyesARContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DetalleCompraDirectaEntity>> GetAllAsync()
    {
        return await _context.DetalleCompraDirecta
            .Include(d => d.Producto)
            .Include(d => d.EquivalenciaUnidad)
            .ToListAsync();
    }

    public async Task<IEnumerable<DetalleCompraDirectaEntity>> GetByCompraAsync(string claveCompra)
    {
        return await _context.DetalleCompraDirecta
            .Include(d => d.Producto)
            .Where(d => d.claveCompra == claveCompra.Trim())
            .ToListAsync();
    }

    public async Task<DetalleCompraDirectaEntity?> GetByIdAsync(string claveCompra, string claveProducto)
    {
        return await _context.DetalleCompraDirecta
            .FirstOrDefaultAsync(d => d.claveCompra == claveCompra.Trim() &&
                                     d.claveProducto == claveProducto.Trim());
    }

    public async Task<DetalleCompraDirectaEntity> CreateAsync(DetalleCompraDirectaEntity detalle)
    {
        // Mapeo de parámetros conforme a la firma del procedimiento
        var p_clavecompra = new NpgsqlParameter("p_clavecompra", detalle.claveCompra.Trim());
        var p_claveproducto = new NpgsqlParameter("p_claveproducto", detalle.claveProducto.Trim());
        var p_cantidad = new NpgsqlParameter("p_cantidad", detalle.cantidad);
        var p_preciounitario = new NpgsqlParameter("p_preciounitario", (double)detalle.precioUnitario);
        var p_impuestos = new NpgsqlParameter("p_impuestos", detalle.impuestos ?? (object)DBNull.Value);
        var p_claveunidadcompra = new NpgsqlParameter("p_claveunidadcompra", detalle.claveUnidadCompra.Trim());

        // Ejecución del procedimiento con comillas dobles
        await _context.Database.ExecuteSqlRawAsync(
            "CALL \"insertar_detalle_compra_directa\"(@p_clavecompra, @p_claveproducto, @p_cantidad, @p_preciounitario, @p_impuestos, @p_claveunidadcompra)",
            p_clavecompra, p_claveproducto, p_cantidad, p_preciounitario, p_impuestos, p_claveunidadcompra
        );

        return detalle;
    }

    public async Task UpdateAsync(string claveCompra, string claveProducto, DetalleCompraDirectaEntity detalle)
    {
        var registro = await GetByIdAsync(claveCompra, claveProducto);

        if (registro != null)
        {
            registro.cantidad = detalle.cantidad;
            registro.precioUnitario = detalle.precioUnitario;
            registro.impuestos = detalle.impuestos;
            // El subtotal no se asigna manualmente, lo recalcula la DB (STORED)

            _context.DetalleCompraDirecta.Update(registro);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(string claveCompra, string claveProducto)
    {
        var registro = await GetByIdAsync(claveCompra, claveProducto);

        if (registro != null)
        {
            _context.DetalleCompraDirecta.Remove(registro);
            await _context.SaveChangesAsync();
        }
    }
}