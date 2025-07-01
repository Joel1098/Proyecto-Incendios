# 📧 Configuración de Email - Forestry System

## 🔧 Configuración de Gmail SMTP

### 1. **Habilitar Autenticación de 2 Factores**

1. Ve a tu cuenta de Google
2. Navega a **Seguridad**
3. Habilita **Verificación en 2 pasos**

### 2. **Generar Contraseña de Aplicación**

1. Ve a **Seguridad** en tu cuenta de Google
2. Busca **Contraseñas de aplicación**
3. Selecciona **Otra (nombre personalizado)**
4. Escribe "Forestry System" como nombre
5. Copia la contraseña generada (16 caracteres)

### 3. **Configurar appsettings.json**

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "tu-email@gmail.com",
    "SmtpPassword": "tu-contraseña-de-aplicación",
    "EnableSsl": true,
    "FromEmail": "tu-email@gmail.com",
    "FromName": "Forestry System",
    "EnableEmailNotifications": true
  }
}
```

### 4. **Variables de Entorno para Producción**

```bash
# Docker/Render
EmailSettings__SmtpServer=smtp.gmail.com
EmailSettings__SmtpPort=587
EmailSettings__SmtpUsername=tu-email@gmail.com
EmailSettings__SmtpPassword=tu-contraseña-de-aplicación
EmailSettings__EnableSsl=true
EmailSettings__FromEmail=tu-email@gmail.com
EmailSettings__FromName=Forestry System
EmailSettings__EnableEmailNotifications=true
```

## 🧪 Probar la Configuración

### Endpoint de Prueba
```bash
POST /api/email/test
Content-Type: application/json

{
  "testEmail": "tu-email@gmail.com"
}
```

### Respuesta Esperada
```json
{
  "message": "Email de prueba enviado exitosamente",
  "success": true
}
```

## 📋 Tipos de Notificaciones

### 1. **Notificaciones de Incendio**
- **Nuevo incendio**: Se envía automáticamente al crear un incendio
- **Actualización**: Se envía al actualizar un incendio
- **Cierre**: Se envía al cerrar un incendio
- **Emergencia**: Para situaciones críticas

### 2. **Notificaciones de Usuario**
- **Registro**: Email de bienvenida con credenciales
- **Restablecimiento de contraseña**: Token de seguridad

### 3. **Notificaciones de Reportes**
- **Nuevo reporte**: Al generar reportes automáticos
- **Reportes especiales**: Para reportes críticos

### 4. **Alertas de Emergencia**
- **Situaciones críticas**: Para emergencias inmediatas
- **Múltiples destinatarios**: Envío masivo a personal clave

## 🔒 Seguridad

### Mejores Prácticas
1. **Nunca** uses tu contraseña principal de Gmail
2. **Siempre** usa contraseñas de aplicación
3. **Rota** las contraseñas periódicamente
4. **Monitorea** los logs de acceso
5. **Usa** variables de entorno en producción

### Configuración de Seguridad
```json
{
  "EmailSettings": {
    "EnableSsl": true,
    "EnableEmailNotifications": true,
    "MaxRetries": 3,
    "TimeoutSeconds": 30
  }
}
```

## 🚨 Solución de Problemas

### Error: "Authentication failed"
- Verifica que la contraseña de aplicación sea correcta
- Asegúrate de que la verificación en 2 pasos esté habilitada
- Revisa que el email sea correcto

### Error: "Connection timeout"
- Verifica la conectividad a internet
- Comprueba que el puerto 587 esté abierto
- Revisa la configuración del firewall

### Error: "SSL/TLS required"
- Asegúrate de que `EnableSsl` esté en `true`
- Verifica que el puerto sea 587 (no 465)

### Error: "Rate limit exceeded"
- Gmail tiene límites de envío (500 emails/día para cuentas gratuitas)
- Considera usar Gmail Business para mayor volumen
- Implementa colas de email para envíos masivos

## 📊 Monitoreo

### Logs Importantes
```csharp
// En EmailService.cs
_logger.LogInformation($"Email sent successfully to {emailMessage.To}");
_logger.LogError(ex, $"Error sending email to {emailMessage.To}: {ex.Message}");
```

### Métricas a Monitorear
- Tasa de éxito de envío
- Tiempo de respuesta del SMTP
- Errores de autenticación
- Límites de rate limit

## 🔄 Actualización de Configuración

### En Desarrollo
1. Modifica `appsettings.json`
2. Reinicia la aplicación

### En Producción (Render)
1. Ve al dashboard de Render
2. Actualiza las variables de entorno
3. El servicio se reiniciará automáticamente

## 📞 Soporte

### Comandos de Diagnóstico
```bash
# Verificar configuración
curl -X POST http://localhost:8080/api/email/test \
  -H "Content-Type: application/json" \
  -d '{"testEmail": "tu-email@gmail.com"}'

# Ver logs
docker-compose logs forestry-api | grep -i email
```

### Contacto
- Revisa los logs de la aplicación
- Verifica la configuración de Gmail
- Consulta la documentación de Gmail SMTP

---

**¡Configuración completada! 🎉**

Tu sistema Forestry ahora puede enviar notificaciones por email automáticamente. 