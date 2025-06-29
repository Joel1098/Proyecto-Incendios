# Forestry API - Endpoints Documentation

## Base URL
```
http://localhost:5000/api
```

## Autenticación

### POST /api/auth/login
**Descripción:** Iniciar sesión de usuario
**Body:**
```json
{
  "usuario": "admin",
  "contrasena": "admin123"
}
```
**Response:**
```json
{
  "idUsuario": 1,
  "usuario": "admin",
  "rol": "Administrador",
  "nombre": "Admin Sistema Forestry",
  "estado": "Activo"
}
```

### POST /api/auth/register
**Descripción:** Registrar nuevo usuario
**Body:**
```json
{
  "rol": "Coordinador",
  "nombre": "Juan",
  "apPaterno": "Pérez",
  "apMaterno": "García",
  "numeTel": "555-0101",
  "usuario": "juan.perez",
  "contrasena": "password123",
  "trabajoInicio": "08:00:00",
  "trabajoFin": "18:00:00"
}
```

### GET /api/auth/profile/{id}
**Descripción:** Obtener perfil de usuario

### PUT /api/auth/profile/{id}
**Descripción:** Actualizar perfil de usuario

## Incendios

### GET /api/incendio
**Descripción:** Obtener lista de incendios con filtros opcionales
**Query Parameters:**
- `fechaInicio` (opcional): Fecha de inicio para filtrar
- `fechaFin` (opcional): Fecha de fin para filtrar
- `idEtapa` (opcional): ID de la etapa
- `estado` (opcional): Estado del incendio
- `idUsuarioResponsable` (opcional): ID del usuario responsable
- `ubicacion` (opcional): Ubicación del incendio

**Response:**
```json
[
  {
    "idIncendio": 1,
    "fechaIni": "2024-01-15T10:30:00",
    "fechaFin": null,
    "idEtapa": 2,
    "nombreDespacho": "Despacho Central",
    "nombreComando": "Comando Norte",
    "ubicacion": "Bosque Nacional Sierra Nevada",
    "descripcion": "Incendio en zona boscosa",
    "estado": "Activo",
    "idUsuarioResponsable": 1,
    "fechaCreacion": "2024-01-15T10:30:00",
    "etapaNombre": "Evaluación",
    "etapaColor": "#ff8800",
    "responsableNombre": "Admin Sistema"
  }
]
```

### GET /api/incendio/{id}
**Descripción:** Obtener incendio específico por ID

### POST /api/incendio
**Descripción:** Crear nuevo incendio
**Body:**
```json
{
  "fechaIni": "2024-01-15T10:30:00",
  "idEtapa": 1,
  "nombreDespacho": "Despacho Central",
  "nombreComando": "Comando Norte",
  "ubicacion": "Bosque Nacional Sierra Nevada",
  "descripcion": "Incendio detectado en zona boscosa",
  "idUsuarioResponsable": 1
}
```

### PUT /api/incendio/{id}
**Descripción:** Actualizar incendio existente

### DELETE /api/incendio/{id}
**Descripción:** Eliminar incendio (cambiar estado a Inactivo)

## Etapas

### GET /api/etapas
**Descripción:** Obtener todas las etapas ordenadas
**Response:**
```json
[
  {
    "idEtapa": 1,
    "nombre": "Detección",
    "descripcion": "Primera detección del incendio",
    "orden": 1,
    "estado": "Activo",
    "color": "#ff4444",
    "icono": "fire",
    "fechaCreacion": "2024-01-01T00:00:00"
  }
]
```

### GET /api/etapas/{id}
**Descripción:** Obtener etapa específica

### POST /api/etapas
**Descripción:** Crear nueva etapa

### PUT /api/etapas/{id}
**Descripción:** Actualizar etapa existente

## Personal

### GET /api/personal
**Descripción:** Obtener lista de personal
**Query Parameters:**
- `estado` (opcional): Estado del personal
- `especialidad` (opcional): Especialidad del personal
- `turno` (opcional): Turno del personal

