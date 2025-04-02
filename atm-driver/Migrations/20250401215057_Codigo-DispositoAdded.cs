using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class CodigoDispositoAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "codigo",
                table: "Dispositivos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "cantidad_ultima_transaccion",
                table: "Cajetines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codigo",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "cantidad_ultima_transaccion",
                table: "Cajetines");
        }
    }
}
