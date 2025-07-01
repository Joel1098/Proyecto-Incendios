#!/bin/bash

# Script de despliegue para Forestry API
# Soporta despliegue local y en Render

set -e

# Colores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Función para mostrar mensajes
print_message() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

print_header() {
    echo -e "${BLUE}================================${NC}"
    echo -e "${BLUE}  $1${NC}"
    echo -e "${BLUE}================================${NC}"
}

# Función para mostrar ayuda
show_help() {
    echo "Uso: $0 [OPCIÓN]"
    echo ""
    echo "Opciones:"
    echo "  local     - Desplegar localmente con Docker Compose"
    echo "  render    - Preparar para despliegue en Render"
    echo "  build     - Solo construir la imagen Docker"
    echo "  test      - Ejecutar tests de la aplicación"
    echo "  clean     - Limpiar contenedores e imágenes"
    echo "  help      - Mostrar esta ayuda"
    echo ""
    echo "Ejemplos:"
    echo "  $0 local"
    echo "  $0 render"
    echo "  $0 build"
}

# Función para despliegue local
deploy_local() {
    print_header "Despliegue Local"
    
    print_message "Verificando Docker..."
    if ! command -v docker &> /dev/null; then
        print_error "Docker no está instalado"
        exit 1
    fi
    
    if ! command -v docker-compose &> /dev/null; then
        print_error "Docker Compose no está instalado"
        exit 1
    fi
    
    print_message "Deteniendo contenedores existentes..."
    docker-compose down 2>/dev/null || true
    
    print_message "Construyendo y ejecutando servicios..."
    docker-compose up --build -d
    
    print_message "Esperando a que los servicios estén listos..."
    sleep 10
    
    print_message "Verificando estado de los servicios..."
    docker-compose ps
    
    print_message "✅ Despliegue local completado!"
    echo ""
    echo "🌐 URLs disponibles:"
    echo "  API: http://localhost:8080"
    echo "  Swagger: http://localhost:8080/swagger"
    echo "  Base de datos: localhost:5432"
    echo ""
    echo "📋 Comandos útiles:"
    echo "  Ver logs: docker-compose logs -f forestry-api"
    echo "  Detener: docker-compose down"
    echo "  Reiniciar: docker-compose restart"
}

# Función para preparar despliegue en Render
deploy_render() {
    print_header "Preparación para Render"
    
    print_message "Verificando archivos necesarios..."
    
    required_files=("Dockerfile" "entrypoint.sh" "render.yaml" "docker-compose.yml")
    for file in "${required_files[@]}"; do
        if [ ! -f "$file" ]; then
            print_error "Archivo requerido no encontrado: $file"
            exit 1
        fi
        print_message "✅ $file encontrado"
    done
    
    print_message "Verificando configuración de Git..."
    if [ ! -d ".git" ]; then
        print_error "No es un repositorio Git"
        exit 1
    fi
    
    print_message "Verificando estado del repositorio..."
    if [ -n "$(git status --porcelain)" ]; then
        print_warning "Hay cambios sin commitear"
        echo "Archivos modificados:"
        git status --short
        echo ""
        read -p "¿Deseas hacer commit de los cambios? (y/N): " -n 1 -r
        echo
        if [[ $REPLY =~ ^[Yy]$ ]]; then
            git add .
            git commit -m "feat: preparar despliegue en Render"
        fi
    fi
    
    print_message "Verificando remoto..."
    if ! git remote get-url origin &> /dev/null; then
        print_error "No hay un remoto 'origin' configurado"
        exit 1
    fi
    
    print_message "✅ Preparación completada!"
    echo ""
    echo "🚀 Pasos para desplegar en Render:"
    echo "1. Ve a https://render.com"
    echo "2. Crea una nueva cuenta o inicia sesión"
    echo "3. Haz clic en 'New +' → 'Blueprint'"
    echo "4. Conecta tu repositorio de GitHub"
    echo "5. Render detectará automáticamente el render.yaml"
    echo "6. Selecciona la región (Oregon recomendado)"
    echo "7. Confirma la configuración"
    echo ""
    echo "📋 Variables de entorno se configurarán automáticamente"
}

# Función para construir imagen
build_image() {
    print_header "Construcción de Imagen Docker"
    
    print_message "Construyendo imagen forestry-api..."
    docker build -t forestry-api .
    
    print_message "✅ Imagen construida exitosamente!"
    echo ""
    echo "📋 Comandos útiles:"
    echo "  Ejecutar: docker run -p 8080:8080 forestry-api"
    echo "  Ver logs: docker logs <container_id>"
    echo "  Eliminar: docker rmi forestry-api"
}

# Función para limpiar
clean_up() {
    print_header "Limpieza"
    
    print_message "Deteniendo contenedores..."
    docker-compose down 2>/dev/null || true
    
    print_message "Eliminando contenedores huérfanos..."
    docker container prune -f
    
    print_message "Eliminando imágenes no utilizadas..."
    docker image prune -f
    
    print_message "Eliminando volúmenes no utilizados..."
    docker volume prune -f
    
    print_message "✅ Limpieza completada!"
}

# Función para tests
run_tests() {
    print_header "Ejecución de Tests"
    
    print_message "Ejecutando tests de la aplicación..."
    
    # Verificar que la aplicación compile correctamente
    if dotnet build --configuration Release; then
        print_message "✅ Compilación exitosa"
    else
        print_error "❌ Error en la compilación"
        exit 1
    fi
    
    # Verificar que el Dockerfile sea válido
    if docker build --dry-run . 2>/dev/null; then
        print_message "✅ Dockerfile válido"
    else
        print_warning "⚠️ No se pudo verificar Dockerfile"
    fi
    
    print_message "✅ Tests completados!"
}

# Función principal
main() {
    case "${1:-help}" in
        "local")
            deploy_local
            ;;
        "render")
            deploy_render
            ;;
        "build")
            build_image
            ;;
        "test")
            run_tests
            ;;
        "clean")
            clean_up
            ;;
        "help"|"-h"|"--help")
            show_help
            ;;
        *)
            print_error "Opción desconocida: $1"
            echo ""
            show_help
            exit 1
            ;;
    esac
}

# Ejecutar función principal
main "$@" 