**Response:**
```json
[
  {
    "idTrabajador": 1,
    "nombre": "Roberto",
    "apPaterno": "Sánchez",
    "apMaterno": "Díaz",
    "turno": "Matutino",
    "especialidad": "Bombero Forestal",
    "estado": "Activo",
    "fechaCreada": "2024-01-01T00:00:00"
  }
]
```

### GET /api/personal/{id}
**Descripción:** Obtener personal específico

### POST /api/personal
**Descripción:** Crear nuevo personal
**Body:**
```json
{
  "nombre": "Roberto",
  "apPaterno": "Sánchez",
  "apMaterno": "Díaz",
  "turno": "Matutino",
  "especialidad": "Bombero Forestal"
}
```

### PUT /api/personal/{id}
**Descripción:** Actualizar personal existente

### DELETE /api/personal/{id}
**Descripción:** Eliminar personal (cambiar estado a Inactivo)

## Asignación de Personal a Incendios

### GET /api/incendio/{idIncendio}/personal
**Descripción:** Obtener personal asignado a un incendio específico

### POST /api/incendio/{idIncendio}/personal
**Descripción:** Asignar personal a un incendio
**Body:**
```json
{
  "idTrabajador": 1,
  "rolEnIncendio": "Jefe de Brigada"
}
```

### DELETE /api/incendio/{idIncendio}/personal/{idTrabajador}
**Descripción:** Remover personal de un incendio

## Reportes

### GET /api/reporte
**Descripción:** Obtener lista de reportes
**Query Parameters:**
- `idIncendio` (opcional): ID del incendio
- `idUsuario` (opcional): ID del usuario que creó el reporte
- `fecha` (opcional): Fecha del reporte
- `tipo` (opcional): Tipo de reporte

**Response:**
```json
[
  {
    "idReporte": 1,
    "idIncendio": 1,
    "idUsuario": 1,
    "fecha": "2024-01-15T11:00:00",
    "tipo": "Inicial",
    "contenido": "Incendio detectado en coordenadas 19.4326, -99.1332",
    "estado": "Activo",
    "fechaCreacion": "2024-01-15T11:00:00"
  }
]
```

### GET /api/reporte/{id}
**Descripción:** Obtener reporte específico

### POST /api/reporte
**Descripción:** Crear nuevo reporte
**Body:**
```json
{
  "idIncendio": 1,
  "fecha": "2024-01-15T11:00:00",
  "tipo": "Inicial",
  "contenido": "Incendio detectado en coordenadas 19.4326, -99.1332"
}
```

### PUT /api/reporte/{id}
**Descripción:** Actualizar reporte existente

### DELETE /api/reporte/{id}
**Descripción:** Eliminar reporte (cambiar estado a Inactivo)

## Códigos de Estado HTTP

- **200 OK:** Operación exitosa
- **201 Created:** Recurso creado exitosamente
- **204 No Content:** Operación exitosa sin contenido
- **400 Bad Request:** Datos de entrada inválidos
- **401 Unauthorized:** No autenticado
- **403 Forbidden:** No autorizado
- **404 Not Found:** Recurso no encontrado
- **500 Internal Server Error:** Error interno del servidor

## Ejemplos de Uso

### Flujo típico de gestión de incendio:

1. **Crear incendio:**
   ```bash
   POST /api/incendio
   ```

2. **Asignar personal:**
   ```bash
   POST /api/incendio/{id}/personal
   ```

3. **Crear reportes:**
   ```bash
   POST /api/reporte
   ```

4. **Actualizar etapa del incendio:**
   ```bash
   PUT /api/incendio/{id}
   ```

5. **Finalizar incendio:**
   ```bash
   PUT /api/incendio/{id}
   # Con fechaFin y estado = "Finalizado"
   ```

## Notas Importantes

- Todos los endpoints devuelven JSON
- Las fechas están en formato ISO 8601
- Los IDs son enteros
- Los estados por defecto son "Activo"
- Las contraseñas deben ser hasheadas en producción
- Se recomienda implementar autenticación JWT para mayor seguridad 