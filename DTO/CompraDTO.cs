namespace ReyesAR.DTO
{
    public class CompraDTO
    {
        // --- Campos de la tabla PADRE (Compras) ---
        // ClaveCompra no se incluye porque se genera con DEFAULT generar_clave_compra()
        public string? ClaveCompra { get; set; }
        public string tipo_compra { get; set; } // 'Pedido' o 'Directa'
        public decimal total { get; set; }
        public string? forma_pago { get; set; }
        public string? ClaveUsuario { get; set; }

        // --- Campos de la subentidad COMPRAPEDIDO ---
        public string? folio { get; set; } // Independiente null
        public string? claveEntrega { get; set; } // Requerido si es 'Pedido'
        public string? estado_cumplimiento { get; set; } // 'Completo', 'Incompleto'
        public string? estado_operativo { get; set; } // 'Abierta', 'Cerrada'
        public IFormFile? facturaImagen { get; set; } // Independiente null
        public string? facturaImagenUrl { get; set; }

        // --- Campos de la subentidad COMPRADIRECTA ---
        public string? referencia_pago { get; set; }
        public string? origen { get; set; } // 'Mercado', 'Centro Comercial'
        public string? estado_directa { get; set; } // 'Abierta', 'Cerrada'
        public IFormFile? documentoImagen { get; set; }
        public string? documentoImagenUrl { get; set; }
    }
}
