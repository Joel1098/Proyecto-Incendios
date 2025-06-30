using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forestry.Models
{
    public class Actualizacion
    {
        [Key]
        public int idActualizacion { get; set; }
        public string Accion { get; set; }
        public DateTime FechaAccion { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public float Radio { get; set; }
        public string Tipo { get; set; }
        public int idIncendio { get; set; }
        [ForeignKey("idIncendio")]
        public virtual Incendio Incendio { get; set; }
    }
} 