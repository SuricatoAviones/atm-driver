using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class asdjsads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Cajeros_cajero_id1",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Servicios_servicio_id1",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_cajero_id1",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_servicio_id1",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "servicio_id1",
                table: "Eventos",
                newName: "servicio_id");

            migrationBuilder.RenameColumn(
                name: "cajero_id1",
                table: "Eventos",
                newName: "cajero_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "servicio_id",
                table: "Eventos",
                newName: "servicio_id1");

            migrationBuilder.RenameColumn(
                name: "cajero_id",
                table: "Eventos",
                newName: "cajero_id1");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_cajero_id1",
                table: "Eventos",
                column: "cajero_id1");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_servicio_id1",
                table: "Eventos",
                column: "servicio_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Cajeros_cajero_id1",
                table: "Eventos",
                column: "cajero_id1",
                principalTable: "Cajeros",
                principalColumn: "cajero_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Servicios_servicio_id1",
                table: "Eventos",
                column: "servicio_id1",
                principalTable: "Servicios",
                principalColumn: "servicio_id");
        }
    }
}
