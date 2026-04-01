using ReyesAR.BasedeDatos;
using ReyesAR.DTO;
using ReyesAR.Models;
using ReyesAR.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

public class PedidoRepository : IPedidoRepository
{
    private readonly ReyesARContext _context;

    public PedidoRepository(ReyesARContext context)
    {
        _context = context;
    }

    // 1. GetAllAsync - Sin Include para evitar ciclos
    public async Task<IEnumerable<PedidoEntity>> GetAllAsync()
    {
        return await _context.Pedidos
            .AsNoTracking()
            .ToListAsync();
    }

    // 2. GetByIdAsync - Sin Include para evitar ciclos
    public async Task<PedidoEntity?> GetByIdAsync(string clavePedido)
    {
        return await _context.Pedidos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.clavePedido == clavePedido);
    }

    // 3. CreateAsync - Ejecuta el SP y recupera el pedido recién creado
    public async Task<PedidoEntity> CreateAsync(PedidoEntity pedido)
    {
        var sql = "CALL \"insertar_pedido\"(@p0, @p1, @p2, @p3, @p4::tipo_pedido_enum, @p5)";
        await _context.Database.ExecuteSqlRawAsync(sql,
            pedido.estado,
            pedido.observaciones,
            pedido.claveUsuario,
            pedido.claveProveedor,
            pedido.tipo_pedido,
            pedido.total
        );

        // ✅ Recupera el pedido recién insertado por el SP
        var pedidoCreado = await _context.Pedidos
            .AsNoTracking()
            .Where(p => p.claveUsuario == pedido.claveUsuario)
            .OrderByDescending(p => p.fecha_pedido)
            .FirstOrDefaultAsync();

        return pedidoCreado ?? pedido;
    }

    // 4. UpdateAsync
    public async Task UpdateAsync(string clavePedido, PedidoDTO pedido)
    {
        var entity = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.clavePedido == clavePedido);

        if (entity != null)
        {
            entity.estado = pedido.estado;
            entity.observaciones = pedido.observaciones;
            entity.total = pedido.total;
            await _context.SaveChangesAsync();
        }
    }

    // 5. DeleteAsync - Eliminación lógica
    public async Task DeleteAsync(string clavePedido)
    {
        var entity = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.clavePedido == clavePedido);

        if (entity != null)
        {
            entity.estado = "Incompleto";
            await _context.SaveChangesAsync();
        }
    }
}