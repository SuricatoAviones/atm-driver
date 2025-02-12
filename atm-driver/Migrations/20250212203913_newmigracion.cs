using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class newmigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Tipo_Eventos_tipo_evento_id1",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "tipo_evento_id1",
                table: "Eventos",
                newName: "codigo_evento_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_tipo_evento_id1",
                table: "Eventos",
                newName: "IX_Eventos_codigo_evento_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Codigos_Evento_codigo_evento_id1",
                table: "Eventos",
                column: "codigo_evento_id1",
                principalTable: "Codigos_Evento",
                principalColumn: "codigo_evento_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Codigos_Evento_codigo_evento_id1",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "codigo_evento_id1",
                table: "Eventos",
                newName: "tipo_evento_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_codigo_evento_id1",
                table: "Eventos",
                newName: "IX_Eventos_tipo_evento_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Tipo_Eventos_tipo_evento_id1",
                table: "Eventos",
                column: "tipo_evento_id1",
                principalTable: "Tipo_Eventos",
                principalColumn: "tipo_evento_id");
        }
    }
}
