# Forestry API - Changelog

## [1.4.0] - 2024-12-19



### Database Schema
- **Tablas principales**: Usuarios, Etapas, Incendio, Personal, Reporte, IncendioPersonal
- **Relaciones corregidas**:
  - Incendio → Etapas (idEtapa)
  - Incendio → Usuarios (idUsuarioResponsable)
  - Reporte → Incendio (idIncendio)
  - Reporte → Usuarios (idUsuario)
  - IncendioPersonal → Incendio (idIncendio)
  - IncendioPersonal → Personal (IdTrabajador)

### API Endpoints Implementados
- **Autenticación**: `/api/auth/login`, `/api/auth/register`, `/api/auth/profile/{id}`
- **Incendios**: CRUD completo con filtros
- **Etapas**: CRUD completo
- **Personal**: CRUD completo con filtros
- **Reportes**: CRUD completo
- **Asignación de Personal**: Gestión de personal por incendio

### Technical Details
- **Base de datos**: PostgreSQL con Npgsql.EntityFrameworkCore
- **API**: ASP.NET Core REST API
- **CORS**: Configurado para Angular frontend
- **Docker**: Configuración completa con docker-compose
- **Índices**: Optimizados para mejor rendimiento

---



### Features
- **Gestión de usuarios**: CRUD completo de usuarios del sistema
- **Gestión de incendios**: Registro y seguimiento de incendios forestales
- **Etapas de incendio**: Control de etapas del proceso de gestión
- **Personal**: Gestión del personal disponible
- **Reportes**: Sistema de reportes por incendio
- **Bitácoras**: Sistema complejo de bitácoras (posteriormente removido)

---

## Instrucciones de Instalación y Uso

### Requisitos
- .NET 6.0 SDK
- Docker y Docker Compose
- PostgreSQL (manejado por Docker)

### Configuración Inicial
```bash
# Clonar el repositorio
git clone <repository-url>
cd proyecto_WB

# Ejecutar con Docker
docker-compose up -d

# Aplicar migraciones (si es necesario)
dotnet ef database update
```

### Endpoints Principales
- **API Base**: http://localhost:5000/api
- **Documentación**: Ver API_ENDPOINTS.md
- **Esquema DB**: Ver forestry_database_schema.dbml


### Base de Datos
- **Tipo**: PostgreSQL
- **Puerto**: 5432
- **Base de datos**: forestrydb
- **Usuario**: postgres
- **Contraseña**: forestry123

### Frontend
- **Tecnología**: Angular (desarrollado por separado)
- **CORS**: Configurado para permitir requests desde Angular
- **Autenticación**: Sistema básico de login (mejorar con JWT en producción) 