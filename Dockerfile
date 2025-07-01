# ULTIMO CAMBIO FORZADO PARA RENDER - 2024-07-01 18:30
# Dockerfile para Forestry API - Render
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY Forestry_Server/Forestry.csproj ./
RUN dotnet restore "Forestry.csproj"

# Copiar todo el código fuente
COPY Forestry_Server/. ./
WORKDIR "/src"

# Build de la aplicación
RUN dotnet build "Forestry.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Forestry.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]

