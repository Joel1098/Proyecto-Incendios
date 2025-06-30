using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forestry.Models
{
    public partial class Incendio
    {
        public Incendio()
        {
            Reporte = new HashSet<Reporte>();
        }

        [Key]
        public int idIncendio { get; set; }

        [Required(ErrorMessage = "Fecha de inicio obligatoria")]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaIni { get; set; }

        [Display(Name = "Fecha de Fin")]
        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "Etapa obligatoria")]
        [ForeignKey("idEtapa")]
        public int idEtapa { get; set; }

        [MaxLength(100)]
        [Display(Name = "Nombre del Despacho")]
        public string NombreDespacho { get; set; }

        [MaxLength(100)]
        [Display(Name = "Nombre del Comando")]
        public string NombreComando { get; set; }

        [MaxLength(200)]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [MaxLength(20)]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        [ForeignKey("idUsuarioResponsable")]
        public int? idUsuarioResponsable { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [InverseProperty(nameof(Etapas.Incendio))]
        public virtual Etapas Etapa { get; set; }

        [InverseProperty(nameof(Usuarios.IncendiosResponsable))]
        public virtual Usuarios UsuarioResponsable { get; set; }

        public virtual ICollection<IncendioPersonal> IncendioPersonal { get; set; }
        public virtual ICollection<Reporte> Reporte { get; set; }
    }
}
