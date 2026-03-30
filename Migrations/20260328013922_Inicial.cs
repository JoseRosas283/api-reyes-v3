using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    claveDepartamento = table.Column<string>(type: "varchar(18)", nullable: false, defaultValueSql: "generar_clave_departamento()"),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaveDepartamento", x => x.claveDepartamento);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    claveProveedor = table.Column<string>(type: "varchar(18)", nullable: false),
                    direccion = table.Column<string>(type: "varchar(255)", nullable: false),
                    tipo_proveedor = table.Column<string>(type: "varchar(20)", nullable: false),
                    telefono = table.Column<string>(type: "varchar(15)", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaveProveedor", x => x.claveProveedor);
                });

            migrationBuilder.CreateTable(
                name: "UnidadCompras",
                columns: table => new
                {
                    claveUnidadCompra = table.Column<string>(type: "varchar(18)", nullable: false, defaultValueSql: "generar_clave_unidadCompra()"),
                    nombre_unidad = table.Column<string>(type: "tipo_unidad_nuevo", nullable: false),
                    abreviatura = table.Column<string>(type: "abreviatura_unidad_enum", nullable: false),
                    tipo_stock = table.Column<string>(type: "tipo_unidad_nuevo", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaveunidadCompra", x => x.claveUnidadCompra);
                });

            migrationBuilder.CreateTable(
                name: "UnidadMedidas",
                columns: table => new
                {
                    claveUnidad = table.Column<string>(type: "varchar(18)", nullable: false, defaultValueSql: "generar_clave_unidad()"),
                    nombreUnidad = table.Column<string>(type: "nombre_unidad_enum", nullable: false),
                    abreviatura = table.Column<string>(type: "abreviatura_unidad_enum", nullable: false),
                    tipoStock = table.Column<string>(type: "tipo_stock_enum", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaveUnidad", x => x.claveUnidad);
                });

            migrationBuilder.CreateTable(
                name: "UnidadVentas",
                columns: table => new
                {
                    claveUnidadVenta = table.Column<string>(type: "varchar(18)", nullable: false, defaultValueSql: "generar_clave_unidadVenta()"),
                    nombreUnidad = table.Column<string>(type: "nombre_unidad_venta_enum", nullable: false),
                    abreviatura = table.Column<string>(type: "abreviatura_unidad_venta_enum", nullable: false),
                    tipoValor = table.Column<string>(type: "tipo_valor_enum", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaveUnidadVenta", x => x.claveUnidadVenta);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    claveCategoria = table.Column<string>(type: "varchar(18)", nullable: false, defaultValueSql: "generar_clave_Categoria()"),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    claveDepartamento = table.Column<string>(type: "varchar(18)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaveCategoria", x => x.claveCategoria);
                    table.ForeignKey(
                        name: "FK_Categorias_Departamentos_claveDepartamento",
                        column: x => x.claveDepartamento,
                        principalTable: "Departamentos",
                        principalColumn: "claveDepartamento",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    claveProveedor = table.Column<string>(type: "varchar(18)", nullable: false),
                    razonSocial = table.Column<string>(type: "varchar(150)", nullable: false),
                    rfc = table.Column<string>(type: "varchar(13)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.claveProveedor);
                    table.ForeignKey(
                        name: "FK_Empresas_Proveedores_claveProveedor",
                        column: x => x.claveProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "claveProveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Independientes",
                columns: table => new
                {
                    claveProveedor = table.Column<string>(type: "varchar(18)", nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                    apellidoPaterno = table.Column<string>(type: "varchar(100)", nullable: false),
                    apellidoMaterno = table.Column<string>(type: "varchar(100)", nullable: false),
                    tipoServicio = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Independientes", x => x.claveProveedor);
                    table.ForeignKey(
                        name: "FK_Independientes_Proveedores_claveProveedor",
                        column: x => x.claveProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "claveProveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    claveProducto = table.Column<string>(type: "varchar(18)", nullable: false, defaultValueSql: "generar_clave_producto()"),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    precio = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    margen_ganancia = table.Column<decimal>(type: "numeric(5,2)", nullable: false, defaultValue: 1.30m),
                    precio_venta = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    codigo_barras = table.Column<string>(type: "varchar(13)", nullable: false),
                    tipo_producto = table.Column<string>(type: "varchar(15)", nullable: false),
                    cantidad = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    caducidad = table.Column<bool>(type: "boolean", nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Imagen = table.Column<string>(type: "varchar", nullable: false),
                    claveCategoria = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveUnidadMedida = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveUnidadVenta = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveProveedor = table.Column<string>(type: "varchar(18)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaveProducto", x => x.claveProducto);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_claveCategoria",
                        column: x => x.claveCategoria,
                        principalTable: "Categorias",
                        principalColumn: "claveCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Productos_Proveedores_claveProveedor",
                        column: x => x.claveProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "claveProveedor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Productos_UnidadMedidas_claveUnidadMedida",
                        column: x => x.claveUnidadMedida,
                        principalTable: "UnidadMedidas",
                        principalColumn: "claveUnidad",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Productos_UnidadVentas_claveUnidadVenta",
                        column: x => x.claveUnidadVenta,
                        principalTable: "UnidadVentas",
                        principalColumn: "claveUnidadVenta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_claveDepartamento",
                table: "Categorias",
                column: "claveDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_claveCategoria",
                table: "Productos",
                column: "claveCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_claveProveedor",
                table: "Productos",
                column: "claveProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_claveUnidadMedida",
                table: "Productos",
                column: "claveUnidadMedida");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_claveUnidadVenta",
                table: "Productos",
                column: "claveUnidadVenta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Independientes");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "UnidadCompras");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "UnidadMedidas");

            migrationBuilder.DropTable(
                name: "UnidadVentas");

            migrationBuilder.DropTable(
                name: "Departamentos");
        }
    }
}
