using Forestry.Models;

namespace Forestry.DTOs
{
    public class CrearInstanciaIncendioDTO
    {
        public Incendio Incendio { get; set; }
        public BitacoraMedidaInicial BitacoraMedidaInicial { get; set; }
        public Reporte Reporte { get; set; }
        public Actualizacion Actualizacion { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public float Radio { get; set; }
        public int IdReporte { get; set; }
    }
} 