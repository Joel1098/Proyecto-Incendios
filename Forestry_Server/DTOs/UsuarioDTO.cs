using System;
using System.ComponentModel.DataAnnotations;

namespace Forestry.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Rol { get; set; }
        public string Nombre { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string Estado { get; set; }
        
        [MaxLength(20)]
        public string NumeTel { get; set; }
        
        public TimeSpan? TrabajoInicio { get; set; }
        public TimeSpan? TrabajoFin { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class UsuarioCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string Usuario { get; set; }

        [Required]
        [MaxLength(256)]
        public string Contrasena { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string ApPaterno { get; set; }

        [Required]
        [MaxLength(50)]
        public string ApMaterno { get; set; }
    }

    public class UsuarioUpdateDTO
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string ApPaterno { get; set; }

        [Required]
        [MaxLength(50)]
        public string ApMaterno { get; set; }
    }

    public class LoginDTO
    {
        [Required]
        public string Usuario { get; set; }
        
        [Required]
        public string Contrasena { get; set; }
    }
} 