# Dockerfile para Forestry API
# Multi-stage build para optimizar el tamaño de la imagen

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["Forestry.csproj", "./"]
RUN dotnet restore "Forestry.csproj"

# Copiar todo el código fuente
COPY . .
WORKDIR "/src"

# Build de la aplicación
RUN dotnet build "Forestry.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Forestry.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Instalar herramientas de PostgreSQL para migraciones
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

# Copiar aplicación publicada
COPY --from=publish /app/publish .

# Exponer puerto
EXPOSE 8080

# Variables de entorno por defecto
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Script de inicio que ejecuta migraciones y luego la aplicación
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"] 