-- =====================================================
-- SCRIPT DE BASE DE DATOS - SISTEMA FORESTRY
-- Versión: 2.0 (Simplificada)
-- Descripción: Sistema de gestión de incendios forestales
-- =====================================================

-- Crear base de datos
CREATE DATABASE IF NOT EXISTS forestrydb;
USE forestrydb;

-- =====================================================
-- TABLA: Usuarios
-- Descripción: Usuarios del sistema con autenticación
-- =====================================================
CREATE TABLE Usuarios (
    idUsuario SERIAL PRIMARY KEY,
    Rol VARCHAR(20) NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Activo',
    Nombre VARCHAR(50) NOT NULL,
    ApPaterno VARCHAR(50) NOT NULL,
    ApMaterno VARCHAR(50) NOT NULL,
    NumeTel VARCHAR(20),
    Usuario VARCHAR(50) UNIQUE NOT NULL,
    Contrasena VARCHAR(256) NOT NULL,
    TrabajoInicio TIME,
    TrabajoFin TIME,
    FechaCreacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    DiasLaborales VARCHAR(100)
);

-- =====================================================
-- TABLA: Etapas
-- Descripción: Etapas del workflow de incendios
-- =====================================================
CREATE TABLE Etapas (
    idEtapa SERIAL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion TEXT,
    Orden INTEGER NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Activo',
    Color VARCHAR(7) DEFAULT '#007bff',
    Icono VARCHAR(50),
    FechaCreacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================
-- TABLA: Incendio
-- Descripción: Información principal de incendios
-- =====================================================
CREATE TABLE Incendio (
    idIncendio SERIAL PRIMARY KEY,
    FechaIni TIMESTAMP NOT NULL,
    FechaFin TIMESTAMP,
    idEtapa INTEGER NOT NULL REFERENCES Etapas(idEtapa),
    NombreDespacho VARCHAR(100),
    NombreComando VARCHAR(100),
    Ubicacion VARCHAR(200) NOT NULL,
    Descripcion TEXT,
    Estado VARCHAR(20) DEFAULT 'Activo',
    idUsuarioResponsable INTEGER REFERENCES Usuarios(idUsuario),
    FechaCreacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================
-- TABLA: Personal
-- Descripción: Personal de campo (trabajadores)
-- =====================================================
CREATE TABLE Personal (
    IdTrabajador SERIAL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    ApPaterno VARCHAR(50) NOT NULL,
    ApMaterno VARCHAR(50) NOT NULL,
    Turno VARCHAR(20),
    Especialidad VARCHAR(100),
    Estado VARCHAR(20) DEFAULT 'Activo',
    FechaCreada TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================
-- TABLA: Reporte
-- Descripción: Reportes de incidentes y seguimiento
-- =====================================================
CREATE TABLE Reporte (
    idReporte SERIAL PRIMARY KEY,
    idIncendio INTEGER REFERENCES Incendio(idIncendio),
    idUsuario INTEGER REFERENCES Usuarios(idUsuario),
    Fecha TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Tipo VARCHAR(50) NOT NULL,
    Contenido TEXT NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Activo',
    FechaCreacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Lugar VARCHAR(200),
    Situacion VARCHAR(200),
    Detalles VARCHAR(500)
);

-- =====================================================
-- TABLA: IncendioPersonal
-- Descripción: Relación muchos a muchos entre incendios y personal
-- =====================================================
CREATE TABLE IncendioPersonal (
    idIncendio INTEGER REFERENCES Incendio(idIncendio) ON DELETE CASCADE,
    IdTrabajador INTEGER REFERENCES Personal(IdTrabajador) ON DELETE RESTRICT,
    FechaAsignacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    RolEnIncendio VARCHAR(100),
    Estado VARCHAR(20) DEFAULT 'Activo',
    PRIMARY KEY (idIncendio, IdTrabajador)
);

-- =====================================================
-- TABLA: BitacoraMedidaInicial
-- Descripción: Bitácora técnica de evaluación inicial
-- =====================================================
CREATE TABLE BitacoraMedidaInicial (
    IdBitacora SERIAL PRIMARY KEY,
    FechaCreada TIMESTAMP NOT NULL,
    Pregunta1 TEXT NOT NULL,
    Pregunta2 TEXT NOT NULL,
    Peligros TEXT NOT NULL,
    PotencialExpansion TEXT NOT NULL,
    CaracterFuego TEXT NOT NULL,
    PendienteFuego TEXT NOT NULL,
    PosicionPendiente TEXT NOT NULL,
    TipoCombustible TEXT NOT NULL,
    CombustiblesAdyacentes TEXT NOT NULL,
    Aspecto TEXT NOT NULL,
    DireccionViento TEXT NOT NULL,
    IdIncendio INTEGER REFERENCES Incendio(idIncendio)
);

-- =====================================================
-- ÍNDICES PARA OPTIMIZACIÓN
-- =====================================================

-- Índices únicos
CREATE UNIQUE INDEX idx_usuarios_usuario ON Usuarios(Usuario);

-- Índices de rendimiento
CREATE INDEX idx_usuarios_estado ON Usuarios(Estado);
CREATE INDEX idx_etapas_orden ON Etapas(Orden);
CREATE INDEX idx_incendio_etapa ON Incendio(idEtapa);
CREATE INDEX idx_incendio_responsable ON Incendio(idUsuarioResponsable);
CREATE INDEX idx_incendio_estado ON Incendio(Estado);
CREATE INDEX idx_personal_estado ON Personal(Estado);
CREATE INDEX idx_reporte_incendio ON Reporte(idIncendio);
CREATE INDEX idx_reporte_usuario ON Reporte(idUsuario);
CREATE INDEX idx_reporte_fecha ON Reporte(Fecha);
CREATE INDEX idx_incendio_fecha_ini ON Incendio(FechaIni);

-- =====================================================
-- DATOS INICIALES
-- =====================================================

-- Insertar etapas básicas del workflow
INSERT INTO Etapas (Nombre, Descripcion, Orden, Color, Icono) VALUES
('Reportado', 'Incendio reportado y en espera de asignación', 1, '#ffc107', 'report'),
('En Atención', 'Incendio asignado y en proceso de atención', 2, '#dc3545', 'fire'),
('Controlado', 'Incendio controlado, monitoreando', 3, '#28a745', 'check'),
('Extinguido', 'Incendio completamente extinguido', 4, '#6c757d', 'done'),
('Cerrado', 'Incendio cerrado y documentado', 5, '#343a40', 'close');

-- Insertar usuario administrador por defecto
INSERT INTO Usuarios (Rol, Nombre, ApPaterno, ApMaterno, Usuario, Contrasena, Estado) VALUES
('Administrador', 'Admin', 'Sistema', '', 'admin', 'admin123', 'Activo');

-- Insertar personal de ejemplo
INSERT INTO Personal (Nombre, ApPaterno, ApMaterno, Turno, Especialidad, Estado) VALUES
('Juan', 'García', 'López', 'Diurno', 'Brigadista', 'Activo'),
('María', 'Rodríguez', 'Pérez', 'Nocturno', 'Conductor', 'Activo'),
('Carlos', 'Martínez', 'González', 'Diurno', 'Especialista', 'Activo');

-- =====================================================
-- VISTAS ÚTILES
-- =====================================================

-- Vista de incendios activos con información básica
CREATE VIEW v_incendios_activos AS
SELECT 
    i.idIncendio,
    i.Ubicacion,
    i.Descripcion,
    i.Estado,
    i.FechaIni,
    e.Nombre as EtapaNombre,
    e.Color as EtapaColor,
    CONCAT(u.Nombre, ' ', u.ApPaterno) as Responsable
FROM Incendio i
LEFT JOIN Etapas e ON i.idEtapa = e.idEtapa
LEFT JOIN Usuarios u ON i.idUsuarioResponsable = u.idUsuario
WHERE i.Estado = 'Activo'
ORDER BY i.FechaIni DESC;

-- Vista de personal asignado a incendios
CREATE VIEW v_personal_incendio AS
SELECT 
    ip.idIncendio,
    ip.IdTrabajador,
    CONCAT(p.Nombre, ' ', p.ApPaterno, ' ', p.ApMaterno) as NombreCompleto,
    p.Especialidad,
    ip.RolEnIncendio,
    ip.FechaAsignacion,
    ip.Estado
FROM IncendioPersonal ip
JOIN Personal p ON ip.IdTrabajador = p.IdTrabajador
WHERE ip.Estado = 'Activo';

-- Vista de reportes con información del incendio
CREATE VIEW v_reportes_incendio AS
SELECT 
    r.idReporte,
    r.Tipo,
    r.Contenido,
    r.Lugar,
    r.Fecha,
    r.Estado,
    i.Ubicacion as UbicacionIncendio,
    CONCAT(u.Nombre, ' ', u.ApPaterno) as UsuarioReporte
FROM Reporte r
LEFT JOIN Incendio i ON r.idIncendio = i.idIncendio
LEFT JOIN Usuarios u ON r.idUsuario = u.idUsuario
ORDER BY r.Fecha DESC;

-- =====================================================
-- FUNCIONES ÚTILES
-- =====================================================

-- Función para obtener estadísticas de incendios
CREATE OR REPLACE FUNCTION obtener_estadisticas_incendios()
RETURNS TABLE (
    total_incendios BIGINT,
    incendios_activos BIGINT,
    incendios_controlados BIGINT,
    incendios_extinguidos BIGINT
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        COUNT(*) as total_incendios,
        COUNT(*) FILTER (WHERE Estado = 'Activo') as incendios_activos,
        COUNT(*) FILTER (WHERE idEtapa = 3) as incendios_controlados,
        COUNT(*) FILTER (WHERE idEtapa = 4) as incendios_extinguidos
    FROM Incendio;
END;
$$ LANGUAGE plpgsql;

-- Función para cambiar etapa de incendio
CREATE OR REPLACE FUNCTION cambiar_etapa_incendio(
    p_id_incendio INTEGER,
    p_id_etapa INTEGER
)
RETURNS BOOLEAN AS $$
BEGIN
    UPDATE Incendio 
    SET idEtapa = p_id_etapa 
    WHERE idIncendio = p_id_incendio;
    
    RETURN FOUND;
END;
$$ LANGUAGE plpgsql;

-- =====================================================
-- TRIGGERS
-- =====================================================

-- Trigger para actualizar fecha de modificación
CREATE OR REPLACE FUNCTION actualizar_fecha_modificacion()
RETURNS TRIGGER AS $$
BEGIN
    NEW.FechaCreacion = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Aplicar trigger a tabla Incendio
CREATE TRIGGER trigger_actualizar_fecha_incendio
    BEFORE UPDATE ON Incendio
    FOR EACH ROW
    EXECUTE FUNCTION actualizar_fecha_modificacion();

-- =====================================================
-- PERMISOS Y ROLES
-- =====================================================

-- Crear roles de aplicación
CREATE ROLE forestry_app_read;
CREATE ROLE forestry_app_write;
CREATE ROLE forestry_app_admin;

-- Asignar permisos de lectura
GRANT SELECT ON ALL TABLES IN SCHEMA public TO forestry_app_read;
GRANT SELECT ON ALL SEQUENCES IN SCHEMA public TO forestry_app_read;

-- Asignar permisos de escritura
GRANT SELECT, INSERT, UPDATE ON ALL TABLES IN SCHEMA public TO forestry_app_write;
GRANT USAGE ON ALL SEQUENCES IN SCHEMA public TO forestry_app_write;

-- Asignar permisos de administración
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO forestry_app_admin;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO forestry_app_admin;

-- =====================================================
-- COMENTARIOS DE DOCUMENTACIÓN
-- =====================================================

COMMENT ON DATABASE forestrydb IS 'Base de datos del sistema de gestión de incendios forestales';
COMMENT ON TABLE Usuarios IS 'Usuarios del sistema con autenticación y roles';
COMMENT ON TABLE Etapas IS 'Etapas del workflow de gestión de incendios';
COMMENT ON TABLE Incendio IS 'Información principal de los incendios forestales';
COMMENT ON TABLE Personal IS 'Personal de campo y brigadistas';
COMMENT ON TABLE Reporte IS 'Reportes de incidentes y seguimiento';
COMMENT ON TABLE IncendioPersonal IS 'Relación entre incendios y personal asignado';
COMMENT ON TABLE BitacoraMedidaInicial IS 'Bitácora técnica de evaluación inicial';

-- =====================================================
-- FIN DEL SCRIPT
-- ===================================================== 