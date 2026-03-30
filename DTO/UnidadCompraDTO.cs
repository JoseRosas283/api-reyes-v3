namespace ReyesAR.DTO
{
    public class UnidadCompraDTO
    {
        public string nombre_unidad { get; set; }

        public string abreviatura { get; set; }

        public string tipo_stock { get; set; }

        public bool estado { get; set; } = true;
    }


    public class UnidadCompraUpdateEstadoDto
    {
        public string EstadoTexto { get; set; }
    }
}
