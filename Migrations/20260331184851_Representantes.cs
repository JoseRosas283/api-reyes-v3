using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class Representantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Representantes",
                columns: table => new
                {
                    claverepresentante = table.Column<string>(type: "varchar(13)", nullable: false, defaultValueSql: "generar_clave_representante()"),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false),
                    apellido_paterno = table.Column<string>(type: "varchar(50)", nullable: false),
                    apellido_materno = table.Column<string>(type: "varchar(50)", nullable: false),
                    telefono = table.Column<string>(type: "varchar(15)", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    claveproveedor = table.Column<string>(type: "varchar(18)", nullable: false),
                    tipo_representante = table.Column<string>(type: "tipo_representante_enum", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Representantes", x => x.claverepresentante);
                    table.ForeignKey(
                        name: "FK_Representantes_Proveedores_claveproveedor",
                        column: x => x.claveproveedor,
                        principalTable: "Proveedores",
                        principalColumn: "claveProveedor",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Representantes_claveproveedor",
                table: "Representantes",
                column: "claveproveedor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Representantes");
        }
    }
}
