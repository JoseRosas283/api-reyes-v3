using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class Compras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    ClaveCompra = table.Column<string>(type: "varchar(18)", nullable: false),
                    fecha_compra = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    tipo_compra = table.Column<string>(type: "varchar(20)", nullable: false),
                    total = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    forma_pago = table.Column<string>(type: "varchar(10)", nullable: true),
                    ClaveUsuario = table.Column<string>(type: "varchar(18)", nullable: true),
                    UsuarioEntityclaveUsuario = table.Column<string>(type: "varchar(18)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaveCompra", x => x.ClaveCompra);
                    table.ForeignKey(
                        name: "FK_Compras_Usuarios_ClaveUsuario",
                        column: x => x.ClaveUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "claveUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compras_Usuarios_UsuarioEntityclaveUsuario",
                        column: x => x.UsuarioEntityclaveUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "claveUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Compradirecta",
                columns: table => new
                {
                    claveCompra = table.Column<string>(type: "varchar(18)", nullable: false),
                    referencia_pago = table.Column<string>(type: "varchar(20)", nullable: true),
                    documento_referencia = table.Column<string>(type: "varchar", nullable: true),
                    origen = table.Column<string>(type: "varchar(20)", nullable: false),
                    estado = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraDirecta", x => x.claveCompra);
                    table.ForeignKey(
                        name: "FK_Compradirecta_Compras_claveCompra",
                        column: x => x.claveCompra,
                        principalTable: "Compras",
                        principalColumn: "ClaveCompra",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comprapedido",
                columns: table => new
                {
                    claveCompra = table.Column<string>(type: "varchar(18)", nullable: false),
                    folio = table.Column<string>(type: "varchar(18)", nullable: true),
                    factura_proveedor = table.Column<string>(type: "varchar", nullable: true),
                    claveEntrega = table.Column<string>(type: "varchar(18)", nullable: false),
                    estado_cumplimiento = table.Column<string>(type: "varchar(20)", nullable: false),
                    estado_operativo = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraPedido", x => x.claveCompra);
                    table.ForeignKey(
                        name: "FK_Comprapedido_Compras_claveCompra",
                        column: x => x.claveCompra,
                        principalTable: "Compras",
                        principalColumn: "ClaveCompra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comprapedido_Entregas_claveEntrega",
                        column: x => x.claveEntrega,
                        principalTable: "Entregas",
                        principalColumn: "claveEntrega",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comprapedido_claveEntrega",
                table: "Comprapedido",
                column: "claveEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_ClaveUsuario",
                table: "Compras",
                column: "ClaveUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_UsuarioEntityclaveUsuario",
                table: "Compras",
                column: "UsuarioEntityclaveUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compradirecta");

            migrationBuilder.DropTable(
                name: "Comprapedido");

            migrationBuilder.DropTable(
                name: "Compras");
        }
    }
}
