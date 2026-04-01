using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class DetalleEntregaColumna : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleEntrega_EquivalenciasUnidad_EquivalenciaUnidadEntity~",
                table: "DetalleEntrega");

            migrationBuilder.DropIndex(
                name: "IX_DetalleEntrega_EquivalenciaUnidadEntityclaveProducto_Equiva~",
                table: "DetalleEntrega");

            migrationBuilder.DropColumn(
                name: "EquivalenciaUnidadEntityclaveProducto",
                table: "DetalleEntrega");

            migrationBuilder.DropColumn(
                name: "EquivalenciaUnidadEntityclaveUnidadCompra",
                table: "DetalleEntrega");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EquivalenciaUnidadEntityclaveProducto",
                table: "DetalleEntrega",
                type: "varchar(18)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquivalenciaUnidadEntityclaveUnidadCompra",
                table: "DetalleEntrega",
                type: "varchar(18)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleEntrega_EquivalenciaUnidadEntityclaveProducto_Equiva~",
                table: "DetalleEntrega",
                columns: new[] { "EquivalenciaUnidadEntityclaveProducto", "EquivalenciaUnidadEntityclaveUnidadCompra" });

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleEntrega_EquivalenciasUnidad_EquivalenciaUnidadEntity~",
                table: "DetalleEntrega",
                columns: new[] { "EquivalenciaUnidadEntityclaveProducto", "EquivalenciaUnidadEntityclaveUnidadCompra" },
                principalTable: "EquivalenciasUnidad",
                principalColumns: new[] { "claveProducto", "claveUnidadCompra" });
        }
    }
}
