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

    // 1. Task<IEnumerable<PedidoEntity>> GetAllAsync()
    public async Task<IEnumerable<PedidoEntity>> GetAllAsync()
    {
        return await _context.Pedidos
            .Include(p => p.Usuario)
            .Include(p => p.Proveedor)
            .AsNoTracking()
            .ToListAsync();
    }

    // 2. Task<PedidoEntity?> GetByIdAsync(string clavePedido)
    public async Task<PedidoEntity?> GetByIdAsync(string clavePedido)
    {
        return await _context.Pedidos
            .Include(p => p.Usuario)
            .Include(p => p.Proveedor)
            .FirstOrDefaultAsync(p => p.clavePedido == clavePedido);
    }

    // 3. Task<PedidoEntity> CreateAsync(PedidoEntity pedido)
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

        return pedido;
    }

    // 4. Task UpdateAsync(string clavePedido, PedidoDTO pedido) 
    // ¡OJO! Aquí estaba el error. Ahora recibe PedidoDTO como dice tu contrato.
    public async Task UpdateAsync(string clavePedido, PedidoDTO pedido)
    {
        var entity = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.clavePedido == clavePedido);

        if (entity != null)
        {
            // Mapeamos los datos del DTO a la Entidad de ReyesAR
            entity.estado = pedido.estado;
            entity.observaciones = pedido.observaciones;
            entity.total = pedido.total;

            await _context.SaveChangesAsync();
        }
    }

    // 5. Task DeleteAsync(string clavePedido)
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