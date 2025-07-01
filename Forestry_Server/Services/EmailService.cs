using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Forestry.Models;

namespace Forestry.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;
        private readonly ContextoBaseDeDatos _context;

        public EmailService(IOptions<EmailSettings> emailSettings, 
                          ILogger<EmailService> logger,
                          ContextoBaseDeDatos context)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
            _context = context;
        }

        public async Task<bool> SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                if (!_emailSettings.EnableEmailNotifications)
                {
                    _logger.LogInformation("Email notifications are disabled");
                    return true;
                }

                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    EnableSsl = _emailSettings.EnableSsl,
                    Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword)
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                    Subject = emailMessage.Subject,
                    Body = emailMessage.Body,
                    IsBodyHtml = emailMessage.IsHtml,
                    Priority = ConvertToMailPriority(emailMessage.Priority)
                };

                // Agregar destinatarios
                message.To.Add(emailMessage.To);
                
                if (!string.IsNullOrEmpty(emailMessage.Cc))
                    message.CC.Add(emailMessage.Cc);
                
                if (!string.IsNullOrEmpty(emailMessage.Bcc))
                    message.Bcc.Add(emailMessage.Bcc);

                // Agregar adjuntos si existen
                if (emailMessage.Attachments != null)
                {
                    foreach (var attachment in emailMessage.Attachments)
                    {
                        if (System.IO.File.Exists(attachment))
                        {
                            message.Attachments.Add(new Attachment(attachment));
                        }
                    }
                }

                await client.SendMailAsync(message);
                
                _logger.LogInformation($"Email sent successfully to {emailMessage.To}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {emailMessage.To}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            var emailMessage = new EmailMessage
            {
                To = to,
                Subject = subject,
                Body = body,
                IsHtml = isHtml
            };

            return await SendEmailAsync(emailMessage);
        }

        public async Task<bool> SendIncendioNotificationAsync(int incendioId, string notificationType)
        {
            try
            {
                var incendio = await _context.Incendio
                    .Include(i => i.UsuarioResponsable)
                    .Include(i => i.Etapa)
                    .FirstOrDefaultAsync(i => i.idIncendio == incendioId);

                if (incendio == null)
                {
                    _logger.LogWarning($"Incendio {incendioId} not found for email notification");
                    return false;
                }

                var subject = GetIncendioSubject(notificationType, incendio);
                var body = await GenerateIncendioEmailBody(incendio, notificationType);

                // Obtener destinatarios según el tipo de notificación
                var recipients = await GetIncendioRecipients(incendio, notificationType);

                var success = true;
                foreach (var recipient in recipients)
                {
                    if (!await SendEmailAsync(recipient, subject, body))
                    {
                        success = false;
                    }
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending incendio notification for {incendioId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendUserRegistrationAsync(string email, string username, string password)
        {
            var subject = "Registro Exitoso - Forestry System";
            var body = GenerateRegistrationEmailBody(username, password);

            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendPasswordResetAsync(string email, string resetToken)
        {
            var subject = "Restablecimiento de Contraseña - Forestry System";
            var body = GeneratePasswordResetEmailBody(resetToken);

            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendReportNotificationAsync(int reportId, string reportType)
        {
            try
            {
                var reporte = await _context.Reporte
                    .Include(r => r.Incendio)
                    .FirstOrDefaultAsync(r => r.idReporte == reportId);

                if (reporte == null)
                {
                    _logger.LogWarning($"Reporte {reportId} not found for email notification");
                    return false;
                }

                var subject = $"Nuevo Reporte - {reportType}";
                var body = await GenerateReportEmailBody(reporte, reportType);

                // Obtener destinatarios para reportes
                var recipients = await GetReportRecipients(reporte, reportType);

                var success = true;
                foreach (var recipient in recipients)
                {
                    if (!await SendEmailAsync(recipient, subject, body))
                    {
                        success = false;
                    }
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending report notification for {reportId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendEmergencyAlertAsync(string location, string description, List<string> recipients)
        {
            var subject = "🚨 ALERTA DE EMERGENCIA - Forestry System";
            var body = GenerateEmergencyAlertBody(location, description);

            var success = true;
            foreach (var recipient in recipients)
            {
                if (!await SendEmailAsync(recipient, subject, body))
                {
                    success = false;
                }
            }

            return success;
        }

        #region Private Methods

        private MailPriority ConvertToMailPriority(EmailPriority priority)
        {
            return priority switch
            {
                EmailPriority.Low => MailPriority.Low,
                EmailPriority.Normal => MailPriority.Normal,
                EmailPriority.High => MailPriority.High,
                EmailPriority.Urgent => MailPriority.High,
                _ => MailPriority.Normal
            };
        }

        private string GetIncendioSubject(string notificationType, Incendio incendio)
        {
            return notificationType.ToLower() switch
            {
                "nuevo" => $"🔥 Nuevo Incendio Reportado - {incendio.Ubicacion}",
                "actualizado" => $"📝 Incendio Actualizado - {incendio.Ubicacion}",
                "cerrado" => $"✅ Incendio Cerrado - {incendio.Ubicacion}",
                "emergencia" => $"🚨 EMERGENCIA - Incendio Crítico - {incendio.Ubicacion}",
                _ => $"📋 Notificación de Incendio - {incendio.Ubicacion}"
            };
        }

        private async Task<string> GenerateIncendioEmailBody(Incendio incendio, string notificationType)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("<html><body>");
            sb.AppendLine("<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>");
            
            // Header según tipo de notificación
            var headerColor = notificationType.ToLower() switch
            {
                "nuevo" => "#ff6b35",
                "actualizado" => "#4ecdc4",
                "cerrado" => "#45b7d1",
                "emergencia" => "#ff0000",
                _ => "#95a5a6"
            };

            sb.AppendLine($"<div style='background-color: {headerColor}; color: white; padding: 20px; text-align: center;'>");
            sb.AppendLine($"<h1>{GetIncendioSubject(notificationType, incendio)}</h1>");
            sb.AppendLine("</div>");

            // Contenido
            sb.AppendLine("<div style='padding: 20px; background-color: #f8f9fa;'>");
            sb.AppendLine($"<h2>Detalles del Incendio</h2>");
            sb.AppendLine($"<p><strong>Ubicación:</strong> {incendio.Ubicacion}</p>");
            sb.AppendLine($"<p><strong>Descripción:</strong> {incendio.Descripcion}</p>");
            sb.AppendLine($"<p><strong>Estado:</strong> {incendio.Estado}</p>");
            sb.AppendLine($"<p><strong>Etapa:</strong> {incendio.Etapa?.Nombre}</p>");
            sb.AppendLine($"<p><strong>Fecha de Inicio:</strong> {incendio.FechaIni:dd/MM/yyyy HH:mm}</p>");
            
            if (incendio.FechaFin.HasValue)
            {
                sb.AppendLine($"<p><strong>Fecha de Fin:</strong> {incendio.FechaFin.Value:dd/MM/yyyy HH:mm}</p>");
            }

            if (incendio.UsuarioResponsable != null)
            {
                sb.AppendLine($"<p><strong>Responsable:</strong> {incendio.UsuarioResponsable.Nombre} {incendio.UsuarioResponsable.ApPaterno}</p>");
            }

            sb.AppendLine("</div>");

            // Footer
            sb.AppendLine("<div style='background-color: #34495e; color: white; padding: 15px; text-align: center;'>");
            sb.AppendLine("<p>Forestry System - Sistema de Gestión de Incendios Forestales</p>");
            sb.AppendLine("<p>Este es un mensaje automático, no responda a este correo.</p>");
            sb.AppendLine("</div>");

            sb.AppendLine("</div></body></html>");

            return sb.ToString();
        }

        private async Task<List<string>> GetIncendioRecipients(Incendio incendio, string notificationType)
        {
            var recipients = new List<string>();

            // Obtener usuarios según rol y tipo de notificación
            var users = await _context.Usuarios
                .Where(u => u.Estado == "Activo")
                .ToListAsync();

            foreach (var user in users)
            {
                var shouldNotify = notificationType.ToLower() switch
                {
                    "nuevo" => user.Rol == "Administrador" || user.Rol == "Despacho" || user.Rol == "Comando",
                    "actualizado" => user.Rol == "Administrador" || user.Rol == "Comando" || 
                                   (user.Rol == "Personal" && user.idUsuario == incendio.idUsuarioResponsable),
                    "cerrado" => user.Rol == "Administrador" || user.Rol == "Despacho" || user.Rol == "Comando",
                    "emergencia" => user.Rol == "Administrador" || user.Rol == "Despacho" || user.Rol == "Comando" || user.Rol == "Personal",
                    _ => user.Rol == "Administrador"
                };

                if (shouldNotify && !string.IsNullOrEmpty(user.NumeTel))
                {
                    // Asumiendo que NumeTel contiene el email
                    recipients.Add(user.NumeTel);
                }
            }

            return recipients.Distinct().ToList();
        }

        private string GenerateRegistrationEmailBody(string username, string password)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("<html><body>");
            sb.AppendLine("<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>");
            
            sb.AppendLine("<div style='background-color: #2ecc71; color: white; padding: 20px; text-align: center;'>");
            sb.AppendLine("<h1>✅ Registro Exitoso</h1>");
            sb.AppendLine("</div>");

            sb.AppendLine("<div style='padding: 20px; background-color: #f8f9fa;'>");
            sb.AppendLine("<h2>Bienvenido al Sistema Forestry</h2>");
            sb.AppendLine($"<p>Su cuenta ha sido creada exitosamente con las siguientes credenciales:</p>");
            sb.AppendLine($"<p><strong>Usuario:</strong> {username}</p>");
            sb.AppendLine($"<p><strong>Contraseña:</strong> {password}</p>");
            sb.AppendLine("<p><strong>IMPORTANTE:</strong> Por seguridad, cambie su contraseña después del primer inicio de sesión.</p>");
            sb.AppendLine("</div>");

            sb.AppendLine("<div style='background-color: #34495e; color: white; padding: 15px; text-align: center;'>");
            sb.AppendLine("<p>Forestry System - Sistema de Gestión de Incendios Forestales</p>");
            sb.AppendLine("</div>");

            sb.AppendLine("</div></body></html>");

            return sb.ToString();
        }

        private string GeneratePasswordResetEmailBody(string resetToken)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("<html><body>");
            sb.AppendLine("<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>");
            
            sb.AppendLine("<div style='background-color: #e74c3c; color: white; padding: 20px; text-align: center;'>");
            sb.AppendLine("<h1>🔐 Restablecimiento de Contraseña</h1>");
            sb.AppendLine("</div>");

            sb.AppendLine("<div style='padding: 20px; background-color: #f8f9fa;'>");
            sb.AppendLine("<h2>Solicitud de Restablecimiento</h2>");
            sb.AppendLine("<p>Ha solicitado restablecer su contraseña. Use el siguiente token:</p>");
            sb.AppendLine($"<div style='background-color: #ecf0f1; padding: 15px; text-align: center; font-size: 18px; font-weight: bold;'>");
            sb.AppendLine($"{resetToken}");
            sb.AppendLine("</div>");
            sb.AppendLine("<p><strong>IMPORTANTE:</strong> Este token expira en 1 hora.</p>");
            sb.AppendLine("</div>");

            sb.AppendLine("<div style='background-color: #34495e; color: white; padding: 15px; text-align: center;'>");
            sb.AppendLine("<p>Forestry System - Sistema de Gestión de Incendios Forestales</p>");
            sb.AppendLine("</div>");

            sb.AppendLine("</div></body></html>");

            return sb.ToString();
        }

        private async Task<string> GenerateReportEmailBody(Reporte reporte, string reportType)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("<html><body>");
            sb.AppendLine("<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>");
            
            sb.AppendLine("<div style='background-color: #9b59b6; color: white; padding: 20px; text-align: center;'>");
            sb.AppendLine($"<h1>📊 Nuevo Reporte - {reportType}</h1>");
            sb.AppendLine("</div>");

            sb.AppendLine("<div style='padding: 20px; background-color: #f8f9fa;'>");
            sb.AppendLine($"<h2>Reporte #{reporte.idReporte}</h2>");
            sb.AppendLine($"<p><strong>Tipo:</strong> {reportType}</p>");
            sb.AppendLine($"<p><strong>Fecha:</strong> {reporte.FechaCreacion:dd/MM/yyyy HH:mm}</p>");
            
            if (reporte.Incendio != null)
            {
                sb.AppendLine($"<p><strong>Incendio Relacionado:</strong> {reporte.Incendio.Ubicacion}</p>");
            }

            sb.AppendLine("</div>");

            sb.AppendLine("<div style='background-color: #34495e; color: white; padding: 15px; text-align: center;'>");
            sb.AppendLine("<p>Forestry System - Sistema de Gestión de Incendios Forestales</p>");
            sb.AppendLine("</div>");

            sb.AppendLine("</div></body></html>");

            return sb.ToString();
        }

        private async Task<List<string>> GetReportRecipients(Reporte reporte, string reportType)
        {
            var recipients = new List<string>();

            var users = await _context.Usuarios
                .Where(u => u.Estado == "Activo" && (u.Rol == "Administrador" || u.Rol == "Comando"))
                .ToListAsync();

            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.NumeTel))
                {
                    recipients.Add(user.NumeTel);
                }
            }

            return recipients.Distinct().ToList();
        }

        private string GenerateEmergencyAlertBody(string location, string description)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("<html><body>");
            sb.AppendLine("<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>");
            
            sb.AppendLine("<div style='background-color: #ff0000; color: white; padding: 20px; text-align: center;'>");
            sb.AppendLine("<h1>🚨 ALERTA DE EMERGENCIA</h1>");
            sb.AppendLine("</div>");

            sb.AppendLine("<div style='padding: 20px; background-color: #f8f9fa;'>");
            sb.AppendLine("<h2>Situación Crítica Detectada</h2>");
            sb.AppendLine($"<p><strong>Ubicación:</strong> {location}</p>");
            sb.AppendLine($"<p><strong>Descripción:</strong> {description}</p>");
            sb.AppendLine("<p><strong>ACCIÓN REQUERIDA:</strong> Respuesta inmediata necesaria.</p>");
            sb.AppendLine("</div>");

            sb.AppendLine("<div style='background-color: #34495e; color: white; padding: 15px; text-align: center;'>");
            sb.AppendLine("<p>Forestry System - Sistema de Gestión de Incendios Forestales</p>");
            sb.AppendLine("</div>");

            sb.AppendLine("</div></body></html>");

            return sb.ToString();
        }

        #endregion
    }
} 