using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicinaESE.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAgendaYCitas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    IdCita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPaciente = table.Column<int>(type: "int", nullable: false),
                    PacienteIdPaciente = table.Column<int>(type: "int", nullable: true),
                    IdMedico = table.Column<int>(type: "int", nullable: false),
                    MedicoIdMedico = table.Column<int>(type: "int", nullable: true),
                    FechaCita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraCita = table.Column<TimeSpan>(type: "time", nullable: false),
                    EstadoCita = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Pendiente"),
                    MotivoConsulta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.IdCita);
                    table.ForeignKey(
                        name: "FK_Citas_Medicos_MedicoIdMedico",
                        column: x => x.MedicoIdMedico,
                        principalTable: "Medicos",
                        principalColumn: "IdMedico");
                    table.ForeignKey(
                        name: "FK_Citas_Pacientes_PacienteIdPaciente",
                        column: x => x.PacienteIdPaciente,
                        principalTable: "Pacientes",
                        principalColumn: "IdPaciente");
                });

            migrationBuilder.CreateTable(
                name: "ExcepcionesHorario",
                columns: table => new
                {
                    IdExcepcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMedico = table.Column<int>(type: "int", nullable: false),
                    MedicoIdMedico = table.Column<int>(type: "int", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicioEspecial = table.Column<TimeSpan>(type: "time", nullable: true),
                    HoraFinEspecial = table.Column<TimeSpan>(type: "time", nullable: true),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcepcionesHorario", x => x.IdExcepcion);
                    table.ForeignKey(
                        name: "FK_ExcepcionesHorario_Medicos_MedicoIdMedico",
                        column: x => x.MedicoIdMedico,
                        principalTable: "Medicos",
                        principalColumn: "IdMedico");
                });

            migrationBuilder.CreateTable(
                name: "HorariosGenerales",
                columns: table => new
                {
                    IdHorarioGeneral = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosGenerales", x => x.IdHorarioGeneral);
                });

            migrationBuilder.CreateTable(
                name: "HistorialClinico",
                columns: table => new
                {
                    IdHistorial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCita = table.Column<int>(type: "int", nullable: false),
                    Diagnostico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecetaMedica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotasAdicionales = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialClinico", x => x.IdHistorial);
                    table.ForeignKey(
                        name: "FK_HistorialClinico_Citas_IdCita",
                        column: x => x.IdCita,
                        principalTable: "Citas",
                        principalColumn: "IdCita",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsignacionHorarios",
                columns: table => new
                {
                    IdAsignacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMedico = table.Column<int>(type: "int", nullable: false),
                    MedicoIdMedico = table.Column<int>(type: "int", nullable: true),
                    IdHorarioGeneral = table.Column<int>(type: "int", nullable: false),
                    HorarioGeneralIdHorarioGeneral = table.Column<int>(type: "int", nullable: true),
                    FechaInicioSemana = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFinSemana = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionHorarios", x => x.IdAsignacion);
                    table.ForeignKey(
                        name: "FK_AsignacionHorarios_HorariosGenerales_HorarioGeneralIdHorarioGeneral",
                        column: x => x.HorarioGeneralIdHorarioGeneral,
                        principalTable: "HorariosGenerales",
                        principalColumn: "IdHorarioGeneral");
                    table.ForeignKey(
                        name: "FK_AsignacionHorarios_Medicos_MedicoIdMedico",
                        column: x => x.MedicoIdMedico,
                        principalTable: "Medicos",
                        principalColumn: "IdMedico");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionHorarios_HorarioGeneralIdHorarioGeneral",
                table: "AsignacionHorarios",
                column: "HorarioGeneralIdHorarioGeneral");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionHorarios_MedicoIdMedico",
                table: "AsignacionHorarios",
                column: "MedicoIdMedico");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_MedicoIdMedico",
                table: "Citas",
                column: "MedicoIdMedico");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_PacienteIdPaciente",
                table: "Citas",
                column: "PacienteIdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_ExcepcionesHorario_MedicoIdMedico",
                table: "ExcepcionesHorario",
                column: "MedicoIdMedico");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialClinico_IdCita",
                table: "HistorialClinico",
                column: "IdCita");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsignacionHorarios");

            migrationBuilder.DropTable(
                name: "ExcepcionesHorario");

            migrationBuilder.DropTable(
                name: "HistorialClinico");

            migrationBuilder.DropTable(
                name: "HorariosGenerales");

            migrationBuilder.DropTable(
                name: "Citas");
        }
    }
}
