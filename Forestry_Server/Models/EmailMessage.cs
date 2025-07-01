using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Forestry.Models
{
    public class EmailMessage
    {
        [Required]
        public string To { get; set; }
        
        public string? Cc { get; set; }
        
        public string? Bcc { get; set; }
        
        [Required]
        public string Subject { get; set; }
        
        [Required]
        public string Body { get; set; }
        
        public bool IsHtml { get; set; } = true;
        
        public List<string>? Attachments { get; set; }
        
        public EmailPriority Priority { get; set; } = EmailPriority.Normal;
    }
    
    public enum EmailPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Urgent = 3
    }
} 