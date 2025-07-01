# 🐳 Forestry API - Configuración Docker y Despliegue

## 📋 Resumen Ejecutivo

Se ha configurado completamente el proyecto Forestry API para despliegue con Docker y Render, incluyendo:

### ✅ Archivos Creados/Configurados

1. **`Dockerfile`** - Imagen multi-stage optimizada para .NET 8
2. **`entrypoint.sh`** - Script de inicio con migraciones automáticas
3. **`docker-compose.yml`** - Configuración para desarrollo local
4. **`render.yaml`** - Configuración automática para Render
5. **`deploy.sh`** - Script de automatización de despliegues
6. **`.dockerignore`** - Optimización de builds
7. **`DEPLOYMENT.md`** - Documentación completa de despliegue

### 🚀 Características Implementadas

#### Docker
- ✅ Multi-stage build para optimizar tamaño
- ✅ Soporte para PostgreSQL con migraciones automáticas
- ✅ Health checks y manejo de errores
- ✅ Variables de entorno configurables
- ✅ Script de entrada robusto

#### Render
- ✅ Configuración automática con Blueprint
- ✅ Base de datos PostgreSQL gestionada
- ✅ Variables de entorno automáticas
- ✅ Health checks configurados
- ✅ Despliegue continuo desde Git

#### Automatización
- ✅ Script de despliegue con múltiples opciones
- ✅ Validación de archivos y configuración
- ✅ Tests automáticos de compilación
- ✅ Limpieza de recursos Docker

## 🎯 Comandos Principales

### Despliegue Local
```bash
# Desplegar con Docker Compose
./deploy.sh local

# O manualmente
docker-compose up --build
```

### Despliegue en Render
```bash
# Preparar para Render
./deploy.sh render

# Luego seguir los pasos en render.com
```

### Otros Comandos
```bash
# Construir imagen
./deploy.sh build

# Ejecutar tests
./deploy.sh test

# Limpiar recursos
./deploy.sh clean
```

## 🌐 URLs de Acceso

### Local
- **API**: http://localhost:8080
- **Swagger**: http://localhost:8080/swagger
- **Base de datos**: localhost:5432

### Render (después del despliegue)
- **API**: https://forestry-api.onrender.com
- **Swagger**: https://forestry-api.onrender.com/swagger

## 🔧 Configuración de Base de Datos

### Variables de Entorno
```bash
DB_HOST=your-db-host
DB_PORT=5432
DB_USER=your-db-user
DB_PASSWORD=your-db-password
DB_NAME=forestrydb
ConnectionStrings__DefaultConnection=Host=your-db-host;Port=5432;Database=forestrydb;Username=your-db-user;Password=your-db-password;SSL Mode=Prefer;Trust Server Certificate=true
```

### Migraciones
- ✅ Se ejecutan automáticamente al iniciar
- ✅ Creación automática de base de datos
- ✅ Manejo de errores y reintentos

## 📊 Estado del Proyecto

### ✅ Completado
- [x] Configuración Docker completa
- [x] Scripts de automatización
- [x] Documentación de despliegue
- [x] Configuración para Render
- [x] Tests de compilación
- [x] Optimización de builds

### 🎯 Próximos Pasos
1. **Desplegar en Render**:
   - Ir a https://render.com
   - Conectar repositorio GitHub
   - Usar Blueprint automático
   - Verificar despliegue

2. **Configurar dominio personalizado** (opcional)
3. **Configurar monitoreo** (opcional)
4. **Configurar backups** (opcional)

## 🛠️ Solución de Problemas

### Error de conexión a base de datos
```bash
# Verificar logs
docker-compose logs forestry-api

# Verificar variables de entorno
docker-compose exec forestry-api env | grep DB
```

### Error de migraciones
```bash
# Ejecutar migraciones manualmente
docker-compose exec forestry-api dotnet ef database update
```

### Error de puerto
```bash
# Verificar puertos en uso
lsof -i :8080
lsof -i :5432
```

## 📞 Soporte

- **Documentación completa**: `DEPLOYMENT.md`
- **Script de ayuda**: `./deploy.sh help`
- **Logs de aplicación**: Docker Compose o Render Dashboard

## 🔄 Actualizaciones

Para actualizar la aplicación:
1. Hacer cambios en el código
2. Commit y push a GitHub
3. Render detectará automáticamente los cambios
4. Se ejecutará nuevo build y despliegue
5. Las migraciones se ejecutarán automáticamente

---

**¡El proyecto está listo para producción! 🚀** 