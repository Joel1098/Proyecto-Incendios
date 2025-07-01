using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forestry.Models
{
    public partial class Reporte
    {
        [Required(ErrorMessage = "IdReporte obligatorio")]
        [Key]
        [Display(Name = "IdReporte")]
        public int idReporte { get; set; }

        [Display(Name = "Incendio")]
        public int? idIncendio { get; set; }

        [Display(Name = "Usuario")]
        public int? idUsuario { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [Required]
        [Display(Name = "Contenido")]
        public string Contenido { get; set; }

        [MaxLength(20)]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [MaxLength(200)]
        [Display(Name = "Lugar")]
        public string Lugar { get; set; }

        [MaxLength(200)]
        [Display(Name = "Situación")]
        public string Situacion { get; set; }

        [MaxLength(500)]
        [Display(Name = "Detalles")]
        public string Detalles { get; set; }

        // Navigation properties
        [ForeignKey("idIncendio")]
        public virtual Incendio Incendio { get; set; }

        [ForeignKey("idUsuario")]
        public virtual Usuarios Usuario { get; set; }
    }
}
