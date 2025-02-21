using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class AddIdToMensajes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mensajes_Servicios_servicio_id1",
                table: "Mensajes");

            migrationBuilder.DropIndex(
                name: "IX_Mensajes_servicio_id1",
                table: "Mensajes");

            migrationBuilder.RenameColumn(
                name: "servicio_id1",
                table: "Mensajes",
                newName: "servicio_id");

            migrationBuilder.AddColumn<int>(
                name: "mensaje_id",
                table: "Mensajes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mensajes",
                table: "Mensajes",
                column: "mensaje_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Mensajes",
                table: "Mensajes");

            migrationBuilder.DropColumn(
                name: "mensaje_id",
                table: "Mensajes");

            migrationBuilder.RenameColumn(
                name: "servicio_id",
                table: "Mensajes",
                newName: "servicio_id1");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_servicio_id1",
                table: "Mensajes",
                column: "servicio_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensajes_Servicios_servicio_id1",
                table: "Mensajes",
                column: "servicio_id1",
                principalTable: "Servicios",
                principalColumn: "servicio_id");
        }
    }
}
