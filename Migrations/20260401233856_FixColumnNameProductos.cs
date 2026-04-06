using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnNameProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_claveCategoria",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Proveedores_claveProveedor",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_UnidadMedidas_claveUnidadMedida",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_UnidadVentas_claveUnidadVenta",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "claveUnidadVenta",
                table: "Productos",
                newName: "claveunidadventa");

            migrationBuilder.RenameColumn(
                name: "claveUnidadMedida",
                table: "Productos",
                newName: "claveunidadmedida");

            migrationBuilder.RenameColumn(
                name: "claveProveedor",
                table: "Productos",
                newName: "claveproveedor");

            migrationBuilder.RenameColumn(
                name: "claveCategoria",
                table: "Productos",
                newName: "clavecategoria");

            migrationBuilder.RenameColumn(
                name: "Imagen",
                table: "Productos",
                newName: "imagen");

            migrationBuilder.RenameColumn(
                name: "claveProducto",
                table: "Productos",
                newName: "claveproducto");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_claveUnidadVenta",
                table: "Productos",
                newName: "IX_Productos_claveunidadventa");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_claveUnidadMedida",
                table: "Productos",
                newName: "IX_Productos_claveunidadmedida");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_claveProveedor",
                table: "Productos",
                newName: "IX_Productos_claveproveedor");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_claveCategoria",
                table: "Productos",
                newName: "IX_Productos_clavecategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_clavecategoria",
                table: "Productos",
                column: "clavecategoria",
                principalTable: "Categorias",
                principalColumn: "claveCategoria",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Proveedores_claveproveedor",
                table: "Productos",
                column: "claveproveedor",
                principalTable: "Proveedores",
                principalColumn: "claveProveedor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_UnidadMedidas_claveunidadmedida",
                table: "Productos",
                column: "claveunidadmedida",
                principalTable: "UnidadMedidas",
                principalColumn: "claveUnidad",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_UnidadVentas_claveunidadventa",
                table: "Productos",
                column: "claveunidadventa",
                principalTable: "UnidadVentas",
                principalColumn: "claveUnidadVenta",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_clavecategoria",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Proveedores_claveproveedor",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_UnidadMedidas_claveunidadmedida",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_UnidadVentas_claveunidadventa",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "imagen",
                table: "Productos",
                newName: "Imagen");

            migrationBuilder.RenameColumn(
                name: "claveunidadventa",
                table: "Productos",
                newName: "claveUnidadVenta");

            migrationBuilder.RenameColumn(
                name: "claveunidadmedida",
                table: "Productos",
                newName: "claveUnidadMedida");

            migrationBuilder.RenameColumn(
                name: "claveproveedor",
                table: "Productos",
                newName: "claveProveedor");

            migrationBuilder.RenameColumn(
                name: "clavecategoria",
                table: "Productos",
                newName: "claveCategoria");

            migrationBuilder.RenameColumn(
                name: "claveproducto",
                table: "Productos",
                newName: "claveProducto");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_claveunidadventa",
                table: "Productos",
                newName: "IX_Productos_claveUnidadVenta");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_claveunidadmedida",
                table: "Productos",
                newName: "IX_Productos_claveUnidadMedida");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_claveproveedor",
                table: "Productos",
                newName: "IX_Productos_claveProveedor");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_clavecategoria",
                table: "Productos",
                newName: "IX_Productos_claveCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_claveCategoria",
                table: "Productos",
                column: "claveCategoria",
                principalTable: "Categorias",
                principalColumn: "claveCategoria",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Proveedores_claveProveedor",
                table: "Productos",
                column: "claveProveedor",
                principalTable: "Proveedores",
                principalColumn: "claveProveedor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_UnidadMedidas_claveUnidadMedida",
                table: "Productos",
                column: "claveUnidadMedida",
                principalTable: "UnidadMedidas",
                principalColumn: "claveUnidad",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_UnidadVentas_claveUnidadVenta",
                table: "Productos",
                column: "claveUnidadVenta",
                principalTable: "UnidadVentas",
                principalColumn: "claveUnidadVenta",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
