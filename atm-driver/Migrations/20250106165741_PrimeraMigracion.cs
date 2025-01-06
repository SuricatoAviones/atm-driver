using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class PrimeraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sistemas_Comunicacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoConexion = table.Column<int>(type: "int", nullable: false),
                    TipoConexionTcp = table.Column<int>(type: "int", nullable: false),
                    DireccionIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PuertoTcp = table.Column<int>(type: "int", nullable: false),
                    SocketTcp = table.Column<int>(type: "int", nullable: false),
                    ServidorSoapApi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PuertoSoapApi = table.Column<int>(type: "int", nullable: false),
                    Encabezado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LongitudPaquete = table.Column<int>(type: "int", nullable: false),
                    Ebcdc = table.Column<bool>(type: "bit", nullable: false),
                    Empaquetado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sistemas_Comunicacion", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sistemas_Comunicacion");
        }
    }
}
