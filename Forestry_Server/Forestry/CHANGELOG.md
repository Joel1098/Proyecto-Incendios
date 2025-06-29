# Forestry - API REST para Gestión de Incendios Forestales

Este proyecto es una API REST desarrollada en ASP.NET Core para la gestión integral de incendios forestales. Permite el registro, seguimiento y administración de reportes de incendios y personal involucrado en cada incidente. El sistema está diseñado para facilitar la coordinación y documentación de las acciones tomadas durante la atención de emergencias forestales.

> **Nota:** Actualmente, el sistema utiliza **PostgreSQL** como base de datos relacional, está preparado para ejecutarse fácilmente en entornos Docker y Docker Compose, y funciona como una API REST para ser consumida por aplicaciones frontend como Angular.

# CHANGELOG


---

# Descripción de los elementos principales del sistema

- **Reportes:** Permiten a los usuarios registrar nuevos incidentes de incendio, especificando detalles como ubicación, situación y observaciones. Los reportes son el punto de partida para la gestión de cada emergencia.

- **Incendios:** Cada reporte puede derivar en la creación de un incendio, el cual centraliza la información y seguimiento de la emergencia, incluyendo etapas y personal involucrado.

- **Personal:** Gestión de los trabajadores y brigadistas involucrados en la atención de incendios, permitiendo su asignación a diferentes incidentes y turnos.

- **Usuarios y Roles:** El sistema soporta diferentes tipos de usuarios (Administrador, Jefe, Comandos, etc.), cada uno con permisos y vistas específicas para garantizar la seguridad y el flujo adecuado de información.

- **Autenticación y Seguridad:** Incluye mecanismos de autenticación y encriptación de contraseñas para proteger la información sensible.

- **API REST:** Endpoints JSON para la gestión centralizada de reportes, incendios y personal, adaptados para consumo por aplicaciones frontend.

---

# Ejecución y Build del Proyecto

Este proyecto es una API REST ASP.NET Core. Para ejecutarlo y compilarlo localmente, sigue estos pasos:

## Prerrequisitos
- [.NET SDK 6.0 o superior](https://dotnet.microsoft.com/download)
- PostgreSQL (local o remoto) para la base de datos

## Pasos para compilar y ejecutar

1. **Restaurar dependencias:**
   ```bash
   dotnet restore
   ```
2. **Compilar el proyecto:**
   ```bash
   dotnet build Forestry/Forestry/Forestry.csproj
   ```
3. **Aplicar migraciones (opcional, si usas base de datos local):**
   ```bash
   dotnet ef database update --project Forestry/Forestry/Forestry.csproj
   ```
4. **Ejecutar la aplicación:**
   ```bash
   dotnet run --project Forestry/Forestry/Forestry.csproj
   ```

La API estará disponible en `http://localhost:5000` o el puerto configurado en `launchSettings.json`.

Asegúrate de configurar correctamente la cadena de conexión a la base de datos en `appsettings.json` antes de ejecutar el proyecto.

# Configuración con PostgreSQL y Docker

## Migración a PostgreSQL

1. **Asegúrate de tener instalado el paquete Npgsql.EntityFrameworkCore.PostgreSQL** (ya incluido en el proyecto).
2. **Configura la cadena de conexión en `appsettings.json`:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=db;Port=5432;Database=forestrydb;Username=postgres;Password=forestry123;"
     }
   }
   ```
3. **Crea la base de datos y aplica las migraciones:**
   ```bash
   dotnet ef database update --project Forestry/Forestry/Forestry.csproj
   ```



## Ejecución con Docker Compose

1. **Construye y levanta los servicios:**
   ```bash
   docker-compose up --build
   ```
2. La API estará disponible en `http://localhost:5000` y la base de datos PostgreSQL en el puerto `5432`.

## Endpoints de la API

### Autenticación
- `GET /api/home` - Verificar estado de la API
- `POST /api/home/login` - Iniciar sesión
- `POST /api/home/logout` - Cerrar sesión

### Incendios
- `GET /api/incendio` - Obtener todos los incendios
- `GET /api/incendio/{id}` - Obtener incendio específico
- `POST /api/incendio` - Crear nuevo incendio
- `PUT /api/incendio/{id}` - Actualizar incendio
- `DELETE /api/incendio/{id}` - Eliminar incendio
- `GET /api/incendio/{id}/personal` - Obtener personal de un incendio
- `POST /api/incendio/{id}/personal` - Agregar personal a un incendio

### Reportes
- `GET /api/reportes` - Obtener todos los reportes
- `GET /api/reportes/{id}` - Obtener reporte específico
- `POST /api/reportes` - Crear nuevo reporte
- `PUT /api/reportes/{id}` - Actualizar reporte
- `DELETE /api/reportes/{id}` - Eliminar reporte
- `POST /api/reportes/{id}/confirmar` - Confirmar reporte
- `POST /api/reportes/{id}/rechazar` - Rechazar reporte

### Usuarios
- `GET /api/usuarios` - Obtener todos los usuarios
- `GET /api/usuarios/{id}` - Obtener usuario específico
- `POST /api/usuarios` - Crear nuevo usuario
- `PUT /api/usuarios/{id}` - Actualizar usuario
- `DELETE /api/usuarios/{id}` - Eliminar usuario

### Personal
- `GET /api/usuarios/personal` - Obtener todo el personal
- `GET /api/usuarios/personal/{id}` - Obtener personal específico
- `POST /api/usuarios/personal` - Crear nuevo personal
- `PUT /api/usuarios/personal/{id}` - Actualizar personal
- `DELETE /api/usuarios/personal/{id}` - Eliminar personal

--- 