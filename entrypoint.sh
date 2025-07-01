#!/bin/sh

# Esperar a que la base de datos esté disponible
if [ -n "$DB_HOST" ]; then
  echo "Esperando a la base de datos en $DB_HOST:$DB_PORT..."
  until nc -z "$DB_HOST" "$DB_PORT"; do
    sleep 1
  done
    echo "Base de datos disponible!"
fi

# Ejecutar migraciones
if [ -f "/app/Forestry_Server.dll" ]; then
  echo "Ejecutando migraciones..."
  dotnet ef database update --no-build --project /app/Forestry_Server.csproj --startup-project /app/Forestry_Server.csproj
fi

# Iniciar la aplicación
exec dotnet /app/Forestry_Server.dll
