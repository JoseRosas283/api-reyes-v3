using System.Text.Json.Serialization;

namespace ReyesAR.Models
{
    public class ProductoEntity
    {
        public string claveProducto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public decimal margen_ganancia { get; set; } = 1.30m;
        public decimal precio_venta { get; set; }
        public string codigo_barras { get; set; }
        public string tipo_producto { get; set; }
        public decimal cantidad { get; set; }
        public bool caducidad { get; set; }
        public bool estado { get; set; } = true;
        public string Imagen { get; set; }

        // --- LLAVES FORÁNEAS (FK) ---
        public string claveCategoria { get; set; }
        public string claveUnidadMedida { get; set; }
        public string claveUnidadVenta { get; set; }
        public string claveProveedor { get; set; }


        [JsonIgnore]
        public virtual CategoriaEntity Categoria { get; set; }

        [JsonIgnore]
        public virtual UnidadMedidaEntity UnidadMedida { get; set; }

        [JsonIgnore]
        public virtual UnidadVentaEntity UnidadVenta { get; set; }

        [JsonIgnore]
        public virtual ProveedorEntity? Proveedor { get; set; }

        public virtual ICollection<EquivalenciaUnidadEntity> Equivalencias { get; set; } = new List<EquivalenciaUnidadEntity>();
        public virtual ICollection<EquivalenciaUnidadVentaEntity> EquivalenciaProductoVenta { get; set; } = new List<EquivalenciaUnidadVentaEntity>();
        public virtual ICollection<DetallePedidoEntity> Detalles { get; set; } = new List<DetallePedidoEntity>();
        public virtual ICollection<DetalleEntregaEntity> DetalleEntregaProducto { get; set; } = new List<DetalleEntregaEntity>();
        public virtual ICollection<DetalleCompraDirectaEntity> DetalleCompraDirectaProducto { get; set; } = new List<DetalleCompraDirectaEntity>();
        public virtual ICollection<EntradaEntity> Entradas { get; set; } = new List<EntradaEntity>();

    }
}
