using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forestry.Models
{
    public partial class IncendioPersonal
    {
        [Key]
        [Column(Order = 0)]
        public int idIncendio { get; set; }

        [Key]
        [Column(Order = 1)]
        public int IdTrabajador { get; set; }

        [Display(Name = "Fecha de Asignación")]
        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        [Display(Name = "Rol en el Incendio")]
        public string RolEnIncendio { get; set; }

        [MaxLength(20)]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        // Navigation properties
        [ForeignKey("idIncendio")]
        public virtual Incendio Incendio { get; set; }

        [ForeignKey("IdTrabajador")]
        public virtual Personal Trabajador { get; set; }
    }
} 