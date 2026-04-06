using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class Entradas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompraDirectaEntityclaveCompra",
                table: "DetalleCompraDirecta",
                type: "varchar(18)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquivalenciaUnidadEntityclaveProducto",
                table: "DetalleCompraDirecta",
                type: "varchar(18)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquivalenciaUnidadEntityclaveUnidadCompra",
                table: "DetalleCompraDirecta",
                type: "varchar(18)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductoEntityclaveProducto",
                table: "DetalleCompraDirecta",
                type: "varchar(18)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Entradas",
                columns: table => new
                {
                    claveEntrada = table.Column<string>(type: "varchar(18)", nullable: false, defaultValueSql: "generar_clave_entrada()"),
                    cantidad_recibida = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    fecha = table.Column<DateTime>(type: "date", nullable: false),
                    tipo_entrada = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    fecha_caducidad = table.Column<DateTime>(type: "date", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    extra = table.Column<bool>(type: "boolean", nullable: true),
                    claveCompra = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveProducto = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveUsuario = table.Column<string>(type: "varchar(18)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entradas", x => x.claveEntrada);
                    table.ForeignKey(
                        name: "FK_Entradas_Compras_claveCompra",
                        column: x => x.claveCompra,
                        principalTable: "Compras",
                        principalColumn: "ClaveCompra",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entradas_Productos_claveProducto",
                        column: x => x.claveProducto,
                        principalTable: "Productos",
                        principalColumn: "claveproducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entradas_Usuarios_claveUsuario",
                        column: x => x.claveUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "claveUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompraDirecta_CompraDirectaEntityclaveCompra",
                table: "DetalleCompraDirecta",
                column: "CompraDirectaEntityclaveCompra");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompraDirecta_EquivalenciaUnidadEntityclaveProducto_~",
                table: "DetalleCompraDirecta",
                columns: new[] { "EquivalenciaUnidadEntityclaveProducto", "EquivalenciaUnidadEntityclaveUnidadCompra" });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompraDirecta_ProductoEntityclaveProducto",
                table: "DetalleCompraDirecta",
                column: "ProductoEntityclaveProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Entradas_claveCompra",
                table: "Entradas",
                column: "claveCompra");

            migrationBuilder.CreateIndex(
                name: "IX_Entradas_claveProducto",
                table: "Entradas",
                column: "claveProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Entradas_claveUsuario",
                table: "Entradas",
                column: "claveUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleCompraDirecta_Compradirecta_CompraDirectaEntityclave~",
                table: "DetalleCompraDirecta",
                column: "CompraDirectaEntityclaveCompra",
                principalTable: "Compradirecta",
                principalColumn: "claveCompra");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleCompraDirecta_EquivalenciasUnidad_EquivalenciaUnidad~",
                table: "DetalleCompraDirecta",
                columns: new[] { "EquivalenciaUnidadEntityclaveProducto", "EquivalenciaUnidadEntityclaveUnidadCompra" },
                principalTable: "EquivalenciasUnidad",
                principalColumns: new[] { "claveProducto", "claveUnidadCompra" });

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleCompraDirecta_Productos_ProductoEntityclaveProducto",
                table: "DetalleCompraDirecta",
                column: "ProductoEntityclaveProducto",
                principalTable: "Productos",
                principalColumn: "claveproducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleCompraDirecta_Compradirecta_CompraDirectaEntityclave~",
                table: "DetalleCompraDirecta");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleCompraDirecta_EquivalenciasUnidad_EquivalenciaUnidad~",
                table: "DetalleCompraDirecta");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleCompraDirecta_Productos_ProductoEntityclaveProducto",
                table: "DetalleCompraDirecta");

            migrationBuilder.DropTable(
                name: "Entradas");

            migrationBuilder.DropIndex(
                name: "IX_DetalleCompraDirecta_CompraDirectaEntityclaveCompra",
                table: "DetalleCompraDirecta");

            migrationBuilder.DropIndex(
                name: "IX_DetalleCompraDirecta_EquivalenciaUnidadEntityclaveProducto_~",
                table: "DetalleCompraDirecta");

            migrationBuilder.DropIndex(
                name: "IX_DetalleCompraDirecta_ProductoEntityclaveProducto",
                table: "DetalleCompraDirecta");

            migrationBuilder.DropColumn(
                name: "CompraDirectaEntityclaveCompra",
                table: "DetalleCompraDirecta");

            migrationBuilder.DropColumn(
                name: "EquivalenciaUnidadEntityclaveProducto",
                table: "DetalleCompraDirecta");

            migrationBuilder.DropColumn(
                name: "EquivalenciaUnidadEntityclaveUnidadCompra",
                table: "DetalleCompraDirecta");

            migrationBuilder.DropColumn(
                name: "ProductoEntityclaveProducto",
                table: "DetalleCompraDirecta");
        }
    }
}
