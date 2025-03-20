using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class nuevamigracionnn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Roles_rol_id1",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "rol_id1",
                table: "Usuarios",
                newName: "rol_id");

            migrationBuilder.RenameIndex(
                name: "IX_Usuarios_rol_id1",
                table: "Usuarios",
                newName: "IX_Usuarios_rol_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Roles_rol_id",
                table: "Usuarios",
                column: "rol_id",
                principalTable: "Roles",
                principalColumn: "rol_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Roles_rol_id",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "rol_id",
                table: "Usuarios",
                newName: "rol_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Usuarios_rol_id",
                table: "Usuarios",
                newName: "IX_Usuarios_rol_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Roles_rol_id1",
                table: "Usuarios",
                column: "rol_id1",
                principalTable: "Roles",
                principalColumn: "rol_id");
        }
    }
}
