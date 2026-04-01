using ReyesAR.BasedeDatos;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

public class DetallePedidoRepository : IDetallePedidoRepository
{
    private readonly ReyesARContext _context;

    public DetallePedidoRepository(ReyesARContext context)
    {
        _context = context;
    }

    // 1. Obtener todos los detalles con Eager Loading
    public async Task<IEnumerable<DetallePedidoEntity>> GetAllAsync()
    {
        return await _context.Set<DetallePedidoEntity>()
            .AsNoTracking()
            .ToListAsync();
    }

    // 2. Obtener por llave compuesta (Pedido + Producto)
    public async Task<DetallePedidoEntity?> GetByIdAsync(string clavePedido, string ClaveProducto)
    {
        return await _context.Set<DetallePedidoEntity>()
            .FirstOrDefaultAsync(d => d.Clavepedido == clavePedido && d.Claveproducto == ClaveProducto);
    }

    // 3. Crear usando tu Procedimiento Almacenado "validar_detalle_pedido"
    public async Task<DetallePedidoEntity> CreateAsync(DetallePedidoEntity detalle)
    {
        // Ejecutamos el procedimiento que creamos anteriormente
        // Se pasan los 5 parámetros: Pedido, Producto, Cantidad, Descripción y TipoDetalle
        await _context.Database.ExecuteSqlRawAsync(
            "CALL \"validar_detalle_pedido\"({0}, {1}, {2}, {3}, {4})",
            detalle.Clavepedido,
            detalle.Claveproducto,
            detalle.cantidad,
            detalle.descripcion,
            detalle.tipo_detalle
        );

        // Retornamos la entidad. Nota: Si el SP falla, lanzará una excepción 
        // y no llegará a esta línea, evitando el error de 'null' que tenías antes.
        return detalle;
    }

    // 4. Actualizar usando la terna de llaves
    public async Task UpdateAsync(string Clavepedido, string Claveproducto, DetallePedidoDTO detalleDto)
    {
        var existente = await GetByIdAsync(Clavepedido, Claveproducto);

        if (existente == null)
            throw new Exception($"No se encontró el detalle para el pedido {Clavepedido} y producto {Claveproducto}");

        // Actualizamos los campos permitidos del DTO a la entidad
        existente.cantidad = detalleDto.cantidad;
        existente.descripcion = detalleDto.descripcion;
        // Si el DTO tiene tipo_detalle, lo actualizamos también
        // existente.tipo_detalle = detalleDto.tipo_detalle; 

        _context.Set<DetallePedidoEntity>().Update(existente);
        await _context.SaveChangesAsync();
    }

    // 5. Eliminación física
    public async Task DeleteAsync(string Clavepedido, string Claveproducto)
    {
        var entidad = await GetByIdAsync(Clavepedido, Claveproducto);

        if (entidad != null)
        {
            _context.Set<DetallePedidoEntity>().Remove(entidad);
            await _context.SaveChangesAsync();
        }
    }
}