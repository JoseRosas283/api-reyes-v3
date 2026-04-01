namespace ReyesAR.DTO
{
    public class PedidoDTO
    {
        public string? clavePedido { get; set; }
        public DateTime fecha_pedido { get; set; }
        public string estado { get; set; }
        public string observaciones { get; set; }
        public string claveUsuario { get; set; }
        public string tipo_pedido { get; set; }
        public string claveProveedor { get; set; }
        public decimal total { get; set; }
    }
}
