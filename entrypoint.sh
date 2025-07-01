#!/bin/bash

# Script de entrada para Forestry API
# Ejecuta migraciones de base de datos y luego inicia la aplicación

set -e

echo "🚀 Iniciando Forestry API..."

# Función para esperar a que PostgreSQL esté disponible
wait_for_postgres() {
    echo "⏳ Esperando a que PostgreSQL esté disponible..."
    
    # Intentar conectarse a PostgreSQL hasta que esté disponible
    until pg_isready -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME; do
        echo "📡 PostgreSQL no está listo, esperando..."
        sleep 2
    done
    
    echo "✅ PostgreSQL está listo!"
}

# Función para ejecutar migraciones
run_migrations() {
    echo "🔄 Ejecutando migraciones de base de datos..."
    
    # Ejecutar migraciones
    dotnet ef database update --no-build
    
    echo "✅ Migraciones completadas"
}

# Función para crear base de datos si no existe
create_database() {
    echo "🗄️ Verificando base de datos..."
    
    # Intentar crear la base de datos si no existe
    createdb -h $DB_HOST -p $DB_PORT -U $DB_USER $DB_NAME 2>/dev/null || echo "📋 Base de datos ya existe o no se pudo crear"
}

# Configurar variables de entorno por defecto si no están definidas
export DB_HOST=${DB_HOST:-"localhost"}
export DB_PORT=${DB_PORT:-"5432"}
export DB_USER=${DB_USER:-"postgres"}
export DB_NAME=${DB_NAME:-"forestrydb"}
export DB_PASSWORD=${DB_PASSWORD:-"forestry123"}

# Configurar cadena de conexión
export ConnectionStrings__DefaultConnection="Host=$DB_HOST;Port=$DB_PORT;Database=$DB_NAME;Username=$DB_USER;Password=$DB_PASSWORD;SSL Mode=Prefer;Trust Server Certificate=true"

echo "🔧 Configuración de base de datos:"
echo "   Host: $DB_HOST"
echo "   Port: $DB_PORT"
echo "   Database: $DB_NAME"
echo "   User: $DB_USER"

# Esperar a que PostgreSQL esté disponible
wait_for_postgres

# Crear base de datos si es necesario
create_database

# Ejecutar migraciones
run_migrations

echo "🎯 Iniciando aplicación Forestry API..."
echo "📡 API disponible en: http://localhost:8080"
echo "📚 Swagger disponible en: http://localhost:8080/swagger"

# Iniciar la aplicación
exec dotnet Forestry.dll 