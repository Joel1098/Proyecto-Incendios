namespace Forestry.DTOs
{
    public class RegisterRequest
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public string Nombre { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string Rol { get; set; }
        public string NumeTel { get; set; }
        public string DiasLaborales { get; set; }
        public string Estado { get; set; } = "Activo";
    }
} 