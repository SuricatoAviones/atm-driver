using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class keyschanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "download_id",
                table: "Cajeros",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "key_id",
                table: "Cajeros",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "download_id",
                table: "Cajeros");

            migrationBuilder.DropColumn(
                name: "key_id",
                table: "Cajeros");
        }
    }
}
