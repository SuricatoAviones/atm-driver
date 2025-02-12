using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class NewTableTipEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Codigos_Evento_codigo_evento_id1",
                table: "Eventos");

            migrationBuilder.DropTable(
                name: "Codigos_Evento");

            migrationBuilder.RenameColumn(
                name: "codigo_evento_id1",
                table: "Eventos",
                newName: "tipo_evento_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_codigo_evento_id1",
                table: "Eventos",
                newName: "IX_Eventos_tipo_evento_id1");

            migrationBuilder.CreateTable(
                name: "Tipo_Eventos",
                columns: table => new
                {
                    tipo_evento_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Eventos", x => x.tipo_evento_id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Tipo_Eventos_tipo_evento_id1",
                table: "Eventos",
                column: "tipo_evento_id1",
                principalTable: "Tipo_Eventos",
                principalColumn: "tipo_evento_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Tipo_Eventos_tipo_evento_id1",
                table: "Eventos");

            migrationBuilder.DropTable(
                name: "Tipo_Eventos");

            migrationBuilder.RenameColumn(
                name: "tipo_evento_id1",
                table: "Eventos",
                newName: "codigo_evento_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_tipo_evento_id1",
                table: "Eventos",
                newName: "IX_Eventos_codigo_evento_id1");

            migrationBuilder.CreateTable(
                name: "Codigos_Evento",
                columns: table => new
                {
                    codigo_evento_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codigos_Evento", x => x.codigo_evento_id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Codigos_Evento_codigo_evento_id1",
                table: "Eventos",
                column: "codigo_evento_id1",
                principalTable: "Codigos_Evento",
                principalColumn: "codigo_evento_id");
        }
    }
}
