using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class TableChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cajetines_Dispositivos_dispositivo_id",
                table: "Cajetines");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Cajeros_cajero_id",
                table: "Dispositivos");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivos_cajero_id",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "cajero_id",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "estado_dispositivo",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "estado_suministro",
                table: "Dispositivos");

            migrationBuilder.RenameColumn(
                name: "dispositivo_id",
                table: "Cajetines",
                newName: "cajero_dispositivo_id");

            migrationBuilder.RenameIndex(
                name: "IX_Cajetines_dispositivo_id",
                table: "Cajetines",
                newName: "IX_Cajetines_cajero_dispositivo_id");

            migrationBuilder.CreateTable(
                name: "Cajeros_Dispositivos",
                columns: table => new
                {
                    cajero_dispositivo_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    estado_dispositivo = table.Column<int>(type: "int", nullable: true),
                    estado_suministro = table.Column<int>(type: "int", nullable: true),
                    dispositivo_id = table.Column<int>(type: "int", nullable: true),
                    cajero_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cajeros_Dispositivos", x => x.cajero_dispositivo_id);
                    table.ForeignKey(
                        name: "FK_Cajeros_Dispositivos_Cajeros_cajero_id",
                        column: x => x.cajero_id,
                        principalTable: "Cajeros",
                        principalColumn: "cajero_id");
                    table.ForeignKey(
                        name: "FK_Cajeros_Dispositivos_Dispositivos_dispositivo_id",
                        column: x => x.dispositivo_id,
                        principalTable: "Dispositivos",
                        principalColumn: "dispositivo_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cajeros_Dispositivos_cajero_id",
                table: "Cajeros_Dispositivos",
                column: "cajero_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cajeros_Dispositivos_dispositivo_id",
                table: "Cajeros_Dispositivos",
                column: "dispositivo_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cajetines_Cajeros_Dispositivos_cajero_dispositivo_id",
                table: "Cajetines",
                column: "cajero_dispositivo_id",
                principalTable: "Cajeros_Dispositivos",
                principalColumn: "cajero_dispositivo_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cajetines_Cajeros_Dispositivos_cajero_dispositivo_id",
                table: "Cajetines");

            migrationBuilder.DropTable(
                name: "Cajeros_Dispositivos");

            migrationBuilder.RenameColumn(
                name: "cajero_dispositivo_id",
                table: "Cajetines",
                newName: "dispositivo_id");

            migrationBuilder.RenameIndex(
                name: "IX_Cajetines_cajero_dispositivo_id",
                table: "Cajetines",
                newName: "IX_Cajetines_dispositivo_id");

            migrationBuilder.AddColumn<int>(
                name: "cajero_id",
                table: "Dispositivos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "estado_dispositivo",
                table: "Dispositivos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "estado_suministro",
                table: "Dispositivos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_cajero_id",
                table: "Dispositivos",
                column: "cajero_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cajetines_Dispositivos_dispositivo_id",
                table: "Cajetines",
                column: "dispositivo_id",
                principalTable: "Dispositivos",
                principalColumn: "dispositivo_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Cajeros_cajero_id",
                table: "Dispositivos",
                column: "cajero_id",
                principalTable: "Cajeros",
                principalColumn: "cajero_id");
        }
    }
}
