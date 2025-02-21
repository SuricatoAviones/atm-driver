using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class keylessadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Codigos_Evento_Tipo_Eventos_tipo_evento_id1",
                table: "Codigos_Evento");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Cajeros_cajero_id1",
                table: "Dispositivos");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Denominaciones_Monedas_codigo_moneda_iddenominacion_moneda_id",
                table: "Dispositivos");

            migrationBuilder.DropForeignKey(
                name: "FK_Downloads_Formato_Cajero_FormatoCajero",
                table: "Downloads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mensajes",
                table: "Mensajes");

            migrationBuilder.DropIndex(
                name: "IX_Downloads_FormatoCajero",
                table: "Downloads");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivos_cajero_id1",
                table: "Dispositivos");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivos_codigo_moneda_iddenominacion_moneda_id",
                table: "Dispositivos");

            migrationBuilder.DropIndex(
                name: "IX_Codigos_Evento_tipo_evento_id1",
                table: "Codigos_Evento");

            migrationBuilder.DropColumn(
                name: "mensaje_id",
                table: "Mensajes");

            migrationBuilder.RenameColumn(
                name: "FormatoCajero",
                table: "Downloads",
                newName: "formato_cajero_id");

            migrationBuilder.RenameColumn(
                name: "codigo_moneda_iddenominacion_moneda_id",
                table: "Dispositivos",
                newName: "denominacion_moneda_id");

            migrationBuilder.RenameColumn(
                name: "cajero_id1",
                table: "Dispositivos",
                newName: "cajero_id");

            migrationBuilder.RenameColumn(
                name: "tipo_evento_id1",
                table: "Codigos_Evento",
                newName: "tipo_evento_id");

            migrationBuilder.AlterColumn<bool>(
                name: "origen",
                table: "Mensajes",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "formato_cajero_id",
                table: "Downloads",
                newName: "FormatoCajero");

            migrationBuilder.RenameColumn(
                name: "denominacion_moneda_id",
                table: "Dispositivos",
                newName: "codigo_moneda_iddenominacion_moneda_id");

            migrationBuilder.RenameColumn(
                name: "cajero_id",
                table: "Dispositivos",
                newName: "cajero_id1");

            migrationBuilder.RenameColumn(
                name: "tipo_evento_id",
                table: "Codigos_Evento",
                newName: "tipo_evento_id1");

            migrationBuilder.AlterColumn<string>(
                name: "origen",
                table: "Mensajes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "mensaje_id",
                table: "Mensajes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mensajes",
                table: "Mensajes",
                column: "mensaje_id");

            migrationBuilder.CreateIndex(
                name: "IX_Downloads_FormatoCajero",
                table: "Downloads",
                column: "FormatoCajero");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_cajero_id1",
                table: "Dispositivos",
                column: "cajero_id1");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_codigo_moneda_iddenominacion_moneda_id",
                table: "Dispositivos",
                column: "codigo_moneda_iddenominacion_moneda_id");

            migrationBuilder.CreateIndex(
                name: "IX_Codigos_Evento_tipo_evento_id1",
                table: "Codigos_Evento",
                column: "tipo_evento_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Codigos_Evento_Tipo_Eventos_tipo_evento_id1",
                table: "Codigos_Evento",
                column: "tipo_evento_id1",
                principalTable: "Tipo_Eventos",
                principalColumn: "tipo_evento_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Cajeros_cajero_id1",
                table: "Dispositivos",
                column: "cajero_id1",
                principalTable: "Cajeros",
                principalColumn: "cajero_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Denominaciones_Monedas_codigo_moneda_iddenominacion_moneda_id",
                table: "Dispositivos",
                column: "codigo_moneda_iddenominacion_moneda_id",
                principalTable: "Denominaciones_Monedas",
                principalColumn: "denominacion_moneda_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Downloads_Formato_Cajero_FormatoCajero",
                table: "Downloads",
                column: "FormatoCajero",
                principalTable: "Formato_Cajero",
                principalColumn: "formato_cajero_id");
        }
    }
}
