# Diagrama de Base de Datos - Sistema Forestry
## Formato para dbdiagram.io

```sql
// Copia y pega este código en dbdiagram.io

Table Usuarios {
  idUsuario int [pk]
  Rol varchar(20) [not null]
  Estado varchar(20) [default: "Activo"]
  Nombre varchar(50) [not null]
  ApPaterno varchar(50) [not null]
  ApMaterno varchar(50) [not null]
  NumeTel varchar(20)
  Usuario varchar(50) [unique, not null]
  Contrasena varchar(256) [not null]
  TrabajoInicio time
  TrabajoFin time
  FechaCreacion timestamp [default: `now()`]
  DiasLaborales varchar(100)
}

Table Etapas {
  idEtapa int [pk]
  Nombre varchar(50) [not null]
  Descripcion text
  Orden int [not null]
  Estado varchar(20) [default: "Activo"]
  Color varchar(7) [default: "#007bff"]
  Icono varchar(50)
  FechaCreacion timestamp [default: `now()`]
}

Table Incendio {
  idIncendio int [pk]
  FechaIni timestamp [not null]
  FechaFin timestamp
  idEtapa int [ref: > Etapas.idEtapa, not null]
  NombreDespacho varchar(100)
  NombreComando varchar(100)
  Ubicacion varchar(200) [not null]
  Descripcion text
  Estado varchar(20) [default: "Activo"]
  idUsuarioResponsable int [ref: > Usuarios.idUsuario]
  FechaCreacion timestamp [default: `now()`]
}

Table Personal {
  IdTrabajador int [pk]
  Nombre varchar(50) [not null]
  ApPaterno varchar(50) [not null]
  ApMaterno varchar(50) [not null]
  Turno varchar(20)
  Especialidad varchar(100)
  Estado varchar(20) [default: "Activo"]
  FechaCreada timestamp [default: `now()`]
}

Table Reporte {
  idReporte int [pk]
  idIncendio int [ref: > Incendio.idIncendio]
  idUsuario int [ref: > Usuarios.idUsuario]
  Fecha timestamp [default: `now()`]
  Tipo varchar(50) [not null]
  Contenido text [not null]
  Estado varchar(20) [default: "Activo"]
  FechaCreacion timestamp [default: `now()`]
  Lugar varchar(200)
  Situacion varchar(200)
  Detalles varchar(500)
}

Table IncendioPersonal {
  idIncendio int [ref: > Incendio.idIncendio]
  IdTrabajador int [ref: > Personal.IdTrabajador]
  FechaAsignacion timestamp [default: `now()`]
  RolEnIncendio varchar(100)
  Estado varchar(20) [default: "Activo"]
  primary key (idIncendio, IdTrabajador)
}

Table BitacoraMedidaInicial {
  IdBitacora int [pk]
  FechaCreada timestamp [not null]
  Pregunta1 varchar
  Pregunta2 varchar
  Peligros varchar
  PotencialExpansion varchar
  CaracterFuego varchar
  PendienteFuego varchar
  PosicionPendiente varchar
  TipoCombustible varchar
  CombustiblesAdyacentes varchar
  Aspecto varchar
  DireccionViento varchar
  IdIncendio int [ref: > Incendio.idIncendio]
}
```

## Instrucciones de Uso

1. Ve a [dbdiagram.io](https://dbdiagram.io)
2. Haz clic en "Create new diagram"
3. Copia y pega el código SQL de arriba en el editor
4. El diagrama se generará automáticamente

## Relaciones del Diagrama

- **Incendio** → **Etapas** (Muchos a Uno)
- **Incendio** → **Usuarios** (Muchos a Uno) - Usuario Responsable
- **Reporte** → **Incendio** (Muchos a Uno) - Opcional
- **Reporte** → **Usuarios** (Muchos a Uno) - Opcional
- **IncendioPersonal** → **Incendio** (Muchos a Uno)
- **IncendioPersonal** → **Personal** (Muchos a Uno)
- **BitacoraMedidaInicial** → **Incendio** (Muchos a Uno) - Opcional

## Características del Diagrama

- **7 tablas principales** con relaciones claras
- **Campos esenciales** sin sobrecarga
- **Valores por defecto** especificados
- **Claves primarias y foráneas** definidas
- **Restricciones** de integridad
- **Tipos de datos** apropiados

## Flujo de Datos

1. **Usuarios** se registran y autentican
2. **Reportes** se crean (opcionalmente asociados a incendios)
3. **Incendios** se crean con etapa inicial
4. **Personal** se asigna a incendios específicos
5. **Bitácoras** se crean para evaluación técnica
6. **Etapas** se actualizan durante el proceso 