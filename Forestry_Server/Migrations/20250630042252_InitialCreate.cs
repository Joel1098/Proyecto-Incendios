using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Forestry.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Etapas",
                columns: table => new
                {
                    idEtapa = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "Activo"),
                    Color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true, defaultValue: "#007bff"),
                    Icono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etapas", x => x.idEtapa);
                });

            migrationBuilder.CreateTable(
                name: "Personal",
                columns: table => new
                {
                    IdTrabajador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ApPaterno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ApMaterno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Turno = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Especialidad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "Activo"),
                    FechaCreada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personal", x => x.IdTrabajador);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Activo"),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ApPaterno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ApMaterno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumeTel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Usuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Contrasena = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TrabajoInicio = table.Column<TimeSpan>(type: "interval", nullable: true),
                    TrabajoFin = table.Column<TimeSpan>(type: "interval", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DiasLaborales = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Incendio",
                columns: table => new
                {
                    idIncendio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaIni = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    idEtapa = table.Column<int>(type: "integer", nullable: false),
                    NombreDespacho = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NombreComando = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Ubicacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "Activo"),
                    idUsuarioResponsable = table.Column<int>(type: "integer", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incendio", x => x.idIncendio);
                    table.ForeignKey(
                        name: "FK_Incendio_Etapas_idEtapa",
                        column: x => x.idEtapa,
                        principalTable: "Etapas",
                        principalColumn: "idEtapa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incendio_Usuarios_idUsuarioResponsable",
                        column: x => x.idUsuarioResponsable,
                        principalTable: "Usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BitacoraMedidaInicial",
                columns: table => new
                {
                    IdBitacora = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaCreada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Pregunta1 = table.Column<string>(type: "text", nullable: false),
                    Pregunta2 = table.Column<string>(type: "text", nullable: false),
                    Peligros = table.Column<string>(type: "text", nullable: false),
                    PotencialExpansion = table.Column<string>(type: "text", nullable: false),
                    CaracterFuego = table.Column<string>(type: "text", nullable: false),
                    PendienteFuego = table.Column<string>(type: "text", nullable: false),
                    PosicionPendiente = table.Column<string>(type: "text", nullable: false),
                    TipoCombustible = table.Column<string>(type: "text", nullable: false),
                    CombustiblesAdyacentes = table.Column<string>(type: "text", nullable: false),
                    Aspecto = table.Column<string>(type: "text", nullable: false),
                    DireccionViento = table.Column<string>(type: "text", nullable: false),
                    IdIncendio = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitacoraMedidaInicial", x => x.IdBitacora);
                    table.ForeignKey(
                        name: "FK_BitacoraMedidaInicial_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio");
                });

            migrationBuilder.CreateTable(
                name: "IncendioPersonal",
                columns: table => new
                {
                    idIncendio = table.Column<int>(type: "integer", nullable: false),
                    IdTrabajador = table.Column<int>(type: "integer", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RolEnIncendio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "Activo"),
                    IncendioidIncendio = table.Column<int>(type: "integer", nullable: true),
                    PersonalIdTrabajador = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncendioPersonal", x => new { x.idIncendio, x.IdTrabajador });
                    table.ForeignKey(
                        name: "FK_IncendioPersonal_Incendio_IncendioidIncendio",
                        column: x => x.IncendioidIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio");
                    table.ForeignKey(
                        name: "FK_IncendioPersonal_Incendio_idIncendio",
                        column: x => x.idIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncendioPersonal_Personal_IdTrabajador",
                        column: x => x.IdTrabajador,
                        principalTable: "Personal",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncendioPersonal_Personal_PersonalIdTrabajador",
                        column: x => x.PersonalIdTrabajador,
                        principalTable: "Personal",
                        principalColumn: "IdTrabajador");
                });

            migrationBuilder.CreateTable(
                name: "Reporte",
                columns: table => new
                {
                    idReporte = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idIncendio = table.Column<int>(type: "integer", nullable: false),
                    idUsuario = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Contenido = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "Activo"),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Lugar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Situacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Detalles = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporte", x => x.idReporte);
                    table.ForeignKey(
                        name: "FK_Reporte_Incendio_idIncendio",
                        column: x => x.idIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reporte_Usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraMedidaInicial_IdIncendio",
                table: "BitacoraMedidaInicial",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_Etapas_Orden",
                table: "Etapas",
                column: "Orden");

            migrationBuilder.CreateIndex(
                name: "IX_Incendio_Estado",
                table: "Incendio",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Incendio_idEtapa",
                table: "Incendio",
                column: "idEtapa");

            migrationBuilder.CreateIndex(
                name: "IX_Incendio_idUsuarioResponsable",
                table: "Incendio",
                column: "idUsuarioResponsable");

            migrationBuilder.CreateIndex(
                name: "IX_IncendioPersonal_IdTrabajador",
                table: "IncendioPersonal",
                column: "IdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_IncendioPersonal_IncendioidIncendio",
                table: "IncendioPersonal",
                column: "IncendioidIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_IncendioPersonal_PersonalIdTrabajador",
                table: "IncendioPersonal",
                column: "PersonalIdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_Personal_Estado",
                table: "Personal",
                column: "Estado");

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

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Estado",
                table: "Usuarios",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Usuario",
                table: "Usuarios",
                column: "Usuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BitacoraMedidaInicial");

            migrationBuilder.DropTable(
                name: "IncendioPersonal");

            migrationBuilder.DropTable(
                name: "Reporte");

            migrationBuilder.DropTable(
                name: "Personal");

            migrationBuilder.DropTable(
                name: "Incendio");

            migrationBuilder.DropTable(
                name: "Etapas");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
