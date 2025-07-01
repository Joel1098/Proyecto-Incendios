using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forestry.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyIncendioModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altitud",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "CausaIncendio",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "CodigoEmergencia",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "CondicionesClimaticas",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "CoordenadasGPS",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "CostoEstimado",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "DireccionViento",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "DistanciaPoblacion",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "FuentesAgua",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "HumedadRelativa",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "InfraestructuraAfectada",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "Moneda",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "PoblacionRiesgo",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "PorcentajeControl",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "PropietarioTerreno",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "PuntoAcceso",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "RequiereInvestigacion",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "Severidad",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "SuperficieAfectada",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "SuperficieControlada",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "Temperatura",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "TipoIncendio",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "TipoVegetacion",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "UltimaActualizacion",
                table: "Incendio");

            migrationBuilder.DropColumn(
                name: "VelocidadViento",
                table: "Incendio");

            migrationBuilder.AlterColumn<string>(
                name: "Ubicacion",
                table: "Incendio",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ubicacion",
                table: "Incendio",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "Altitud",
                table: "Incendio",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CausaIncendio",
                table: "Incendio",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoEmergencia",
                table: "Incendio",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CondicionesClimaticas",
                table: "Incendio",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordenadasGPS",
                table: "Incendio",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoEstimado",
                table: "Incendio",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionViento",
                table: "Incendio",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DistanciaPoblacion",
                table: "Incendio",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuentesAgua",
                table: "Incendio",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HumedadRelativa",
                table: "Incendio",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InfraestructuraAfectada",
                table: "Incendio",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Moneda",
                table: "Incendio",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Incendio",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PoblacionRiesgo",
                table: "Incendio",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PorcentajeControl",
                table: "Incendio",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropietarioTerreno",
                table: "Incendio",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PuntoAcceso",
                table: "Incendio",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiereInvestigacion",
                table: "Incendio",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Severidad",
                table: "Incendio",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SuperficieAfectada",
                table: "Incendio",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SuperficieControlada",
                table: "Incendio",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Temperatura",
                table: "Incendio",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoIncendio",
                table: "Incendio",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoVegetacion",
                table: "Incendio",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaActualizacion",
                table: "Incendio",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VelocidadViento",
                table: "Incendio",
                type: "numeric",
                nullable: true);
        }
    }
}
