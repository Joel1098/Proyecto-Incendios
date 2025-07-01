using System.Threading.Tasks;
using System.Collections.Generic;
using Forestry.Models;

namespace Forestry.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailMessage emailMessage);
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task<bool> SendIncendioNotificationAsync(int incendioId, string notificationType);
        Task<bool> SendUserRegistrationAsync(string email, string username, string password);
        Task<bool> SendPasswordResetAsync(string email, string resetToken);
        Task<bool> SendReportNotificationAsync(int reportId, string reportType);
        Task<bool> SendEmergencyAlertAsync(string location, string description, List<string> recipients);
    }
} 