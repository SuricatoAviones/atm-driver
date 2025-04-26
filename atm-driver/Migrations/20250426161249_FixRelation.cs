using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class FixRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cajeros_Servicios_servicio_id1",
                table: "Cajeros");

            migrationBuilder.DropIndex(
                name: "IX_Cajeros_servicio_id1",
                table: "Cajeros");

            migrationBuilder.DropColumn(
                name: "servicio_id1",
                table: "Cajeros");

            migrationBuilder.CreateIndex(
                name: "IX_Cajeros_servicio_id",
                table: "Cajeros",
                column: "servicio_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cajeros_Servicios_servicio_id",
                table: "Cajeros",
                column: "servicio_id",
                principalTable: "Servicios",
                principalColumn: "servicio_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cajeros_Servicios_servicio_id",
                table: "Cajeros");

            migrationBuilder.DropIndex(
                name: "IX_Cajeros_servicio_id",
                table: "Cajeros");

            migrationBuilder.AddColumn<int>(
                name: "servicio_id1",
                table: "Cajeros",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cajeros_servicio_id1",
                table: "Cajeros",
                column: "servicio_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cajeros_Servicios_servicio_id1",
                table: "Cajeros",
                column: "servicio_id1",
                principalTable: "Servicios",
                principalColumn: "servicio_id");
        }
    }
}
