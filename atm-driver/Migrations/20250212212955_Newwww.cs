using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class Newwww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Codigos_Evento_codigo_evento_id1",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_codigo_evento_id1",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "codigo_evento_id1",
                table: "Eventos",
                newName: "codigo_evento_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "codigo_evento_id",
                table: "Eventos",
                newName: "codigo_evento_id1");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_codigo_evento_id1",
                table: "Eventos",
                column: "codigo_evento_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Codigos_Evento_codigo_evento_id1",
                table: "Eventos",
                column: "codigo_evento_id1",
                principalTable: "Codigos_Evento",
                principalColumn: "codigo_evento_id");
        }
    }
}
