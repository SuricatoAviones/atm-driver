using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class TableCodigo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fecha_mensaje",
                table: "Transacciones",
                newName: "fecha_mensaje_respuesta");

            migrationBuilder.AddColumn<string>(
                name: "codigo_operacion",
                table: "Transaccion_codigo",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codigo_operacion",
                table: "Transaccion_codigo");

            migrationBuilder.RenameColumn(
                name: "fecha_mensaje_respuesta",
                table: "Transacciones",
                newName: "fecha_mensaje");
        }
    }
}
