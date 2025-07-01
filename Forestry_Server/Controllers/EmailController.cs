using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Forestry.Services;
using Forestry.Models;
using System.Collections.Generic;

namespace Forestry.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IEmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Envía un email personalizado
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailMessage emailMessage)
        {
            try
            {
                var result = await _emailService.SendEmailAsync(emailMessage);
                
                if (result)
                {
                    return Ok(new { message = "Email enviado exitosamente", success = true });
                }
                else
                {
                    return BadRequest(new { message = "Error al enviar el email", success = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en SendEmail");
                return StatusCode(500, new { message = "Error interno del servidor", success = false });
            }
        }

        /// <summary>
        /// Envía notificación de incendio
        /// </summary>
        [HttpPost("incendio/{id}")]
        public async Task<IActionResult> SendIncendioNotification(int id, [FromQuery] string notificationType = "nuevo")
        {
            try
            {
                var result = await _emailService.SendIncendioNotificationAsync(id, notificationType);
                
                if (result)
                {
                    return Ok(new { message = "Notificación de incendio enviada exitosamente", success = true });
                }
                else
                {
                    return BadRequest(new { message = "Error al enviar la notificación de incendio", success = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en SendIncendioNotification");
                return StatusCode(500, new { message = "Error interno del servidor", success = false });
            }
        }

        /// <summary>
        /// Envía email de registro de usuario
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> SendRegistrationEmail([FromBody] RegistrationEmailRequest request)
        {
            try
            {
                var result = await _emailService.SendUserRegistrationAsync(request.Email, request.Username, request.Password);
                
                if (result)
                {
                    return Ok(new { message = "Email de registro enviado exitosamente", success = true });
                }
                else
                {
                    return BadRequest(new { message = "Error al enviar el email de registro", success = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en SendRegistrationEmail");
                return StatusCode(500, new { message = "Error interno del servidor", success = false });
            }
        }

        /// <summary>
        /// Envía email de restablecimiento de contraseña
        /// </summary>
        [HttpPost("password-reset")]
        public async Task<IActionResult> SendPasswordResetEmail([FromBody] PasswordResetRequest request)
        {
            try
            {
                var result = await _emailService.SendPasswordResetAsync(request.Email, request.ResetToken);
                
                if (result)
                {
                    return Ok(new { message = "Email de restablecimiento enviado exitosamente", success = true });
                }
                else
                {
                    return BadRequest(new { message = "Error al enviar el email de restablecimiento", success = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en SendPasswordResetEmail");
                return StatusCode(500, new { message = "Error interno del servidor", success = false });
            }
        }

        /// <summary>
        /// Envía notificación de reporte
        /// </summary>
        [HttpPost("report/{id}")]
        public async Task<IActionResult> SendReportNotification(int id, [FromQuery] string reportType = "general")
        {
            try
            {
                var result = await _emailService.SendReportNotificationAsync(id, reportType);
                
                if (result)
                {
                    return Ok(new { message = "Notificación de reporte enviada exitosamente", success = true });
                }
                else
                {
                    return BadRequest(new { message = "Error al enviar la notificación de reporte", success = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en SendReportNotification");
                return StatusCode(500, new { message = "Error interno del servidor", success = false });
            }
        }

        /// <summary>
        /// Envía alerta de emergencia
        /// </summary>
        [HttpPost("emergency")]
        public async Task<IActionResult> SendEmergencyAlert([FromBody] EmergencyAlertRequest request)
        {
            try
            {
                var result = await _emailService.SendEmergencyAlertAsync(request.Location, request.Description, request.Recipients);
                
                if (result)
                {
                    return Ok(new { message = "Alerta de emergencia enviada exitosamente", success = true });
                }
                else
                {
                    return BadRequest(new { message = "Error al enviar la alerta de emergencia", success = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en SendEmergencyAlert");
                return StatusCode(500, new { message = "Error interno del servidor", success = false });
            }
        }

        /// <summary>
        /// Prueba la configuración de email
        /// </summary>
        [HttpPost("test")]
        public async Task<IActionResult> TestEmailConfiguration([FromBody] TestEmailRequest request)
        {
            try
            {
                var testMessage = new EmailMessage
                {
                    To = request.TestEmail,
                    Subject = "🧪 Prueba de Configuración - Forestry System",
                    Body = EmailTemplates.GetBaseTemplate("Prueba de Configuración", 
                        "<div class='success'><h3>✅ Configuración Exitosa</h3><p>El sistema de email está funcionando correctamente.</p></div>"),
                    IsHtml = true
                };

                var result = await _emailService.SendEmailAsync(testMessage);
                
                if (result)
                {
                    return Ok(new { message = "Email de prueba enviado exitosamente", success = true });
                }
                else
                {
                    return BadRequest(new { message = "Error al enviar el email de prueba", success = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en TestEmailConfiguration");
                return StatusCode(500, new { message = "Error interno del servidor", success = false });
            }
        }
    }

    // DTOs para las peticiones
    public class RegistrationEmailRequest
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class PasswordResetRequest
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }
    }

    public class EmergencyAlertRequest
    {
        public string Location { get; set; }
        public string Description { get; set; }
        public List<string> Recipients { get; set; }
    }

    public class TestEmailRequest
    {
        public string TestEmail { get; set; }
    }
} 