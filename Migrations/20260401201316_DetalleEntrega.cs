using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class DetalleEntrega : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetalleEntrega",
                columns: table => new
                {
                    claveEntrega = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveProducto = table.Column<string>(type: "varchar(18)", nullable: false),
                    tipo_detalle = table.Column<string>(type: "varchar(20)", nullable: false),
                    extra = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    claveCompra = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveUnidadCompra = table.Column<string>(type: "varchar(18)", nullable: false),
                    cantidad = table.Column<decimal>(type: "numeric", nullable: false),
                    precioUnitario = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    observaciones = table.Column<string>(type: "varchar(150)", nullable: true),
                    EquivalenciaUnidadEntityclaveProducto = table.Column<string>(type: "varchar(18)", nullable: true),
                    EquivalenciaUnidadEntityclaveUnidadCompra = table.Column<string>(type: "varchar(18)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleEntrega", x => new { x.claveEntrega, x.claveProducto, x.tipo_detalle, x.extra });
                    table.ForeignKey(
                        name: "FK_DetalleEntrega_EquivalenciasUnidad_EquivalenciaUnidadEntity~",
                        columns: x => new { x.EquivalenciaUnidadEntityclaveProducto, x.EquivalenciaUnidadEntityclaveUnidadCompra },
                        principalTable: "EquivalenciasUnidad",
                        principalColumns: new[] { "claveProducto", "claveUnidadCompra" });
                    table.ForeignKey(
                        name: "FK_DetalleEntrega_Productos_claveProducto",
                        column: x => x.claveProducto,
                        principalTable: "Productos",
                        principalColumn: "claveProducto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_detalle_entrega_compra",
                        column: x => x.claveCompra,
                        principalTable: "Comprapedido",
                        principalColumn: "claveCompra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_detalle_entrega_padre",
                        column: x => x.claveEntrega,
                        principalTable: "Entregas",
                        principalColumn: "claveEntrega",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_entrega_unidad_equivalencia",
                        columns: x => new { x.claveProducto, x.claveUnidadCompra },
                        principalTable: "EquivalenciasUnidad",
                        principalColumns: new[] { "claveProducto", "claveUnidadCompra" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleEntrega_claveCompra",
                table: "DetalleEntrega",
                column: "claveCompra");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleEntrega_claveProducto_claveUnidadCompra",
                table: "DetalleEntrega",
                columns: new[] { "claveProducto", "claveUnidadCompra" });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleEntrega_EquivalenciaUnidadEntityclaveProducto_Equiva~",
                table: "DetalleEntrega",
                columns: new[] { "EquivalenciaUnidadEntityclaveProducto", "EquivalenciaUnidadEntityclaveUnidadCompra" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleEntrega");
        }
    }
}
