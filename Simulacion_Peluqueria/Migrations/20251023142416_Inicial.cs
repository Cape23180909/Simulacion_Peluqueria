using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simulacion_Peluqueria.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    TiempoLlegada = table.Column<double>(type: "REAL", nullable: false),
                    Atendido = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnCola = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuraciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Semilla = table.Column<int>(type: "INTEGER", nullable: false),
                    NumPeluqueros = table.Column<int>(type: "INTEGER", nullable: false),
                    TiempoCorteMin = table.Column<int>(type: "INTEGER", nullable: false),
                    TiempoCorteMax = table.Column<int>(type: "INTEGER", nullable: false),
                    TLlegadas = table.Column<int>(type: "INTEGER", nullable: false),
                    TotClientes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuraciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosSimulacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TiempoTotalSimulacion = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosSimulacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventosSimulacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cliente = table.Column<string>(type: "TEXT", nullable: false),
                    TipoEvento = table.Column<string>(type: "TEXT", nullable: false),
                    Tiempo = table.Column<double>(type: "REAL", nullable: false),
                    TiempoEspera = table.Column<double>(type: "REAL", nullable: true),
                    TiempoCorte = table.Column<double>(type: "REAL", nullable: true),
                    ResultadoSimulacionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosSimulacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventosSimulacion_ResultadosSimulacion_ResultadoSimulacionId",
                        column: x => x.ResultadoSimulacionId,
                        principalTable: "ResultadosSimulacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialSimulaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FechaEjecucion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Semilla = table.Column<int>(type: "INTEGER", nullable: false),
                    NumeroPeluqueros = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalClientes = table.Column<int>(type: "INTEGER", nullable: false),
                    LongitudPromedioCola = table.Column<double>(type: "REAL", nullable: false),
                    TiempoEsperaPromedio = table.Column<double>(type: "REAL", nullable: false),
                    UsoPromedioInstalacion = table.Column<double>(type: "REAL", nullable: false),
                    ConfiguracionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResultadoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialSimulaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialSimulaciones_Configuraciones_ConfiguracionId",
                        column: x => x.ConfiguracionId,
                        principalTable: "Configuraciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialSimulaciones_ResultadosSimulacion_ResultadoId",
                        column: x => x.ResultadoId,
                        principalTable: "ResultadosSimulacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Indicadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    LongitudPromedioCola = table.Column<double>(type: "REAL", nullable: false),
                    TiempoEsperaPromedio = table.Column<double>(type: "REAL", nullable: false),
                    UsoPromedioInstalacion = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Indicadores_ResultadosSimulacion_Id",
                        column: x => x.Id,
                        principalTable: "ResultadosSimulacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventosSimulacion_ResultadoSimulacionId",
                table: "EventosSimulacion",
                column: "ResultadoSimulacionId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialSimulaciones_ConfiguracionId",
                table: "HistorialSimulaciones",
                column: "ConfiguracionId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialSimulaciones_ResultadoId",
                table: "HistorialSimulaciones",
                column: "ResultadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "EventosSimulacion");

            migrationBuilder.DropTable(
                name: "HistorialSimulaciones");

            migrationBuilder.DropTable(
                name: "Indicadores");

            migrationBuilder.DropTable(
                name: "Configuraciones");

            migrationBuilder.DropTable(
                name: "ResultadosSimulacion");
        }
    }
}
