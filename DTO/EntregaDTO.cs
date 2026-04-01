namespace ReyesAR.DTO
{
    public class EntregaDTO
    {
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
    }
}
