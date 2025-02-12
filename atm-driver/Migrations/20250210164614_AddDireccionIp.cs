using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class AddDireccionIp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "direccion_ip",
                table: "Cajeros",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "direccion_ip",
                table: "Cajeros");
        }
    }
}
