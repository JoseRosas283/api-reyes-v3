using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class Pedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquivalenciasUnidad",
                columns: table => new
                {
                    claveProducto = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveUnidadCompra = table.Column<string>(type: "varchar(18)", nullable: false),
                    factor_conversion = table.Column<decimal>(type: "numeric", nullable: false),
                    unidad_base = table.Column<string>(type: "nombre_unidad_enum", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquivalenciasUnidad", x => new { x.claveProducto, x.claveUnidadCompra });
                    table.ForeignKey(
                        name: "FK_EquivalenciasUnidad_Productos_claveProducto",
                        column: x => x.claveProducto,
                        principalTable: "Productos",
                        principalColumn: "claveProducto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquivalenciasUnidad_UnidadCompras_claveUnidadCompra",
                        column: x => x.claveUnidadCompra,
                        principalTable: "UnidadCompras",
                        principalColumn: "claveUnidadCompra",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquivalenciasUnidadVentas",
                columns: table => new
                {
                    claveProducto = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveUnidadVenta = table.Column<string>(type: "varchar(18)", nullable: false),
                    factor_conversion = table.Column<decimal>(type: "numeric", nullable: false),
                    unidad_base = table.Column<string>(type: "nombre_unidad_enum", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquivalenciasUnidadVentas", x => new { x.claveProducto, x.claveUnidadVenta });
                    table.ForeignKey(
                        name: "FK_EquivalenciasUnidadVentas_Productos_claveProducto",
                        column: x => x.claveProducto,
                        principalTable: "Productos",
                        principalColumn: "claveProducto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquivalenciasUnidadVentas_UnidadVentas_claveUnidadVenta",
                        column: x => x.claveUnidadVenta,
                        principalTable: "UnidadVentas",
                        principalColumn: "claveUnidadVenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    claveUsuario = table.Column<string>(type: "varchar(18)", nullable: false, defaultValueSql: "generar_clave_usuario()"),
                    nombre_usuario = table.Column<string>(type: "varchar(50)", nullable: false),
                    contrasena = table.Column<string>(type: "varchar(250)", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    claveEmpleado = table.Column<string>(type: "varchar(18)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.claveUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Empleados_claveEmpleado",
                        column: x => x.claveEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "claveEmpleado",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    clavepedido = table.Column<string>(type: "varchar(18)", nullable: false, defaultValueSql: "generar_clave_pedido()"),
                    fecha_pedido = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    estado = table.Column<string>(type: "varchar(20)", nullable: false),
                    observaciones = table.Column<string>(type: "varchar(150)", nullable: false),
                    claveusuario = table.Column<string>(type: "varchar(18)", nullable: false),
                    tipo_pedido = table.Column<string>(type: "varchar(20)", nullable: false),
                    claveproveedor = table.Column<string>(type: "varchar(18)", nullable: false),
                    total = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.clavepedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Proveedores_claveproveedor",
                        column: x => x.claveproveedor,
                        principalTable: "Proveedores",
                        principalColumn: "claveProveedor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_claveusuario",
                        column: x => x.claveusuario,
                        principalTable: "Usuarios",
                        principalColumn: "claveUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquivalenciasUnidad_claveUnidadCompra",
                table: "EquivalenciasUnidad",
                column: "claveUnidadCompra");

            migrationBuilder.CreateIndex(
                name: "IX_EquivalenciasUnidadVentas_claveUnidadVenta",
                table: "EquivalenciasUnidadVentas",
                column: "claveUnidadVenta");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_claveproveedor",
                table: "Pedidos",
                column: "claveproveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_claveusuario",
                table: "Pedidos",
                column: "claveusuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_claveEmpleado",
                table: "Usuarios",
                column: "claveEmpleado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_nombre_usuario",
                table: "Usuarios",
                column: "nombre_usuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquivalenciasUnidad");

            migrationBuilder.DropTable(
                name: "EquivalenciasUnidadVentas");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
