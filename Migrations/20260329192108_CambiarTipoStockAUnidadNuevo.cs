using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class CambiarTipoStockAUnidadNuevo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Crear columna temporal con el tipo correcto (tipo_valor_enum)
            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            ADD COLUMN tipo_stock_tmp tipo_valor_enum NOT NULL DEFAULT 'entero';
        ");

            // 2. Copiar valores existentes con cast explícito
            migrationBuilder.Sql(@"
            UPDATE ""UnidadCompras""
            SET tipo_stock_tmp = tipo_stock::text::tipo_valor_enum;
        ");

            // 3. Eliminar la columna vieja
            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            DROP COLUMN tipo_stock;
        ");

            // 4. Renombrar la columna temporal
            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            RENAME COLUMN tipo_stock_tmp TO tipo_stock;
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Si alguna vez quieres revertir, aquí podrías dejarlo como estaba antes
            // (por ejemplo, tipo_unidad_nuevo). Ajusta según tu necesidad real.
            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            ADD COLUMN tipo_stock_tmp tipo_unidad_nuevo NOT NULL DEFAULT 'entero';
        ");

            migrationBuilder.Sql(@"
            UPDATE ""UnidadCompras""
            SET tipo_stock_tmp = tipo_stock::text::tipo_unidad_nuevo;
        ");

            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            DROP COLUMN tipo_stock;
        ");

            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            RENAME COLUMN tipo_stock_tmp TO tipo_stock;
        ");
        }
    }


}
