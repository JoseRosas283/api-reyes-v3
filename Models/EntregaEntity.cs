using ReyesAR.Models;

public class EntregaEntity
{
    public string claveEntrega { get; set; }
    public DateTime fecha_entrega { get; set; }
    public string estado { get; set; } // 'Completa' o 'Incompleta'
    public decimal total { get; set; } // Usamos decimal para NUMERIC(10,2) por precisión
    public string descripcion { get; set; }
    public string tipo_entrega { get; set; } // 'EMPRESA' o 'INDEPENDIENTE'

    // Claves Foráneas
    public string clavePedido { get; set; }
    public string claveUsuario { get; set; }
    public string claveProveedor { get; set; }
    public string? claveRepresentante { get; set; } // Puede ser nulo

    // --- PROPIEDADES DE NAVEGACIÓN ---
    public virtual PedidoEntity Pedido { get; set; }
    public virtual UsuarioEntity Usuario { get; set; }
    public virtual ProveedorEntity Proveedor { get; set; }
    public virtual RepresentanteEntity? Representante { get; set; }
}