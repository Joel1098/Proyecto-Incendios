namespace Forestry.DTOs
{
    public class ReporteDTO
    {
        public int IdReporte { get; set; }
        public int IdIncendio { get; set; }
        public int IdUsuario { get; set; }
        public string Lugar { get; set; }
        public string Situacion { get; set; }
        public string Detalles { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        public string Contenido { get; set; }
        public System.DateTime Fecha { get; set; }
    }
} 