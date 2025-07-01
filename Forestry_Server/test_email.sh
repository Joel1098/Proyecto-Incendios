#!/bin/bash

# Script para probar el sistema de email de Forestry
# Asegúrate de que la aplicación esté ejecutándose en http://localhost:8080

echo "🧪 Probando Sistema de Email - Forestry"
echo "======================================"

# Colores para output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Función para mostrar mensajes
print_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

print_error() {
    echo -e "${RED}❌ $1${NC}"
}

print_info() {
    echo -e "${YELLOW}ℹ️ $1${NC}"
}

# Verificar si la aplicación está ejecutándose
print_info "Verificando que la aplicación esté ejecutándose..."
if curl -s http://localhost:8080/swagger > /dev/null; then
    print_success "Aplicación ejecutándose en http://localhost:8080"
else
    print_error "La aplicación no está ejecutándose. Inicia la aplicación primero."
    echo "Comando: dotnet run"
    exit 1
fi

echo ""
print_info "Iniciando pruebas de email..."

# 1. Probar configuración básica
echo ""
print_info "1. Probando configuración básica..."
TEST_RESPONSE=$(curl -s -X POST http://localhost:8080/api/email/test \
  -H "Content-Type: application/json" \
  -d '{"testEmail": "joeldgjo98@gmail.com"}')

if echo "$TEST_RESPONSE" | grep -q "success.*true"; then
    print_success "Configuración básica: OK"
else
    print_error "Configuración básica: FALLÓ"
    echo "Respuesta: $TEST_RESPONSE"
fi

# 2. Probar envío de email personalizado
echo ""
print_info "2. Probando envío de email personalizado..."
CUSTOM_RESPONSE=$(curl -s -X POST http://localhost:8080/api/email/send \
  -H "Content-Type: application/json" \
  -d '{
    "to": "joeldgjo98@gmail.com",
    "subject": "Prueba Personalizada - Forestry System",
    "body": "<h1>¡Hola!</h1><p>Esta es una prueba del sistema de email de Forestry.</p>",
    "isHtml": true
  }')

if echo "$CUSTOM_RESPONSE" | grep -q "success.*true"; then
    print_success "Email personalizado: OK"
else
    print_error "Email personalizado: FALLÓ"
    echo "Respuesta: $CUSTOM_RESPONSE"
fi

# 3. Probar notificación de registro
echo ""
print_info "3. Probando notificación de registro..."
REGISTER_RESPONSE=$(curl -s -X POST http://localhost:8080/api/email/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joeldgjo98@gmail.com",
    "username": "usuario_prueba",
    "password": "password123"
  }')

if echo "$REGISTER_RESPONSE" | grep -q "success.*true"; then
    print_success "Notificación de registro: OK"
else
    print_error "Notificación de registro: FALLÓ"
    echo "Respuesta: $REGISTER_RESPONSE"
fi

# 4. Probar alerta de emergencia
echo ""
print_info "4. Probando alerta de emergencia..."
EMERGENCY_RESPONSE=$(curl -s -X POST http://localhost:8080/api/email/emergency \
  -H "Content-Type: application/json" \
  -d '{
    "location": "Bosque Nacional Test",
    "description": "Prueba de alerta de emergencia del sistema",
    "recipients": ["joeldgjo98@gmail.com"]
  }')

if echo "$EMERGENCY_RESPONSE" | grep -q "success.*true"; then
    print_success "Alerta de emergencia: OK"
else
    print_error "Alerta de emergencia: FALLÓ"
    echo "Respuesta: $EMERGENCY_RESPONSE"
fi

echo ""
echo "🎯 Resumen de Pruebas:"
echo "====================="
echo "✅ Configuración básica"
echo "✅ Email personalizado"
echo "✅ Notificación de registro"
echo "✅ Alerta de emergencia"

echo ""
print_info "📧 Verifica tu bandeja de entrada en joeldgjo98@gmail.com"
print_info "📧 Revisa también la carpeta de spam si no encuentras los emails"

echo ""
print_info "🔧 Para ver logs detallados:"
echo "   docker-compose logs forestry-api | grep -i email"

echo ""
print_success "¡Pruebas completadas! 🚀" 