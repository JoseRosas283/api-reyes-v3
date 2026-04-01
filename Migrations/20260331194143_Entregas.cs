using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class Entregas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "claverepresentante",
                table: "Representantes",
                newName: "claveRepresentante");

            migrationBuilder.AlterColumn<string>(
                name: "claveRepresentante",
                table: "Representantes",
                type: "varchar(18)",
                nullable: false,
                defaultValueSql: "generar_clave_representante()",
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldDefaultValueSql: "generar_clave_representante()");

            migrationBuilder.CreateTable(
                name: "Entregas",
                columns: table => new
                {
                    claveEntrega = table.Column<string>(type: "varchar(18)", nullable: false, defaultValueSql: "generar_clave_entrega()"),
                    fecha_entrega = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    estado = table.Column<string>(type: "varchar(20)", nullable: false),
                    total = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    tipo_entrega = table.Column<string>(type: "varchar(50)", nullable: false),
                    clavePedido = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveUsuario = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveProveedor = table.Column<string>(type: "varchar(18)", nullable: false),
                    claveRepresentante = table.Column<string>(type: "varchar(18)", nullable: true),
                    RepresentanteEntityclaveRepresentante = table.Column<string>(type: "varchar(18)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entregas", x => x.claveEntrega);
                    table.ForeignKey(
                        name: "FK_Entregas_Pedidos_clavePedido",
                        column: x => x.clavePedido,
                        principalTable: "Pedidos",
                        principalColumn: "clavepedido",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entregas_Proveedores_claveProveedor",
                        column: x => x.claveProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "claveProveedor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entregas_Representantes_RepresentanteEntityclaveRepresentan~",
                        column: x => x.RepresentanteEntityclaveRepresentante,
                        principalTable: "Representantes",
                        principalColumn: "claveRepresentante");
                    table.ForeignKey(
                        name: "FK_Entregas_Representantes_claveRepresentante",
                        column: x => x.claveRepresentante,
                        principalTable: "Representantes",
                        principalColumn: "claveRepresentante",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Entregas_Usuarios_claveUsuario",
                        column: x => x.claveUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "claveUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_clavePedido",
                table: "Entregas",
                column: "clavePedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_claveProveedor",
                table: "Entregas",
                column: "claveProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_claveRepresentante",
                table: "Entregas",
                column: "claveRepresentante");

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_claveUsuario",
                table: "Entregas",
                column: "claveUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_RepresentanteEntityclaveRepresentante",
                table: "Entregas",
                column: "RepresentanteEntityclaveRepresentante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entregas");

            migrationBuilder.RenameColumn(
                name: "claveRepresentante",
                table: "Representantes",
                newName: "claverepresentante");

            migrationBuilder.AlterColumn<string>(
                name: "claverepresentante",
                table: "Representantes",
                type: "varchar(13)",
                nullable: false,
                defaultValueSql: "generar_clave_representante()",
                oldClrType: typeof(string),
                oldType: "varchar(18)",
                oldDefaultValueSql: "generar_clave_representante()");
        }
    }
}
