# 📧 Configuración de Email - Forestry System

## ✅ **Configuración Actual**

### **Credenciales Gmail:**
- **Email:** joeldgjo98@gmail.com
- **Contraseña de Aplicación:** hiir jopq uuah fhhs
- **Servidor SMTP:** smtp.gmail.com
- **Puerto:** 587
- **SSL/TLS:** Habilitado

### **Configuración Aplicada:**
- ✅ `appsettings.json` - Configuración local
- ✅ `docker-compose.yml` - Variables de entorno Docker
- ✅ `render.yaml` - Configuración de producción

## 🧪 **Probar el Sistema**

### **1. Iniciar la Aplicación:**
```bash
# Opción 1: Desarrollo local
dotnet run

# Opción 2: Con Docker
docker-compose up --build
```

### **2. Ejecutar Pruebas Automáticas:**
```bash
./test_email.sh
```

### **3. Pruebas Manuales:**

#### **Probar Configuración Básica:**
```bash
curl -X POST http://localhost:8080/api/email/test \
  -H "Content-Type: application/json" \
  -d '{"testEmail": "joeldgjo98@gmail.com"}'
```

#### **Enviar Email Personalizado:**
```bash
curl -X POST http://localhost:8080/api/email/send \
  -H "Content-Type: application/json" \
  -d '{
    "to": "joeldgjo98@gmail.com",
    "subject": "Prueba - Forestry System",
    "body": "<h1>¡Hola!</h1><p>Prueba del sistema de email.</p>",
    "isHtml": true
  }'
```

#### **Probar Notificación de Registro:**
```bash
curl -X POST http://localhost:8080/api/email/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joeldgjo98@gmail.com",
    "username": "usuario_test",
    "password": "password123"
  }'
```

#### **Probar Alerta de Emergencia:**
```bash
curl -X POST http://localhost:8080/api/email/emergency \
  -H "Content-Type: application/json" \
  -d '{
    "location": "Bosque Nacional Test",
    "description": "Prueba de alerta de emergencia",
    "recipients": ["joeldgjo98@gmail.com"]
  }'
```

## 📋 **Endpoints Disponibles**

| Endpoint | Método | Descripción |
|----------|--------|-------------|
| `/api/email/test` | POST | Probar configuración |
| `/api/email/send` | POST | Email personalizado |
| `/api/email/register` | POST | Notificación de registro |
| `/api/email/password-reset` | POST | Restablecimiento de contraseña |
| `/api/email/incendio/{id}` | POST | Notificación de incendio |
| `/api/email/report/{id}` | POST | Notificación de reporte |
| `/api/email/emergency` | POST | Alerta de emergencia |

## 🔄 **Notificaciones Automáticas**

### **Se Envían Automáticamente:**
- ✅ **Nuevos incendios** - Al crear un incendio
- ✅ **Registro de usuarios** - Al registrar un usuario
- ✅ **Actualizaciones de incendios** - Al modificar un incendio
- ✅ **Reportes críticos** - Al generar reportes especiales

### **Destinatarios por Rol:**
- **Administrador:** Todas las notificaciones
- **Despacho:** Nuevos incendios, cierres, emergencias
- **Comando:** Actualizaciones, reportes, emergencias
- **Personal:** Emergencias y notificaciones específicas

## 📊 **Monitoreo y Logs**

### **Ver Logs de Email:**
```bash
# Docker
docker-compose logs forestry-api | grep -i email

# Desarrollo local
# Los logs aparecen en la consola de la aplicación
```

### **Logs Importantes:**
- ✅ Éxitos de envío
- ❌ Errores de configuración
- ⚠️ Problemas de conectividad
- 📊 Rate limits de Gmail

## 🚨 **Solución de Problemas**

### **Error: "Authentication failed"**
- ✅ Verificar que la contraseña de aplicación sea correcta
- ✅ Asegurar que 2FA esté habilitado en Gmail
- ✅ Comprobar que el email sea correcto

### **Error: "Connection timeout"**
- ✅ Verificar conectividad a internet
- ✅ Comprobar que el puerto 587 esté abierto
- ✅ Revisar configuración del firewall

### **Error: "Rate limit exceeded"**
- ⚠️ Gmail tiene límite de 500 emails/día (cuenta gratuita)
- 💡 Considerar Gmail Business para mayor volumen
- 🔄 Implementar colas para envíos masivos

## 🔒 **Seguridad**

### **Mejores Prácticas Implementadas:**
- ✅ **Contraseña de aplicación** (no contraseña principal)
- ✅ **SSL/TLS** habilitado
- ✅ **Variables de entorno** para producción
- ✅ **Logging seguro** sin exponer credenciales
- ✅ **Manejo de errores** robusto

### **Configuración de Seguridad:**
```json
{
  "EmailSettings": {
    "EnableSsl": true,
    "EnableEmailNotifications": true,
    "SmtpPort": 587
  }
}
```

## 🌐 **Despliegue en Render**

### **Variables de Entorno Configuradas:**
```bash
EmailSettings__SmtpServer=smtp.gmail.com
EmailSettings__SmtpPort=587
EmailSettings__SmtpUsername=joeldgjo98@gmail.com
EmailSettings__SmtpPassword=hiir jopq uuah fhhs
EmailSettings__EnableSsl=true
EmailSettings__FromEmail=joeldgjo98@gmail.com
EmailSettings__FromName=Forestry System
EmailSettings__EnableEmailNotifications=true
```

### **Pasos para Desplegar:**
1. ✅ Conectar repositorio en Render
2. ✅ Render detectará `render.yaml` automáticamente
3. ✅ Las variables de entorno se configurarán
4. ✅ El sistema de email funcionará automáticamente

## 📞 **Soporte**

### **Comandos de Diagnóstico:**
```bash
# Verificar configuración
./test_email.sh

# Ver logs
docker-compose logs forestry-api | grep -i email

# Probar endpoint específico
curl -X POST http://localhost:8080/api/email/test \
  -H "Content-Type: application/json" \
  -d '{"testEmail": "joeldgjo98@gmail.com"}'
```

### **Contacto:**
- 📧 Email: joeldgjo98@gmail.com
- 📚 Documentación: `EMAIL_SETUP.md`
- 🔧 Scripts: `test_email.sh`, `deploy.sh`

---

## 🎯 **Estado Actual**

### ✅ **Completado:**
- [x] Configuración Gmail SMTP
- [x] Credenciales reales configuradas
- [x] Plantillas HTML profesionales
- [x] Endpoints de prueba
- [x] Integración automática
- [x] Configuración Docker
- [x] Configuración Render
- [x] Scripts de prueba
- [x] Documentación completa

### 🚀 **Listo para:**
- ✅ **Desarrollo local**
- ✅ **Pruebas**
- ✅ **Despliegue en Render**
- ✅ **Producción**

---

**¡El sistema de email está completamente configurado y listo para usar! 🎉** 