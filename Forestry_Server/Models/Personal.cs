using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Forestry.Models
{
    public partial class Personal
    {
        [Key]
        public int IdTrabajador { get; set; }

        [Required(ErrorMessage = "Nombre obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Apellido Paterno obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Apellido Paterno")]
        public string ApPaterno { get; set; }

        [Required(ErrorMessage = "Apellido Materno obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Apellido Materno")]
        public string ApMaterno { get; set; }

        [MaxLength(20)]
        [Display(Name = "Turno")]
        public string Turno { get; set; }

        [MaxLength(100)]
        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; }

        [MaxLength(20)]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreada { get; set; } = DateTime.Now;

        // Navigation property - This will be handled through the junction table
        // The actual relationship is many-to-many through IncendioPersonal
        public virtual ICollection<IncendioPersonal> IncendioPersonal { get; set; }
    }
}
