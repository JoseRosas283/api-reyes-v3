using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{

    public partial class CorregirUnidadCompraMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Crear columna temporal con el nuevo tipo
            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            ADD COLUMN abreviatura_tmp abreviatura_unidad_nuevo NOT NULL DEFAULT 'cj';
        ");

            // 2. Copiar valores existentes con cast explícito
            migrationBuilder.Sql(@"
            UPDATE ""UnidadCompras""
            SET abreviatura_tmp = abreviatura::text::abreviatura_unidad_nuevo;
        ");

            // 3. Eliminar la columna vieja
            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            DROP COLUMN abreviatura;
        ");

            // 4. Renombrar la columna temporal
            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            RENAME COLUMN abreviatura_tmp TO abreviatura;
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revertir al tipo anterior
            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            ADD COLUMN abreviatura_tmp abreviatura_unidad_enum NOT NULL DEFAULT 'cj';
        ");

            migrationBuilder.Sql(@"
            UPDATE ""UnidadCompras""
            SET abreviatura_tmp = abreviatura::text::abreviatura_unidad_enum;
        ");

            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            DROP COLUMN abreviatura;
        ");

            migrationBuilder.Sql(@"
            ALTER TABLE ""UnidadCompras""
            RENAME COLUMN abreviatura_tmp TO abreviatura;
        ");
        }
    }

}