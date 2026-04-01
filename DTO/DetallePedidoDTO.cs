namespace ReyesAR.DTO
{
    public class DetallePedidoDTO
    {
        public string Clavepedido { get; set; }
        public string Claveproducto { get; set; }
        public decimal cantidad { get; set; }
        public string descripcion { get; set; }
        public string tipo_detalle { get; set; }
    }
}
