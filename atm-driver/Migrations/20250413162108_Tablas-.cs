using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class Tablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pin",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "nombre",
                table: "Cajeros_Dispositivos");

            migrationBuilder.CreateTable(
                name: "Transaccion_codigo",
                columns: table => new
                {
                    transaccion_codigo_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo_transaccion = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    tipo_cuenta_origen = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    tipo_cuenta_destino = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    pos_ini = table.Column<int>(type: "int", maxLength: 1, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion_codigo", x => x.transaccion_codigo_id);
                });

            migrationBuilder.CreateTable(
                name: "Transaccion_evento",
                columns: table => new
                {
                    transaccion_evento_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    estado = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    screen = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion_evento", x => x.transaccion_evento_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaccion_codigo");

            migrationBuilder.DropTable(
                name: "Transaccion_evento");

            migrationBuilder.AddColumn<string>(
                name: "pin",
                table: "Transacciones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombre",
                table: "Cajeros_Dispositivos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
