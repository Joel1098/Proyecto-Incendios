using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forestry.Models
{
    public partial class Etapas
    {
        public Etapas()
        {
            Incendio = new HashSet<Incendio>();
        }

        [Key]
        public int idEtapa { get; set; }

        [Required(ErrorMessage = "Nombre obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Orden obligatorio")]
        [Display(Name = "Orden")]
        public int Orden { get; set; }

        [MaxLength(20)]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        [MaxLength(7)]
        [Display(Name = "Color")]
        public string Color { get; set; } = "#007bff";

        [MaxLength(50)]
        [Display(Name = "Icono")]
        public string Icono { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [InverseProperty(nameof(Incendio.Etapa))]
        public virtual ICollection<Incendio> Incendio { get; set; }
    }
} 