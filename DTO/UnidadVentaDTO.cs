namespace ReyesAR.DTO
{
    public class UnidadVentaDTO
    {
        public string nombreUnidad { get; set; }
        public string abreviatura { get; set; }
        public string tipoValor { get; set; }
        public bool estado { get; set; } = true;
    }

    public class UnidadVentaUpdateEstadoDto
    {
        public string EstadoTexto { get; set; }
    }
}
