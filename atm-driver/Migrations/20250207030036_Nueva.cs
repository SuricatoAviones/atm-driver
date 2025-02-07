using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class Nueva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicios_Sistemas_Comunicacion_tipo_comunicacion_idsistema_comunicacion_id",
                table: "Servicios");

            migrationBuilder.RenameColumn(
                name: "tipo_comunicacion_idsistema_comunicacion_id",
                table: "Servicios",
                newName: "sistema_comunicacion_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Servicios_tipo_comunicacion_idsistema_comunicacion_id",
                table: "Servicios",
                newName: "IX_Servicios_sistema_comunicacion_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicios_Sistemas_Comunicacion_sistema_comunicacion_id1",
                table: "Servicios",
                column: "sistema_comunicacion_id1",
                principalTable: "Sistemas_Comunicacion",
                principalColumn: "sistema_comunicacion_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicios_Sistemas_Comunicacion_sistema_comunicacion_id1",
                table: "Servicios");

            migrationBuilder.RenameColumn(
                name: "sistema_comunicacion_id1",
                table: "Servicios",
                newName: "tipo_comunicacion_idsistema_comunicacion_id");

            migrationBuilder.RenameIndex(
                name: "IX_Servicios_sistema_comunicacion_id1",
                table: "Servicios",
                newName: "IX_Servicios_tipo_comunicacion_idsistema_comunicacion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicios_Sistemas_Comunicacion_tipo_comunicacion_idsistema_comunicacion_id",
                table: "Servicios",
                column: "tipo_comunicacion_idsistema_comunicacion_id",
                principalTable: "Sistemas_Comunicacion",
                principalColumn: "sistema_comunicacion_id");
        }
    }
}
