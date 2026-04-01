namespace ReyesAR.Models
{
    public class DetallePedidoEntity
    {
        public string Clavepedido { get; set; }
        public string Claveproducto { get; set; }
        public decimal cantidad { get; set; }
        public string descripcion { get; set; }
        public string tipo_detalle { get; set; }
        public PedidoEntity Pedido { get; set; }
        public ProductoEntity Producto { get; set; }
    }
}
