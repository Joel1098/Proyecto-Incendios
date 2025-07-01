namespace Forestry.DTOs
{
    public class CrearReporteDTO
    {
        public string Tipo { get; set; }
        public string Contenido { get; set; }
        public string Lugar { get; set; }
        public string Situacion { get; set; }
        public string Detalles { get; set; }
        public int? idIncendio { get; set; }
        public int? idUsuario { get; set; }
    }
} 