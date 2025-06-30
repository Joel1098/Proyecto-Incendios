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

        [Required(ErrorMessage = "Incendio obligatorio")]
        [ForeignKey("idIncendio")]
        public int idIncendio { get; set; }

        [Required(ErrorMessage = "Usuario obligatorio")]
        [ForeignKey("idUsuario")]
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "Fecha obligatoria")]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Tipo obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Contenido obligatorio")]
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
        [Display(Name = "Situacion")]
        public string Situacion { get; set; }

        [MaxLength(500)]
        [Display(Name = "Detalles")]
        public string Detalles { get; set; }

        // Navigation properties
        [InverseProperty(nameof(Incendio.Reporte))]
        public virtual Incendio Incendio { get; set; }

        [InverseProperty(nameof(Usuarios.Reporte))]
        public virtual Usuarios Usuario { get; set; }
    }
}
