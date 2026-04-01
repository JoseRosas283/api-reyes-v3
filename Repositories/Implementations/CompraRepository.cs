using Npgsql;
using ReyesAR.BasedeDatos;
using ReyesAR.DTO;
using ReyesAR.Models;
using Microsoft.EntityFrameworkCore;

public class CompraRepository : ICompraRepository
{
    private readonly ReyesARContext _context;

    public CompraRepository(ReyesARContext context)
    {
        _context = context;
    }

    // 1. Listado mapeado a DTO
    public async Task<IEnumerable<CompraDTO>> GetAllAsync()
    {
        var entities = await _context.Compras
            .Include(c => c.CompraPedido)
            .Include(c => c.CompraDirecta)
            .ToListAsync();

        return entities.Select(MapToDTO);
    }

    // 2. Detalle único mapeado a DTO
    public async Task<CompraDTO> GetByIdAsync(string claveCompra)
    {
        var entity = await _context.Compras
            .Include(c => c.CompraPedido)
            .Include(c => c.CompraDirecta)
            .FirstOrDefaultAsync(c => c.ClaveCompra == claveCompra);

        return entity != null ? MapToDTO(entity) : null!;
    }

    // 3. Crear Compra (Uso de SP registrar_compra con Blindaje)
    public async Task<CompraDTO> CreateAsync(CompraDTO compra)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Mapeo riguroso de p0 a p11 según tu SP y DTO
            var p0 = new NpgsqlParameter("p0", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)compra.tipo_compra ?? DBNull.Value };
            var p1 = new NpgsqlParameter("p1", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = compra.total };
            var p2 = new NpgsqlParameter("p2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)compra.forma_pago ?? DBNull.Value };
            var p3 = new NpgsqlParameter("p3", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)compra.ClaveUsuario ?? DBNull.Value };
            var p4 = new NpgsqlParameter("p4", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)compra.folio ?? DBNull.Value };

            // Igualamos facturaImagenUrl a p_factura_proveedor
            var p5 = new NpgsqlParameter("p5", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)compra.facturaImagenUrl ?? DBNull.Value };

            var p6 = new NpgsqlParameter("p6", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)compra.referencia_pago ?? DBNull.Value };

            // Igualamos documentoImagenUrl a p_documento_referencia
            var p7 = new NpgsqlParameter("p7", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)compra.documentoImagenUrl ?? DBNull.Value };

            var p8 = new NpgsqlParameter("p8", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)compra.origen ?? DBNull.Value };
            var p9 = new NpgsqlParameter("p9", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)compra.claveEntrega ?? DBNull.Value };
            var p10 = new NpgsqlParameter("p10", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)compra.estado_cumplimiento ?? DBNull.Value };
            var p11 = new NpgsqlParameter("p11", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)compra.estado_operativo ?? DBNull.Value };

            _context.ChangeTracker.Clear();

            // Ejecución del SP con esquema public y comillas dobles
            await _context.Database.ExecuteSqlRawAsync(
                @"CALL public.""registrar_compra""(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11)",
                p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11
            );

            // Recuperamos la entidad recién creada para retornar el DTO con su ClaveCompra real
            var entityGenerada = await _context.Compras
                .Include(c => c.CompraPedido)
                .Include(c => c.CompraDirecta)
                .Where(c => c.ClaveUsuario == compra.ClaveUsuario)
                .OrderByDescending(c => c.fecha_compra)
                .FirstOrDefaultAsync();

            await transaction.CommitAsync();
            return MapToDTO(entityGenerada!);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Error en registrar_compra: {ex.Message}", ex);
        }
    }

    // 4. Actualizar Estado (Apegado al contrato Task<CompraDTO>)
    public async Task<CompraDTO> UpdateEstadoAsync(string claveCompra, string nuevoEstado)
    {
        var entity = await _context.Compras
            .Include(c => c.CompraPedido)
            .Include(c => c.CompraDirecta)
            .FirstOrDefaultAsync(c => c.ClaveCompra == claveCompra);

        if (entity == null) throw new Exception("Compra no encontrada");

        if (entity.tipo_compra == "Pedido" && entity.CompraPedido != null)
            entity.CompraPedido.estado_operativo = nuevoEstado;
        else if (entity.tipo_compra == "Directa" && entity.CompraDirecta != null)
            entity.CompraDirecta.estado = nuevoEstado;

        await _context.SaveChangesAsync();
        return MapToDTO(entity);
    }

    // 5. Cancelar Compra (Apegado al contrato Task<CompraDTO>)
    public async Task<CompraDTO> CancelarAsync(string claveCompra)
    {
        return await UpdateEstadoAsync(claveCompra, "Cancelada");
    }

    // --- MAPEO MANUAL RESPETANDO CAMPOS DEL DTO ---
    private static CompraDTO MapToDTO(CompraEntity e)
    {
        if (e == null) return null!;

        return new CompraDTO
        {
            // Campos Tabla Padre
            tipo_compra = e.tipo_compra,
            total = e.total,
            forma_pago = e.forma_pago,
            ClaveUsuario = e.ClaveUsuario,

            // Campos CompraPedido
            folio = e.CompraPedido?.folio,
            facturaImagenUrl = e.CompraPedido?.factura_proveedor,   // URL para el DTO
            claveEntrega = e.CompraPedido?.claveEntrega,
            estado_cumplimiento = e.CompraPedido?.estado_cumplimiento,
            estado_operativo = e.CompraPedido?.estado_operativo,

            // Campos CompraDirecta
            referencia_pago = e.CompraDirecta?.referencia_pago,
            documentoImagenUrl = e.CompraDirecta?.documento_referencia,   // URL para el DTO
            origen = e.CompraDirecta?.origen,
            estado_directa = e.CompraDirecta?.estado
        };
    }
}