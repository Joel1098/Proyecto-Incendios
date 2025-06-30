using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forestry.Models
{
    public class BitacoraMedidaInicial
    {
        [Key]
        public int IdBitacora { get; set; }

        [Required]
        public DateTime FechaCreada { get; set; }

        [Required]
        public string Pregunta1 { get; set; }
        [Required]
        public string Pregunta2 { get; set; }
        [Required]
        public string Peligros { get; set; }
        [Required]
        public string PotencialExpansion { get; set; }
        [Required]
        public string CaracterFuego { get; set; }
        [Required]
        public string PendienteFuego { get; set; }
        [Required]
        public string PosicionPendiente { get; set; }
        [Required]
        public string TipoCombustible { get; set; }
        [Required]
        public string CombustiblesAdyacentes { get; set; }
        [Required]
        public string Aspecto { get; set; }
        [Required]
        public string DireccionViento { get; set; }

        public int? IdIncendio { get; set; }
        [ForeignKey("IdIncendio")]
        public virtual Incendio Incendio { get; set; }
    }
} 