using System;
using System.Text;

namespace Forestry.Services
{
    public static class EmailTemplates
    {
        public static string GetBaseTemplate(string title, string content)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='es'>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset='UTF-8'>");
            sb.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            sb.AppendLine("    <title>Forestry System</title>");
            sb.AppendLine("    <style>");
            sb.AppendLine("        body { font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f4f4f4; }");
            sb.AppendLine("        .container { max-width: 600px; margin: 0 auto; background-color: #ffffff; }");
            sb.AppendLine("        .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }");
            sb.AppendLine("        .content { padding: 30px; line-height: 1.6; }");
            sb.AppendLine("        .footer { background-color: #34495e; color: white; padding: 20px; text-align: center; font-size: 12px; }");
            sb.AppendLine("        .button { display: inline-block; padding: 12px 24px; background-color: #3498db; color: white; text-decoration: none; border-radius: 5px; margin: 10px 0; }");
            sb.AppendLine("        .alert { background-color: #f8d7da; border: 1px solid #f5c6cb; color: #721c24; padding: 15px; border-radius: 5px; margin: 15px 0; }");
            sb.AppendLine("        .success { background-color: #d4edda; border: 1px solid #c3e6cb; color: #155724; padding: 15px; border-radius: 5px; margin: 15px 0; }");
            sb.AppendLine("        .info { background-color: #d1ecf1; border: 1px solid #bee5eb; color: #0c5460; padding: 15px; border-radius: 5px; margin: 15px 0; }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div class='container'>");
            sb.AppendLine("        <div class='header'>");
            sb.AppendLine("            <h1>🌲 Forestry System</h1>");
            sb.AppendLine($"            <h2>{title}</h2>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class='content'>");
            sb.AppendLine(content);
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class='footer'>");
            sb.AppendLine("            <p>Sistema de Gestión de Incendios Forestales</p>");
            sb.AppendLine("            <p>Este es un mensaje automático, no responda a este correo.</p>");
            sb.AppendLine("            <p>&copy; 2024 Forestry System. Todos los derechos reservados.</p>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public static string GetIncendioTemplate(string ubicacion, string descripcion, string estado, string etapa, DateTime fechaInicio, DateTime? fechaFin = null, string responsable = null)
        {
            var content = new StringBuilder();
            
            content.AppendLine("<div class='info'>");
            content.AppendLine("    <h3>🔥 Información del Incendio</h3>");
            content.AppendLine("</div>");
            
            content.AppendLine("<table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>");
            content.AppendLine("    <tr><td style='padding: 10px; border-bottom: 1px solid #ddd;'><strong>Ubicación:</strong></td><td style='padding: 10px; border-bottom: 1px solid #ddd;'>" + ubicacion + "</td></tr>");
            content.AppendLine("    <tr><td style='padding: 10px; border-bottom: 1px solid #ddd;'><strong>Descripción:</strong></td><td style='padding: 10px; border-bottom: 1px solid #ddd;'>" + descripcion + "</td></tr>");
            content.AppendLine("    <tr><td style='padding: 10px; border-bottom: 1px solid #ddd;'><strong>Estado:</strong></td><td style='padding: 10px; border-bottom: 1px solid #ddd;'>" + estado + "</td></tr>");
            content.AppendLine("    <tr><td style='padding: 10px; border-bottom: 1px solid #ddd;'><strong>Etapa:</strong></td><td style='padding: 10px; border-bottom: 1px solid #ddd;'>" + etapa + "</td></tr>");
            content.AppendLine("    <tr><td style='padding: 10px; border-bottom: 1px solid #ddd;'><strong>Fecha de Inicio:</strong></td><td style='padding: 10px; border-bottom: 1px solid #ddd;'>" + fechaInicio.ToString("dd/MM/yyyy HH:mm") + "</td></tr>");
            
            if (fechaFin.HasValue)
            {
                content.AppendLine("    <tr><td style='padding: 10px; border-bottom: 1px solid #ddd;'><strong>Fecha de Fin:</strong></td><td style='padding: 10px; border-bottom: 1px solid #ddd;'>" + fechaFin.Value.ToString("dd/MM/yyyy HH:mm") + "</td></tr>");
            }
            
            if (!string.IsNullOrEmpty(responsable))
            {
                content.AppendLine("    <tr><td style='padding: 10px; border-bottom: 1px solid #ddd;'><strong>Responsable:</strong></td><td style='padding: 10px; border-bottom: 1px solid #ddd;'>" + responsable + "</td></tr>");
            }
            
            content.AppendLine("</table>");

            return GetBaseTemplate("Notificación de Incendio", content.ToString());
        }

        public static string GetEmergencyTemplate(string ubicacion, string descripcion)
        {
            var content = new StringBuilder();
            
            content.AppendLine("<div class='alert'>");
            content.AppendLine("    <h3>🚨 ALERTA DE EMERGENCIA</h3>");
            content.AppendLine("    <p><strong>Se ha detectado una situación crítica que requiere atención inmediata.</strong></p>");
            content.AppendLine("</div>");
            
            content.AppendLine("<div style='background-color: #fff3cd; border: 1px solid #ffeaa7; color: #856404; padding: 15px; border-radius: 5px; margin: 15px 0;'>");
            content.AppendLine("    <h4>Detalles de la Emergencia:</h4>");
            content.AppendLine("    <p><strong>Ubicación:</strong> " + ubicacion + "</p>");
            content.AppendLine("    <p><strong>Descripción:</strong> " + descripcion + "</p>");
            content.AppendLine("</div>");
            
            content.AppendLine("<p><strong>ACCIÓN REQUERIDA:</strong> Respuesta inmediata necesaria.</p>");
            content.AppendLine("<p>Por favor, tome las medidas necesarias lo antes posible.</p>");

            return GetBaseTemplate("Alerta de Emergencia", content.ToString());
        }

        public static string GetRegistrationTemplate(string username, string password)
        {
            var content = new StringBuilder();
            
            content.AppendLine("<div class='success'>");
            content.AppendLine("    <h3>✅ Registro Exitoso</h3>");
            content.AppendLine("    <p>Su cuenta ha sido creada exitosamente en el Sistema Forestry.</p>");
            content.AppendLine("</div>");
            
            content.AppendLine("<div style='background-color: #e8f5e8; border: 1px solid #c3e6cb; padding: 15px; border-radius: 5px; margin: 15px 0;'>");
            content.AppendLine("    <h4>Credenciales de Acceso:</h4>");
            content.AppendLine("    <p><strong>Usuario:</strong> " + username + "</p>");
            content.AppendLine("    <p><strong>Contraseña:</strong> " + password + "</p>");
            content.AppendLine("</div>");
            
            content.AppendLine("<div class='alert'>");
            content.AppendLine("    <p><strong>IMPORTANTE:</strong> Por seguridad, cambie su contraseña después del primer inicio de sesión.</p>");
            content.AppendLine("</div>");
            
            content.AppendLine("<p>Bienvenido al Sistema de Gestión de Incendios Forestales.</p>");

            return GetBaseTemplate("Bienvenido a Forestry System", content.ToString());
        }

        public static string GetPasswordResetTemplate(string resetToken)
        {
            var content = new StringBuilder();
            
            content.AppendLine("<div class='info'>");
            content.AppendLine("    <h3>🔐 Restablecimiento de Contraseña</h3>");
            content.AppendLine("    <p>Ha solicitado restablecer su contraseña en el Sistema Forestry.</p>");
            content.AppendLine("</div>");
            
            content.AppendLine("<div style='background-color: #d1ecf1; border: 1px solid #bee5eb; padding: 15px; border-radius: 5px; margin: 15px 0; text-align: center;'>");
            content.AppendLine("    <h4>Token de Restablecimiento:</h4>");
            content.AppendLine("    <div style='background-color: #ffffff; padding: 10px; border-radius: 5px; font-size: 18px; font-weight: bold; letter-spacing: 2px;'>");
            content.AppendLine("        " + resetToken);
            content.AppendLine("    </div>");
            content.AppendLine("</div>");
            
            content.AppendLine("<div class='alert'>");
            content.AppendLine("    <p><strong>IMPORTANTE:</strong> Este token expira en 1 hora por motivos de seguridad.</p>");
            content.AppendLine("</div>");
            
            content.AppendLine("<p>Si no solicitó este restablecimiento, ignore este correo.</p>");

            return GetBaseTemplate("Restablecimiento de Contraseña", content.ToString());
        }

        public static string GetReportTemplate(string reportType, int reportId, DateTime fechaCreacion, string incendioUbicacion = null)
        {
            var content = new StringBuilder();
            
            content.AppendLine("<div class='info'>");
            content.AppendLine("    <h3>📊 Nuevo Reporte Generado</h3>");
            content.AppendLine("    <p>Se ha generado un nuevo reporte en el Sistema Forestry.</p>");
            content.AppendLine("</div>");
            
            content.AppendLine("<div style='background-color: #e8f4fd; border: 1px solid #bee5eb; padding: 15px; border-radius: 5px; margin: 15px 0;'>");
            content.AppendLine("    <h4>Detalles del Reporte:</h4>");
            content.AppendLine("    <p><strong>Tipo de Reporte:</strong> " + reportType + "</p>");
            content.AppendLine("    <p><strong>ID del Reporte:</strong> #" + reportId + "</p>");
            content.AppendLine("    <p><strong>Fecha de Creación:</strong> " + fechaCreacion.ToString("dd/MM/yyyy HH:mm") + "</p>");
            
            if (!string.IsNullOrEmpty(incendioUbicacion))
            {
                content.AppendLine("    <p><strong>Incendio Relacionado:</strong> " + incendioUbicacion + "</p>");
            }
            
            content.AppendLine("</div>");
            
            content.AppendLine("<p>El reporte está disponible en el sistema para su revisión.</p>");

            return GetBaseTemplate("Nuevo Reporte - " + reportType, content.ToString());
        }
    }
} 