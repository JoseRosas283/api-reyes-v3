using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class Empleados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    claveEmpleado = table.Column<string>(type: "varchar(18)", nullable: false, defaultValueSql: "generar_clave_empleado()"),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false),
                    apellido_paterno = table.Column<string>(type: "varchar(50)", nullable: false),
                    apellido_materno = table.Column<string>(type: "varchar(50)", nullable: false),
                    telefono = table.Column<string>(type: "varchar(15)", nullable: false),
                    curp = table.Column<string>(type: "varchar(18)", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.claveEmpleado);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_curp",
                table: "Empleados",
                column: "curp",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empleados");
        }
    }
}
