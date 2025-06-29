using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Forestry.Models
{
    public partial class Usuarios
    {
        public Usuarios()
        {
            Reporte = new HashSet<Reporte>();
            IncendiosResponsable = new HashSet<Incendio>();
        }

        [Required(ErrorMessage = "IdUsuarios obligatorio")]
        [Key]
        [Display(Name = "IdUsuario")]
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "Rol obligatorio")]
        [MaxLength(20)]
        [Display(Name = "Rol")]
        public string Rol { get; set; }

        [Required(ErrorMessage = "Estado obligatorio")]
        [MaxLength(20)]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

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
        [Display(Name = "Número de Teléfono")]
        public string NumeTel { get; set; }

        [Required(ErrorMessage = "Usuario obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Contraseña obligatoria")]
        [MaxLength(256)]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }

        [Display(Name = "Inicio de Trabajo")]
        public TimeSpan? TrabajoInicio { get; set; }

        [Display(Name = "Fin de Trabajo")]
        public TimeSpan? TrabajoFin { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Navigation properties
        [InverseProperty(nameof(Reporte.Usuario))]
        public virtual ICollection<Reporte> Reporte { get; set; }

        [InverseProperty(nameof(Incendio.UsuarioResponsable))]
        public virtual ICollection<Incendio> IncendiosResponsable { get; set; }
    }
}
