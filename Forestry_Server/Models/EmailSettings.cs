using System.ComponentModel.DataAnnotations;

namespace Forestry.Models
{
    public class EmailSettings
    {
        [Required]
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        
        [Required]
        public int SmtpPort { get; set; } = 587;
        
        [Required]
        public string SmtpUsername { get; set; }
        
        [Required]
        public string SmtpPassword { get; set; }
        
        public bool EnableSsl { get; set; } = true;
        
        [Required]
        public string FromEmail { get; set; }
        
        [Required]
        public string FromName { get; set; } = "Forestry System";
        
        public bool EnableEmailNotifications { get; set; } = true;
    }
} 