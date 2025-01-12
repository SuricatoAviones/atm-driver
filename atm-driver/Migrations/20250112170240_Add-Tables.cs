using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tipo_Mensaje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Mensaje", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tiempo_espera_uno = table.Column<int>(type: "int", nullable: true),
                    tiempo_espera_dos = table.Column<int>(type: "int", nullable: true),
                    tipo_mensajeId = table.Column<int>(type: "int", nullable: true),
                    tipo_comunicacion_idId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Servicios_Sistemas_Comunicacion_tipo_comunicacion_idId",
                        column: x => x.tipo_comunicacion_idId,
                        principalTable: "Sistemas_Comunicacion",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Servicios_Tipo_Mensaje_tipo_mensajeId",
                        column: x => x.tipo_mensajeId,
                        principalTable: "Tipo_Mensaje",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_tipo_comunicacion_idId",
                table: "Servicios",
                column: "tipo_comunicacion_idId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_tipo_mensajeId",
                table: "Servicios",
                column: "tipo_mensajeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Servicios");

            migrationBuilder.DropTable(
                name: "Tipo_Mensaje");
        }
    }
}
