namespace ReyesAR.DTO
{
    public class UnidadMedidaDTO
    {
        public string nombreUnidad { get; set; }

        public string abreviatura { get; set; }

        public string tipoStock { get; set; }

        public bool estado { get; set; } = true;
    }


    public class UnidadMedidaUpdateEstadoDto
    {
        public string EstadoTexto { get; set; } 
    }
}
