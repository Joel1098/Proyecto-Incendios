using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forestry.Migrations
{
    public partial class DataBaseForestryV14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personal",
                columns: table => new
                {
                    IdTrabajador = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    ApPaterno = table.Column<string>(nullable: true),
                    ApMaterno = table.Column<string>(nullable: true),
                    Turno = table.Column<string>(nullable: true),
                    IdUsuarios = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personal", x => x.IdTrabajador);
                });

            migrationBuilder.CreateTable(
                name: "Reporte",
                columns: table => new
                {
                    idReporte = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(nullable: true),
                    Lugar = table.Column<string>(nullable: false),
                    Situacion = table.Column<string>(nullable: false),
                    Detalles = table.Column<string>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Estado = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporte", x => x.idReporte);
                });

            migrationBuilder.CreateTable(
                name: "Incendio",
                columns: table => new
                {
                    idIncendio = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaIni = table.Column<DateTime>(nullable: false),
                    FechaFin = table.Column<DateTime>(nullable: false),
                    Etapa = table.Column<int>(nullable: false),
                    NombreDespacho = table.Column<string>(nullable: false),
                    NombreComando = table.Column<string>(nullable: false),
                    idReporte = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incendio", x => x.idIncendio);
                    table.ForeignKey(
                        name: "FK_Incendio_Reporte_idReporte",
                        column: x => x.idReporte,
                        principalTable: "Reporte",
                        principalColumn: "idReporte",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "_Usuarios",
                columns: table => new
                {
                    idUsuario = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idIncendio = table.Column<int>(nullable: true),
                    Rol = table.Column<string>(maxLength: 20, nullable: false),
                    Estado = table.Column<string>(maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(maxLength: 20, nullable: false),
                    ApMaterno = table.Column<string>(maxLength: 20, nullable: false),
                    ApPaterno = table.Column<string>(maxLength: 20, nullable: false),
                    NumeTel = table.Column<string>(maxLength: 20, nullable: true),
                    Usuario = table.Column<string>(maxLength: 20, nullable: false),
                    Contrasena = table.Column<string>(maxLength: 256, nullable: false),
                    DiasLaborales = table.Column<string>(nullable: true),
                    TrabajoInicio = table.Column<TimeSpan>(nullable: false),
                    TrabajoFin = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios", x => x.idUsuario);
                    table.ForeignKey(
                        name: "FK__Usuarios_Incendio_idIncendio",
                        column: x => x.idIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Actualizacion",
                columns: table => new
                {
                    idActualizacion = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Radio = table.Column<float>(nullable: false),
                    Latitud = table.Column<decimal>(type: "decimal(18, 15)", nullable: false),
                    Longitud = table.Column<decimal>(type: "decimal(18, 15)", nullable: false),
                    FechaAccion = table.Column<DateTime>(nullable: false),
                    Accion = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(nullable: true),
                    idIncendio = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actualizacion", x => x.idActualizacion);
                    table.ForeignKey(
                        name: "FK_Actualizacion_Incendio_idIncendio",
                        column: x => x.idIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActualizacionRecursos",
                columns: table => new
                {
                    IdActRecursos = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    NombreRecurso = table.Column<string>(nullable: true),
                    NumSerie = table.Column<string>(nullable: true),
                    Estado = table.Column<string>(nullable: true),
                    Latitud = table.Column<decimal>(type: "decimal(18, 15)", nullable: false),
                    Longitud = table.Column<decimal>(type: "decimal(18, 15)", nullable: false),
                    IdIncendio = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActualizacionRecursos", x => x.IdActRecursos);
                    table.ForeignKey(
                        name: "FK_ActualizacionRecursos_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BitacoraChequeoYPlaneacion",
                columns: table => new
                {
                    IdBitacora = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    Objetivo2 = table.Column<string>(nullable: true),
                    Objetivo3 = table.Column<string>(nullable: true),
                    Objetivo4 = table.Column<string>(nullable: true),
                    Objetivo5 = table.Column<string>(nullable: true),
                    Pregunta1 = table.Column<string>(nullable: true),
                    Pregunta2 = table.Column<string>(nullable: true),
                    Pregunta3 = table.Column<string>(nullable: true),
                    Pregunta4 = table.Column<string>(nullable: true),
                    Pregunta5 = table.Column<string>(nullable: true),
                    Pregunta6 = table.Column<string>(nullable: true),
                    Pregunta7 = table.Column<string>(nullable: true),
                    IdIncendio = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitacoraChequeoYPlaneacion", x => x.IdBitacora);
                    table.ForeignKey(
                        name: "FK_BitacoraChequeoYPlaneacion_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BitacoraMedidaInicial",
                columns: table => new
                {
                    IdBitacora = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    Pregunta1 = table.Column<string>(nullable: false),
                    Pregunta2 = table.Column<string>(nullable: false),
                    Peligros = table.Column<string>(nullable: false),
                    PotencialExpansion = table.Column<string>(nullable: false),
                    CaracterFuego = table.Column<string>(nullable: false),
                    PendienteFuego = table.Column<string>(nullable: false),
                    PosicionPendiente = table.Column<string>(nullable: false),
                    TipoCombustible = table.Column<string>(nullable: false),
                    CombustiblesAdyacentes = table.Column<string>(nullable: false),
                    Aspecto = table.Column<string>(nullable: false),
                    DireccionViento = table.Column<string>(nullable: false),
                    IdIncendio = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitacoraMedidaInicial", x => x.IdBitacora);
                    table.ForeignKey(
                        name: "FK_BitacoraMedidaInicial_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BitacoraRecursos",
                columns: table => new
                {
                    IdBitacora = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    Recurso = table.Column<string>(nullable: true),
                    FechaInicio = table.Column<DateTime>(nullable: false),
                    FechaTermino = table.Column<DateTime>(nullable: false),
                    HorasTrabajadas = table.Column<double>(nullable: false),
                    HorasDescansadas = table.Column<double>(nullable: false),
                    IdIncendio = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitacoraRecursos", x => x.IdBitacora);
                    table.ForeignKey(
                        name: "FK_BitacoraRecursos_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BitacoraRelacionTrabajo",
                columns: table => new
                {
                    IdBitacora = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    Recurso = table.Column<string>(nullable: true),
                    FechaInicio = table.Column<DateTime>(nullable: false),
                    FechaTermino = table.Column<DateTime>(nullable: false),
                    HorasTrabajadas = table.Column<double>(nullable: false),
                    HorasDescansadas = table.Column<double>(nullable: false),
                    IdIncendio = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitacoraRelacionTrabajo", x => x.IdBitacora);
                    table.ForeignKey(
                        name: "FK_BitacoraRelacionTrabajo_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BitacoraRevisionPosterior",
                columns: table => new
                {
                    IdBitacora = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    CI = table.Column<string>(nullable: true),
                    Recursos = table.Column<string>(nullable: true),
                    CriticadoPor = table.Column<string>(nullable: true),
                    Comentarios = table.Column<string>(nullable: true),
                    NombreLider = table.Column<string>(nullable: true),
                    NombreRevisadoPor = table.Column<string>(nullable: true),
                    IdIncendio = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitacoraRevisionPosterior", x => x.IdBitacora);
                    table.ForeignKey(
                        name: "FK_BitacoraRevisionPosterior_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BitacoraStatus",
                columns: table => new
                {
                    IdBitacora = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    RecursosActual = table.Column<string>(nullable: true),
                    RecursosProyectado = table.Column<string>(nullable: true),
                    SituacionActual = table.Column<string>(nullable: true),
                    SituacionProyectada = table.Column<string>(nullable: true),
                    IdIncendio = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitacoraStatus", x => x.IdBitacora);
                    table.ForeignKey(
                        name: "FK_BitacoraStatus_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BitacoraTamanoIncendio",
                columns: table => new
                {
                    IdBitacora = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    USDA = table.Column<string>(nullable: true),
                    Estado = table.Column<string>(nullable: true),
                    DescripcionLocacion = table.Column<string>(nullable: true),
                    FechaLlegada = table.Column<DateTime>(nullable: false),
                    Legal = table.Column<string>(nullable: true),
                    Municipio = table.Column<string>(nullable: true),
                    Rango = table.Column<string>(nullable: true),
                    Secciones = table.Column<string>(nullable: true),
                    Latitud = table.Column<double>(nullable: false),
                    Longitud = table.Column<double>(nullable: false),
                    UTM = table.Column<string>(nullable: true),
                    E = table.Column<string>(nullable: true),
                    N = table.Column<string>(nullable: true),
                    ReportadoPor = table.Column<string>(nullable: true),
                    TamanoEstimado = table.Column<double>(nullable: false),
                    FechaContencion = table.Column<DateTime>(nullable: false),
                    FechaControl = table.Column<DateTime>(nullable: false),
                    IdIncendio = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitacoraTamanoIncendio", x => x.IdBitacora);
                    table.ForeignKey(
                        name: "FK_BitacoraTamanoIncendio_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BitacoraVerificacionCI",
                columns: table => new
                {
                    IdBitacora = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    Pregunta1 = table.Column<string>(nullable: true),
                    Pregunta2 = table.Column<string>(nullable: true),
                    Pregunta3 = table.Column<string>(nullable: true),
                    Pregunta4 = table.Column<string>(nullable: true),
                    Pregunta5 = table.Column<string>(nullable: true),
                    Pregunta6 = table.Column<string>(nullable: true),
                    Pregunta7 = table.Column<string>(nullable: true),
                    Pregunta8 = table.Column<string>(nullable: true),
                    Pregunta9 = table.Column<string>(nullable: true),
                    NombreFirma = table.Column<string>(nullable: true),
                    IdIncendio = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitacoraVerificacionCI", x => x.IdBitacora);
                    table.ForeignKey(
                        name: "FK_BitacoraVerificacionCI_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    IdRecursos = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    NombreRecurso = table.Column<string>(nullable: true),
                    NumSerie = table.Column<string>(nullable: true),
                    Estado = table.Column<string>(nullable: true),
                    Latitud = table.Column<decimal>(type: "decimal(18, 15)", nullable: false),
                    Longitud = table.Column<decimal>(type: "decimal(18, 15)", nullable: false),
                    IdIncendio = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.IdRecursos);
                    table.ForeignKey(
                        name: "FK_Recursos_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecursosSolicitados",
                columns: table => new
                {
                    IdRecursoSolicitado = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreada = table.Column<DateTime>(nullable: false),
                    NombreRecurso = table.Column<string>(nullable: true),
                    CantidadSolicitada = table.Column<int>(nullable: false),
                    IdIncendio = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursosSolicitados", x => x.IdRecursoSolicitado);
                    table.ForeignKey(
                        name: "FK_RecursosSolicitados_Incendio_IdIncendio",
                        column: x => x.IdIncendio,
                        principalTable: "Incendio",
                        principalColumn: "idIncendio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX__Usuarios_idIncendio",
                table: "_Usuarios",
                column: "idIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_Actualizacion_idIncendio",
                table: "Actualizacion",
                column: "idIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_ActualizacionRecursos_IdIncendio",
                table: "ActualizacionRecursos",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraChequeoYPlaneacion_IdIncendio",
                table: "BitacoraChequeoYPlaneacion",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraMedidaInicial_IdIncendio",
                table: "BitacoraMedidaInicial",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraRecursos_IdIncendio",
                table: "BitacoraRecursos",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraRelacionTrabajo_IdIncendio",
                table: "BitacoraRelacionTrabajo",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraRevisionPosterior_IdIncendio",
                table: "BitacoraRevisionPosterior",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraStatus_IdIncendio",
                table: "BitacoraStatus",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraTamanoIncendio_IdIncendio",
                table: "BitacoraTamanoIncendio",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraVerificacionCI_IdIncendio",
                table: "BitacoraVerificacionCI",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_Incendio_idReporte",
                table: "Incendio",
                column: "idReporte");

            migrationBuilder.CreateIndex(
                name: "IX_Personal_IdUsuarios",
                table: "Personal",
                column: "IdUsuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_IdIncendio",
                table: "Recursos",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosSolicitados_IdIncendio",
                table: "RecursosSolicitados",
                column: "IdIncendio");

            migrationBuilder.CreateIndex(
                name: "IX_Reporte_IdUsuario",
                table: "Reporte",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Personal__Usuarios_IdUsuarios",
                table: "Personal",
                column: "IdUsuarios",
                principalTable: "_Usuarios",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reporte__Usuarios_IdUsuario",
                table: "Reporte",
                column: "IdUsuario",
                principalTable: "_Usuarios",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Usuarios_Incendio_idIncendio",
                table: "_Usuarios");

            migrationBuilder.DropTable(
                name: "Actualizacion");

            migrationBuilder.DropTable(
                name: "ActualizacionRecursos");

            migrationBuilder.DropTable(
                name: "BitacoraChequeoYPlaneacion");

            migrationBuilder.DropTable(
                name: "BitacoraMedidaInicial");

            migrationBuilder.DropTable(
                name: "BitacoraRecursos");

            migrationBuilder.DropTable(
                name: "BitacoraRelacionTrabajo");

            migrationBuilder.DropTable(
                name: "BitacoraRevisionPosterior");

            migrationBuilder.DropTable(
                name: "BitacoraStatus");

            migrationBuilder.DropTable(
                name: "BitacoraTamanoIncendio");

            migrationBuilder.DropTable(
                name: "BitacoraVerificacionCI");

            migrationBuilder.DropTable(
                name: "Personal");

            migrationBuilder.DropTable(
                name: "Recursos");

            migrationBuilder.DropTable(
                name: "RecursosSolicitados");

            migrationBuilder.DropTable(
                name: "Incendio");

            migrationBuilder.DropTable(
                name: "Reporte");

            migrationBuilder.DropTable(
                name: "_Usuarios");
        }
    }
}
