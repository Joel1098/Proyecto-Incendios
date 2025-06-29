-- Forestry Database Schema for PostgreSQL
-- Script para crear la base de datos simplificada sin bitácoras, recursos ni condiciones climáticas

-- Crear la base de datos
CREATE DATABASE forestrydb;

-- Conectar a la base de datos
\c forestrydb;

-- Crear tabla Usuarios
CREATE TABLE "Usuarios" (
    "idUsuario" SERIAL PRIMARY KEY,
    "Rol" VARCHAR(20) NOT NULL,
    "Estado" VARCHAR(20) NOT NULL DEFAULT 'Activo',
    "Nombre" VARCHAR(50) NOT NULL,
    "ApPaterno" VARCHAR(50) NOT NULL,
    "ApMaterno" VARCHAR(50) NOT NULL,
    "NumeTel" VARCHAR(20),
    "Usuario" VARCHAR(50) NOT NULL UNIQUE,
    "Contrasena" VARCHAR(256) NOT NULL,
    "TrabajoInicio" TIME,
    "TrabajoFin" TIME,
    "FechaCreacion" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Crear tabla Etapas
CREATE TABLE "Etapas" (
    "idEtapa" SERIAL PRIMARY KEY,
    "Nombre" VARCHAR(50) NOT NULL,
    "Descripcion" TEXT,
    "Orden" INTEGER NOT NULL,
    "Estado" VARCHAR(20) NOT NULL DEFAULT 'Activo',
    "Color" VARCHAR(7) DEFAULT '#007bff',
    "Icono" VARCHAR(50),
    "FechaCreacion" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Crear tabla Incendio
CREATE TABLE "Incendio" (
    "idIncendio" SERIAL PRIMARY KEY,
    "FechaIni" TIMESTAMP NOT NULL,
    "FechaFin" TIMESTAMP,
    "idEtapa" INTEGER NOT NULL,
    "NombreDespacho" VARCHAR(100),
    "NombreComando" VARCHAR(100),
    "Ubicacion" VARCHAR(200),
    "Descripcion" TEXT,
    "Estado" VARCHAR(20) NOT NULL DEFAULT 'Activo',
    "idUsuarioResponsable" INTEGER,
    "FechaCreacion" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Crear tabla Personal
CREATE TABLE "Personal" (
    "IdTrabajador" SERIAL PRIMARY KEY,
    "Nombre" VARCHAR(50) NOT NULL,
    "ApPaterno" VARCHAR(50) NOT NULL,
    "ApMaterno" VARCHAR(50) NOT NULL,
    "Turno" VARCHAR(20),
    "Especialidad" VARCHAR(100),
    "Estado" VARCHAR(20) NOT NULL DEFAULT 'Activo',
    "FechaCreada" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Crear tabla Reporte
CREATE TABLE "Reporte" (
    "idReporte" SERIAL PRIMARY KEY,
    "idIncendio" INTEGER NOT NULL,
    "idUsuario" INTEGER NOT NULL,
    "Fecha" TIMESTAMP NOT NULL,
    "Tipo" VARCHAR(50) NOT NULL,
    "Contenido" TEXT NOT NULL,
    "Estado" VARCHAR(20) NOT NULL DEFAULT 'Activo',
    "FechaCreacion" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Crear tabla IncendioPersonal (tabla de relación)
CREATE TABLE "IncendioPersonal" (
    "idIncendio" INTEGER NOT NULL,
    "IdTrabajador" INTEGER NOT NULL,
    "FechaAsignacion" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "RolEnIncendio" VARCHAR(100),
    "Estado" VARCHAR(20) NOT NULL DEFAULT 'Activo',
    PRIMARY KEY ("idIncendio", "IdTrabajador")
);

-- Crear índices para mejor rendimiento
CREATE INDEX "IX_Usuarios_Usuario" ON "Usuarios" ("Usuario");
CREATE INDEX "IX_Usuarios_Estado" ON "Usuarios" ("Estado");
CREATE INDEX "IX_Etapas_Orden" ON "Etapas" ("Orden");
CREATE INDEX "IX_Incendio_idEtapa" ON "Incendio" ("idEtapa");
CREATE INDEX "IX_Incendio_idUsuarioResponsable" ON "Incendio" ("idUsuarioResponsable");
CREATE INDEX "IX_Incendio_Estado" ON "Incendio" ("Estado");
CREATE INDEX "IX_Personal_Estado" ON "Personal" ("Estado");
CREATE INDEX "IX_Reporte_idIncendio" ON "Reporte" ("idIncendio");
CREATE INDEX "IX_Reporte_idUsuario" ON "Reporte" ("idUsuario");
CREATE INDEX "IX_Reporte_Fecha" ON "Reporte" ("Fecha");

-- Crear Foreign Keys
ALTER TABLE "Incendio" 
ADD CONSTRAINT "FK_Incendio_Etapas" 
FOREIGN KEY ("idEtapa") REFERENCES "Etapas" ("idEtapa") ON DELETE RESTRICT;

ALTER TABLE "Incendio" 
ADD CONSTRAINT "FK_Incendio_Usuarios" 
FOREIGN KEY ("idUsuarioResponsable") REFERENCES "Usuarios" ("idUsuario") ON DELETE SET NULL;

ALTER TABLE "Reporte" 
ADD CONSTRAINT "FK_Reporte_Incendio" 
FOREIGN KEY ("idIncendio") REFERENCES "Incendio" ("idIncendio") ON DELETE CASCADE;

ALTER TABLE "Reporte" 
ADD CONSTRAINT "FK_Reporte_Usuarios" 
FOREIGN KEY ("idUsuario") REFERENCES "Usuarios" ("idUsuario") ON DELETE RESTRICT;

ALTER TABLE "IncendioPersonal" 
ADD CONSTRAINT "FK_IncendioPersonal_Incendio" 
FOREIGN KEY ("idIncendio") REFERENCES "Incendio" ("idIncendio") ON DELETE CASCADE;

ALTER TABLE "IncendioPersonal" 
ADD CONSTRAINT "FK_IncendioPersonal_Personal" 
FOREIGN KEY ("IdTrabajador") REFERENCES "Personal" ("IdTrabajador") ON DELETE RESTRICT;

-- Insertar datos de ejemplo para Etapas
INSERT INTO "Etapas" ("Nombre", "Descripcion", "Orden", "Color", "Icono") VALUES
('Detección', 'Primera detección del incendio', 1, '#ff4444', 'fire'),
('Evaluación', 'Evaluación inicial del incendio', 2, '#ff8800', 'search'),
('Respuesta', 'Respuesta inicial al incendio', 3, '#ffcc00', 'emergency'),
('Control', 'Control del incendio', 4, '#00cc00', 'shield'),
('Liquidación', 'Liquidación final del incendio', 5, '#0066cc', 'check'),
('Monitoreo', 'Monitoreo post-incendio', 6, '#6600cc', 'eye');

-- Insertar usuario administrador de ejemplo
INSERT INTO "Usuarios" ("Rol", "Nombre", "ApPaterno", "ApMaterno", "Usuario", "Contrasena", "TrabajoInicio", "TrabajoFin") VALUES
('Administrador', 'Admin', 'Sistema', 'Forestry', 'admin', 'admin123', '08:00:00', '18:00:00');

-- Comentarios sobre el esquema
COMMENT ON TABLE "Usuarios" IS 'Tabla de usuarios del sistema con roles y autenticación';
COMMENT ON TABLE "Etapas" IS 'Etapas del proceso de gestión de incendios';
COMMENT ON TABLE "Incendio" IS 'Registro de incendios forestales';
COMMENT ON TABLE "Personal" IS 'Personal disponible para asignación a incendios';
COMMENT ON TABLE "Reporte" IS 'Reportes relacionados con incendios';
COMMENT ON TABLE "IncendioPersonal" IS 'Tabla de relación muchos a muchos entre incendios y personal'; 