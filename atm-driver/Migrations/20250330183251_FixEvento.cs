using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class FixEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Codigos_Evento_Tipo_Eventos_tipo_evento_id1",
                table: "Codigos_Evento");

            migrationBuilder.DropIndex(
                name: "IX_Codigos_Evento_tipo_evento_id1",
                table: "Codigos_Evento");

            migrationBuilder.DropColumn(
                name: "tipo_evento_id1",
                table: "Codigos_Evento");

            migrationBuilder.CreateIndex(
                name: "IX_Codigos_Evento_tipo_evento_id",
                table: "Codigos_Evento",
                column: "tipo_evento_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Codigos_Evento_Tipo_Eventos_tipo_evento_id",
                table: "Codigos_Evento",
                column: "tipo_evento_id",
                principalTable: "Tipo_Eventos",
                principalColumn: "tipo_evento_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Codigos_Evento_Tipo_Eventos_tipo_evento_id",
                table: "Codigos_Evento");

            migrationBuilder.DropIndex(
                name: "IX_Codigos_Evento_tipo_evento_id",
                table: "Codigos_Evento");

            migrationBuilder.AddColumn<int>(
                name: "tipo_evento_id1",
                table: "Codigos_Evento",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Codigos_Evento_tipo_evento_id1",
                table: "Codigos_Evento",
                column: "tipo_evento_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Codigos_Evento_Tipo_Eventos_tipo_evento_id1",
                table: "Codigos_Evento",
                column: "tipo_evento_id1",
                principalTable: "Tipo_Eventos",
                principalColumn: "tipo_evento_id");
        }
    }
}
