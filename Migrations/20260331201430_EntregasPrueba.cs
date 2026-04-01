using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReyesAR.Migrations
{
    /// <inheritdoc />
    public partial class EntregasPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entregas_Representantes_RepresentanteEntityclaveRepresentan~",
                table: "Entregas");

            migrationBuilder.DropForeignKey(
                name: "FK_Entregas_Representantes_claveRepresentante",
                table: "Entregas");

            migrationBuilder.DropIndex(
                name: "IX_Entregas_RepresentanteEntityclaveRepresentante",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "RepresentanteEntityclaveRepresentante",
                table: "Entregas");

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_Representantes",
                table: "Entregas",
                column: "claveRepresentante",
                principalTable: "Representantes",
                principalColumn: "claveRepresentante",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entregas_Representantes",
                table: "Entregas");

            migrationBuilder.AddColumn<string>(
                name: "RepresentanteEntityclaveRepresentante",
                table: "Entregas",
                type: "varchar(18)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_RepresentanteEntityclaveRepresentante",
                table: "Entregas",
                column: "RepresentanteEntityclaveRepresentante");

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_Representantes_RepresentanteEntityclaveRepresentan~",
                table: "Entregas",
                column: "RepresentanteEntityclaveRepresentante",
                principalTable: "Representantes",
                principalColumn: "claveRepresentante");

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_Representantes_claveRepresentante",
                table: "Entregas",
                column: "claveRepresentante",
                principalTable: "Representantes",
                principalColumn: "claveRepresentante",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
