# 🚀 Guía de Despliegue - Forestry API

## 📋 Requisitos Previos

- Docker Desktop instalado
- Git configurado
- Cuenta en Render.com (para despliegue en la nube)

## 🐳 Despliegue Local con Docker

### 1. Clonar el repositorio
```bash
git clone https://github.com/Joel1098/Proyecto-Incendios.git
cd Proyecto-Incendios/Forestry_Server
```

### 2. Ejecutar con Docker Compose
```bash
# Construir y ejecutar todos los servicios
docker-compose up --build

# Ejecutar en segundo plano
docker-compose up -d --build
```

### 3. Verificar el despliegue
- **API**: http://localhost:8080
- **Swagger**: http://localhost:8080/swagger
- **Base de datos**: localhost:5432

### 4. Comandos útiles
```bash
# Ver logs
docker-compose logs -f forestry-api

# Detener servicios
docker-compose down

# Limpiar volúmenes
docker-compose down -v
```

## ☁️ Despliegue en Render

### 1. Preparar el repositorio
Asegúrate de que todos los archivos estén en el repositorio:
- `Dockerfile`
- `entrypoint.sh`
- `render.yaml`
- `docker-compose.yml`

### 2. Conectar con Render
1. Ve a [render.com](https://render.com)
2. Crea una nueva cuenta o inicia sesión
3. Haz clic en "New +" → "Blueprint"
4. Conecta tu repositorio de GitHub

### 3. Configurar el despliegue
1. Render detectará automáticamente el `render.yaml`
2. Selecciona la región (Oregon recomendado)
3. Confirma la configuración

### 4. Variables de entorno
Render configurará automáticamente:
- `DB_HOST`
- `DB_PORT`
- `DB_USER`
- `DB_PASSWORD`
- `DB_NAME`
- `ConnectionStrings__DefaultConnection`

### 5. Verificar el despliegue
- La URL se generará automáticamente
- Swagger estará disponible en `/swagger`
- Las migraciones se ejecutarán automáticamente

## 🔧 Configuración de Base de Datos

### Variables de Entorno Requeridas
```bash
# Base de datos
DB_HOST=your-db-host
DB_PORT=5432
DB_USER=your-db-user
DB_PASSWORD=your-db-password
DB_NAME=forestrydb

# Cadena de conexión completa
ConnectionStrings__DefaultConnection=Host=your-db-host;Port=5432;Database=forestrydb;Username=your-db-user;Password=your-db-password;SSL Mode=Prefer;Trust Server Certificate=true

# Aplicación
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
```

## 📊 Endpoints Disponibles

### Autenticación
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/register` - Registrar usuario
- `GET /api/auth/profile/{id}` - Obtener perfil

### Incendios
- `GET /api/incendio` - Listar incendios
- `POST /api/incendio` - Crear incendio
- `GET /api/incendio/{id}` - Obtener incendio
- `PUT /api/incendio/{id}` - Actualizar incendio
- `DELETE /api/incendio/{id}` - Eliminar incendio

### Reportes
- `GET /api/reportes` - Listar reportes
- `POST /api/reportes` - Crear reporte
- `GET /api/reportes/incendios` - Incendios por estado
- `GET /api/reportes/ver-incendio/{id}` - Ver incendio específico

### Personal
- `GET /api/usuarios/personal` - Listar personal
- `POST /api/usuarios/personal` - Crear personal
- `POST /api/incendio/{id}/personal` - Asignar personal

## 🔍 Monitoreo y Logs

### Logs de la aplicación
```bash
# Docker local
docker-compose logs forestry-api

# Render
# Los logs están disponibles en el dashboard de Render
```

### Health Check
- Endpoint: `/swagger`
- Método: GET
- Respuesta esperada: 200 OK

## 🛠️ Solución de Problemas

### Error de conexión a base de datos
1. Verificar variables de entorno
2. Comprobar que PostgreSQL esté ejecutándose
3. Verificar credenciales

### Error de migraciones
1. Verificar permisos de base de datos
2. Comprobar que la base de datos existe
3. Revisar logs del entrypoint.sh

### Error de puerto
1. Verificar que el puerto 8080 esté disponible
2. Comprobar configuración de ASPNETCORE_URLS

## 📞 Soporte

Para problemas técnicos:
- Revisar logs en Render Dashboard
- Verificar configuración de variables de entorno
- Comprobar conectividad de base de datos

## 🔄 Actualizaciones

Para actualizar la aplicación:
1. Hacer push de cambios al repositorio
2. Render detectará automáticamente los cambios
3. Se ejecutará un nuevo build y despliegue
4. Las migraciones se ejecutarán automáticamente 