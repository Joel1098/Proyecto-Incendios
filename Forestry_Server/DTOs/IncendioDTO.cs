using System;
using System.ComponentModel.DataAnnotations;

namespace Forestry.DTOs
{
    public class IncendioDTO
    {
        public int IdIncendio { get; set; }
        public string NombreDespacho { get; set; }
        public string NombreComando { get; set; }
        public string Ubicacion { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int? IdUsuarioResponsable { get; set; }
        public int IdEtapa { get; set; }
        public System.DateTime FechaIni { get; set; }
        public System.DateTime? FechaFin { get; set; }
        public DateTime FechaCreacion { get; set; }
        
        // Navigation properties for API responses
        public string EtapaNombre { get; set; }
        public string EtapaColor { get; set; }
        public string ResponsableNombre { get; set; }
    }

    public class IncendioCreateDTO
    {
        [Required]
        public DateTime FechaIni { get; set; }
        
        public DateTime? FechaFin { get; set; }
        
        [Required]
        public int IdEtapa { get; set; }
        
        [MaxLength(100)]
        public string NombreDespacho { get; set; }
        
        [MaxLength(100)]
        public string NombreComando { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Ubicacion { get; set; }
        
        public string Descripcion { get; set; }
        
        public int? IdUsuarioResponsable { get; set; }
    }

    public class IncendioUpdateDTO
    {
        public DateTime? FechaFin { get; set; }
        
        [Required]
        public int IdEtapa { get; set; }
        
        [MaxLength(100)]
        public string NombreDespacho { get; set; }
        
        [MaxLength(100)]
        public string NombreComando { get; set; }
        
        [MaxLength(200)]
        public string Ubicacion { get; set; }
        
        public string Descripcion { get; set; }
        
        public int? IdUsuarioResponsable { get; set; }
        
        [MaxLength(20)]
        public string Estado { get; set; }
    }

    public class IncendioFilterDTO
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? IdEtapa { get; set; }
        public string Estado { get; set; }
        public int? IdUsuarioResponsable { get; set; }
        public string Ubicacion { get; set; }
    }

    public class IncendioCreateSimpleDTO
    {
        [Required]
        [MaxLength(200)]
        public string Ubicacion { get; set; }

        public string Descripcion { get; set; }
    }
} 