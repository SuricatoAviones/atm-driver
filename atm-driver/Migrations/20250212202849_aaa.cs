using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class aaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Codigos_Evento",
                columns: table => new
                {
                    codigo_evento_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    tipo_evento_id1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codigos_Evento", x => x.codigo_evento_id);
                    table.ForeignKey(
                        name: "FK_Codigos_Evento_Tipo_Eventos_tipo_evento_id1",
                        column: x => x.tipo_evento_id1,
                        principalTable: "Tipo_Eventos",
                        principalColumn: "tipo_evento_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Codigos_Evento_tipo_evento_id1",
                table: "Codigos_Evento",
                column: "tipo_evento_id1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Codigos_Evento");
        }
    }
}
