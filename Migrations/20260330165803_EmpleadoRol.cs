using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class EmpleadoRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmpleadoRol",
                columns: table => new
                {
                    claveEmpleado = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveRol = table.Column<string>(type: "varchar(18)", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    fecha_fin = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadoRol", x => new { x.claveEmpleado, x.claveRol, x.fecha_inicio });
                    table.ForeignKey(
                        name: "FK_EmpleadoRol_Empleados_claveEmpleado",
                        column: x => x.claveEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "claveEmpleado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpleadoRol_Roles_claveRol",
                        column: x => x.claveRol,
                        principalTable: "Roles",
                        principalColumn: "claveRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadoRol_claveRol",
                table: "EmpleadoRol",
                column: "claveRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpleadoRol");
        }
    }
}
