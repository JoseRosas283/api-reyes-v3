using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

public class EntregaRepository : IEntregaRepository
{
    private readonly ReyesARContext _context;

    public EntregaRepository(ReyesARContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EntregaEntity>> GetAllAsync()
    {
        return await _context.Entregas
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<EntregaEntity?> GetByClaveAsync(string claveEntrega)
    {
        return await _context.Entregas
            .FirstOrDefaultAsync(e => e.claveEntrega == claveEntrega);
    }

    public async Task<EntregaEntity?> AddAsync(EntregaEntity entrega)
    {
        var sql = @"CALL ""insertar_entrega""(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7)";

        var parameters = new[]
        {
                new NpgsqlParameter("@p0", entrega.estado),
                new NpgsqlParameter("@p1", entrega.total),
                new NpgsqlParameter("@p2", entrega.descripcion),
                new NpgsqlParameter("@p3", entrega.tipo_entrega),
                new NpgsqlParameter("@p4", entrega.clavePedido),
                new NpgsqlParameter("@p5", entrega.claveUsuario),
                new NpgsqlParameter("@p6", entrega.claveProveedor),
                new NpgsqlParameter("@p7", (object?)entrega.claveRepresentante ?? DBNull.Value)
            };

        await _context.Database.ExecuteSqlRawAsync(sql, parameters);

        // Al ser 1:1, recuperamos la entrega recién creada por el ID del pedido
        return await _context.Entregas
            .OrderByDescending(e => e.fecha_entrega)
            .FirstOrDefaultAsync(e => e.clavePedido == entrega.clavePedido);
    }

    public async Task<EntregaEntity?> UpdateAsync(EntregaEntity entrega)
    {
        _context.Entregas.Update(entrega);
        await _context.SaveChangesAsync();
        return entrega;
    }

    public async Task<EntregaEntity?> DeleteAsync(string claveEntrega)
    {
        var entrega = await GetByClaveAsync(claveEntrega);
        if (entrega != null)
        {
            _context.Entregas.Remove(entrega);
            await _context.SaveChangesAsync();
        }
        return entrega;
    }
}