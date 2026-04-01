using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class ComprasColumnaDoble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Usuarios_UsuarioEntityclaveUsuario",
                table: "Compras");

            migrationBuilder.DropIndex(
                name: "IX_Compras_UsuarioEntityclaveUsuario",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "UsuarioEntityclaveUsuario",
                table: "Compras");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioEntityclaveUsuario",
                table: "Compras",
                type: "varchar(18)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compras_UsuarioEntityclaveUsuario",
                table: "Compras",
                column: "UsuarioEntityclaveUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Usuarios_UsuarioEntityclaveUsuario",
                table: "Compras",
                column: "UsuarioEntityclaveUsuario",
                principalTable: "Usuarios",
                principalColumn: "claveUsuario");
        }
    }
}
