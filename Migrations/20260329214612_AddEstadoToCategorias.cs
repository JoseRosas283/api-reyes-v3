using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class AddEstadoToCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tipo_stock",
                table: "UnidadCompras",
                type: "tipo_valor_enum",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "tipo_unidad_nuevo");

            migrationBuilder.AddColumn<bool>(
                name: "estado",
                table: "Categorias",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estado",
                table: "Categorias");

            migrationBuilder.AlterColumn<string>(
                name: "tipo_stock",
                table: "UnidadCompras",
                type: "tipo_unidad_nuevo",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "tipo_valor_enum");
        }
    }
}
