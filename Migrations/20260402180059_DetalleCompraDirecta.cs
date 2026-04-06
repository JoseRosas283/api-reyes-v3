using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class DetalleCompraDirecta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetalleCompraDirecta",
                columns: table => new
                {
                    claveCompra = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false),
                    claveProducto = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false),
                    cantidad = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    precioUnitario = table.Column<double>(type: "double precision", nullable: false),
                    impuestos = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    subtotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false, computedColumnSql: "\"cantidad\" * \"precioUnitario\"", stored: true),
                    claveUnidadCompra = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleCompraDirecta", x => new { x.claveCompra, x.claveProducto });
                    table.ForeignKey(
                        name: "FK_DetalleCompraDirecta_Compradirecta_claveCompra",
                        column: x => x.claveCompra,
                        principalTable: "Compradirecta",
                        principalColumn: "claveCompra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleCompraDirecta_EquivalenciasUnidad_claveProducto_clav~",
                        columns: x => new { x.claveProducto, x.claveUnidadCompra },
                        principalTable: "EquivalenciasUnidad",
                        principalColumns: new[] { "claveProducto", "claveUnidadCompra" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleCompraDirecta_Productos_claveProducto",
                        column: x => x.claveProducto,
                        principalTable: "Productos",
                        principalColumn: "claveproducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompraDirecta_claveProducto_claveUnidadCompra",
                table: "DetalleCompraDirecta",
                columns: new[] { "claveProducto", "claveUnidadCompra" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleCompraDirecta");
        }
    }
}
