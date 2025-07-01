using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forestry.Migrations
{
    /// <inheritdoc />
    public partial class SimplificarReporte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reporte_Incendio_idIncendio",
                table: "Reporte");

            migrationBuilder.DropForeignKey(
                name: "FK_Reporte_Usuarios_idUsuario",
                table: "Reporte");

            migrationBuilder.DropIndex(
                name: "IX_Reporte_Fecha",
                table: "Reporte");

            migrationBuilder.DropIndex(
                name: "IX_Reporte_idIncendio",
                table: "Reporte");

            migrationBuilder.DropIndex(
                name: "IX_Reporte_idUsuario",
                table: "Reporte");

            migrationBuilder.DropColumn(
                name: "Contenido",
                table: "Reporte");

            migrationBuilder.DropColumn(
                name: "Detalles",
                table: "Reporte");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Reporte");

            migrationBuilder.DropColumn(
                name: "Situacion",
                table: "Reporte");

            migrationBuilder.DropColumn(
                name: "idIncendio",
                table: "Reporte");

            migrationBuilder.DropColumn(
                name: "idUsuario",
                table: "Reporte");

            migrationBuilder.AlterColumn<string>(
                name: "Lugar",
                table: "Reporte",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Reporte",
                type: "text",
                nullable: true,
                defaultValue: "Activo",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "Activo");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Reporte",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IncendioidIncendio",
                table: "Reporte",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioidUsuario",
                table: "Reporte",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reporte_IncendioidIncendio",
                table: "Reporte",
                column: "IncendioidIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_Reporte_UsuarioidUsuario",
                table: "Reporte",
                column: "UsuarioidUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Reporte_Incendio_IncendioidIncendio",
                table: "Reporte",
                column: "IncendioidIncendio",
                principalTable: "Incendio",
                principalColumn: "idIncendio");

            migrationBuilder.AddForeignKey(
                name: "FK_Reporte_Usuarios_UsuarioidUsuario",
                table: "Reporte",
                column: "UsuarioidUsuario",
                principalTable: "Usuarios",
                principalColumn: "idUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reporte_Incendio_IncendioidIncendio",
                table: "Reporte");

            migrationBuilder.DropForeignKey(
                name: "FK_Reporte_Usuarios_UsuarioidUsuario",
                table: "Reporte");

            migrationBuilder.DropIndex(
                name: "IX_Reporte_IncendioidIncendio",
                table: "Reporte");

            migrationBuilder.DropIndex(
                name: "IX_Reporte_UsuarioidUsuario",
                table: "Reporte");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Reporte");

            migrationBuilder.DropColumn(
                name: "IncendioidIncendio",
                table: "Reporte");

            migrationBuilder.DropColumn(
                name: "UsuarioidUsuario",
                table: "Reporte");

            migrationBuilder.AlterColumn<string>(
                name: "Lugar",
                table: "Reporte",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Reporte",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "Activo",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Activo");

            migrationBuilder.AddColumn<string>(
                name: "Contenido",
                table: "Reporte",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Detalles",
                table: "Reporte",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "Reporte",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Situacion",
                table: "Reporte",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "idIncendio",
                table: "Reporte",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "idUsuario",
                table: "Reporte",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reporte_Fecha",
                table: "Reporte",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Reporte_idIncendio",
                table: "Reporte",
                column: "idIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_Reporte_idUsuario",
                table: "Reporte",
                column: "idUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Reporte_Incendio_idIncendio",
                table: "Reporte",
                column: "idIncendio",
                principalTable: "Incendio",
                principalColumn: "idIncendio",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reporte_Usuarios_idUsuario",
                table: "Reporte",
                column: "idUsuario",
                principalTable: "Usuarios",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
