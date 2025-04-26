using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class ServicioIDAddedCajero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "codigo_respuesta",
                table: "Transacciones",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "servicio_id",
                table: "Cajeros",
                type: "int",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cajeros_Servicios_servicio_id1",
                table: "Cajeros");

            migrationBuilder.DropIndex(
                name: "IX_Cajeros_servicio_id1",
                table: "Cajeros");

            migrationBuilder.DropColumn(
                name: "servicio_id",
                table: "Cajeros");

            migrationBuilder.DropColumn(
                name: "servicio_id1",
                table: "Cajeros");

            migrationBuilder.AlterColumn<int>(
                name: "codigo_respuesta",
                table: "Transacciones",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);
        }
    }
}
