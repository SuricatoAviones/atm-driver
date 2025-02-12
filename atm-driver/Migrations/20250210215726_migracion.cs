using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class migracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "clave_comunicacion",
                table: "Cajeros");

            migrationBuilder.DropColumn(
                name: "clave_masterKey",
                table: "Cajeros");

            migrationBuilder.AddColumn<int>(
                name: "cajero_id1",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "servicio_id1",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "key_id1",
                table: "Cajeros",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    key_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clave_comunicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    clave_masterKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.key_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_cajero_id1",
                table: "Eventos",
                column: "cajero_id1");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_servicio_id1",
                table: "Eventos",
                column: "servicio_id1");

            migrationBuilder.CreateIndex(
                name: "IX_Cajeros_key_id1",
                table: "Cajeros",
                column: "key_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cajeros_Keys_key_id1",
                table: "Cajeros",
                column: "key_id1",
                principalTable: "Keys",
                principalColumn: "key_id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cajeros_Keys_key_id1",
                table: "Cajeros");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Cajeros_cajero_id1",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Servicios_servicio_id1",
                table: "Eventos");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_cajero_id1",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_servicio_id1",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Cajeros_key_id1",
                table: "Cajeros");

            migrationBuilder.DropColumn(
                name: "cajero_id1",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "servicio_id1",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "key_id1",
                table: "Cajeros");

            migrationBuilder.AddColumn<string>(
                name: "clave_comunicacion",
                table: "Cajeros",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "clave_masterKey",
                table: "Cajeros",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
