using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class DetallePedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetallePedido",
                columns: table => new
                {
                    Clavepedido = table.Column<string>(type: "varchar(18)", nullable: false),
                    Claveproducto = table.Column<string>(type: "varchar(18)", nullable: false),
                    cantidad = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    tipo_detalle = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallePedido", x => new { x.Clavepedido, x.Claveproducto });
                    table.ForeignKey(
                        name: "FK_DetallePedido_Pedidos_Clavepedido",
                        column: x => x.Clavepedido,
                        principalTable: "Pedidos",
                        principalColumn: "clavepedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallePedido_Productos_Claveproducto",
                        column: x => x.Claveproducto,
                        principalTable: "Productos",
                        principalColumn: "claveProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedido_Claveproducto",
                table: "DetallePedido",
                column: "Claveproducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallePedido");
        }
    }
}
