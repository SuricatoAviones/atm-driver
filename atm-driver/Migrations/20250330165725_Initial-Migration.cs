using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atm_driver.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
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
                    tipo_evento_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codigos_Evento", x => x.codigo_evento_id);
                });

            migrationBuilder.CreateTable(
                name: "Control",
                columns: table => new
                {
                    control = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo_institucion = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fecha_negocio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estado_sistema = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Control", x => x.control);
                });

            migrationBuilder.CreateTable(
                name: "Denominaciones_Monedas",
                columns: table => new
                {
                    denominacion_moneda_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denominaciones_Monedas", x => x.denominacion_moneda_id);
                });

            migrationBuilder.CreateTable(
                name: "Formato_Cajero",
                columns: table => new
                {
                    formato_cajero_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formato_Cajero", x => x.formato_cajero_id);
                });

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    key_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clave_comunicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    clave_masterKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.key_id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    rol_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.rol_id);
                });

            migrationBuilder.CreateTable(
                name: "Sistemas_Comunicacion",
                columns: table => new
                {
                    sistema_comunicacion_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo_conexion = table.Column<int>(type: "int", nullable: false),
                    tipo_conexion_tcp = table.Column<int>(type: "int", nullable: false),
                    direccion_ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    puerto_tcp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    socket_tcp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    servidor_soap_api = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    puerto_soap_api = table.Column<int>(type: "int", nullable: true),
                    encabezado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    longitud_paquete = table.Column<int>(type: "int", nullable: false),
                    ebcdc = table.Column<bool>(type: "bit", nullable: false),
                    empaquetado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sistemas_Comunicacion", x => x.sistema_comunicacion_id);
                });

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

            migrationBuilder.CreateTable(
                name: "Tipo_Mensaje",
                columns: table => new
                {
                    tipo_mensaje_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Mensaje", x => x.tipo_mensaje_id);
                });

            migrationBuilder.CreateTable(
                name: "Downloads",
                columns: table => new
                {
                    download_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ruta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    formato_cajero_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Downloads", x => x.download_id);
                    table.ForeignKey(
                        name: "FK_Downloads_Formato_Cajero_formato_cajero_id",
                        column: x => x.formato_cajero_id,
                        principalTable: "Formato_Cajero",
                        principalColumn: "formato_cajero_id");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rol_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.usuario_id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_rol_id",
                        column: x => x.rol_id,
                        principalTable: "Roles",
                        principalColumn: "rol_id");
                });

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    servicio_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tiempo_espera_uno = table.Column<int>(type: "int", nullable: true),
                    tiempo_espera_dos = table.Column<int>(type: "int", nullable: true),
                    tipo_mensaje_id = table.Column<int>(type: "int", nullable: true),
                    sistema_comunicacion_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.servicio_id);
                    table.ForeignKey(
                        name: "FK_Servicios_Sistemas_Comunicacion_sistema_comunicacion_id",
                        column: x => x.sistema_comunicacion_id,
                        principalTable: "Sistemas_Comunicacion",
                        principalColumn: "sistema_comunicacion_id");
                    table.ForeignKey(
                        name: "FK_Servicios_Tipo_Mensaje_tipo_mensaje_id",
                        column: x => x.tipo_mensaje_id,
                        principalTable: "Tipo_Mensaje",
                        principalColumn: "tipo_mensaje_id");
                });

            migrationBuilder.CreateTable(
                name: "Cajeros",
                columns: table => new
                {
                    cajero_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    marca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modelo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    localizacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direccion_ip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    download_id = table.Column<int>(type: "int", nullable: true),
                    key_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cajeros", x => x.cajero_id);
                    table.ForeignKey(
                        name: "FK_Cajeros_Downloads_download_id",
                        column: x => x.download_id,
                        principalTable: "Downloads",
                        principalColumn: "download_id");
                    table.ForeignKey(
                        name: "FK_Cajeros_Keys_key_id",
                        column: x => x.key_id,
                        principalTable: "Keys",
                        principalColumn: "key_id");
                });

            migrationBuilder.CreateTable(
                name: "Mensajes",
                columns: table => new
                {
                    mensaje_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mensaje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    origen = table.Column<bool>(type: "bit", nullable: true),
                    hora_entrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    servicio_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensajes", x => x.mensaje_id);
                    table.ForeignKey(
                        name: "FK_Mensajes_Servicios_servicio_id",
                        column: x => x.servicio_id,
                        principalTable: "Servicios",
                        principalColumn: "servicio_id");
                });

            migrationBuilder.CreateTable(
                name: "Dispositivos",
                columns: table => new
                {
                    dispositivo_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    estado_dispositivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    estado_suministro = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    cajero_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivos", x => x.dispositivo_id);
                    table.ForeignKey(
                        name: "FK_Dispositivos_Cajeros_cajero_id",
                        column: x => x.cajero_id,
                        principalTable: "Cajeros",
                        principalColumn: "cajero_id");
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    evento_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    tipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    observaciones = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    codigo_evento_id = table.Column<int>(type: "int", nullable: true),
                    cajero_id = table.Column<int>(type: "int", nullable: true),
                    servicio_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.evento_id);
                    table.ForeignKey(
                        name: "FK_Eventos_Cajeros_cajero_id",
                        column: x => x.cajero_id,
                        principalTable: "Cajeros",
                        principalColumn: "cajero_id");
                    table.ForeignKey(
                        name: "FK_Eventos_Codigos_Evento_codigo_evento_id",
                        column: x => x.codigo_evento_id,
                        principalTable: "Codigos_Evento",
                        principalColumn: "codigo_evento_id");
                    table.ForeignKey(
                        name: "FK_Eventos_Servicios_servicio_id",
                        column: x => x.servicio_id,
                        principalTable: "Servicios",
                        principalColumn: "servicio_id");
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    transaccion_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fecha_operacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_negocio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_mensaje_recibido = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_mensaje = table.Column<DateTime>(type: "datetime2", nullable: true),
                    monto = table.Column<int>(type: "int", nullable: true),
                    trace = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    codigo_respuesta = table.Column<int>(type: "int", nullable: true),
                    tipo_cuenta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    origen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tipo_cuenta_destino = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    numero_autorizacion = table.Column<int>(type: "int", nullable: true),
                    tarjeta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    pin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    denominacion_moneda_id = table.Column<int>(type: "int", nullable: true),
                    cajero_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.transaccion_id);
                    table.ForeignKey(
                        name: "FK_Transacciones_Cajeros_cajero_id",
                        column: x => x.cajero_id,
                        principalTable: "Cajeros",
                        principalColumn: "cajero_id");
                    table.ForeignKey(
                        name: "FK_Transacciones_Denominaciones_Monedas_denominacion_moneda_id",
                        column: x => x.denominacion_moneda_id,
                        principalTable: "Denominaciones_Monedas",
                        principalColumn: "denominacion_moneda_id");
                });

            migrationBuilder.CreateTable(
                name: "Cajetines",
                columns: table => new
                {
                    cajetin_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    denominacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    tipo_denominacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    tipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    cantidad_disponible = table.Column<int>(type: "int", nullable: false),
                    cantidad_dispensada = table.Column<int>(type: "int", nullable: false),
                    cantidad_rechazada = table.Column<int>(type: "int", nullable: false),
                    numero_cajetin = table.Column<int>(type: "int", nullable: false),
                    fecha_habil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dispositivo_id = table.Column<int>(type: "int", nullable: false),
                    denominacion_moneda_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cajetines", x => x.cajetin_id);
                    table.ForeignKey(
                        name: "FK_Cajetines_Denominaciones_Monedas_denominacion_moneda_id",
                        column: x => x.denominacion_moneda_id,
                        principalTable: "Denominaciones_Monedas",
                        principalColumn: "denominacion_moneda_id");
                    table.ForeignKey(
                        name: "FK_Cajetines_Dispositivos_dispositivo_id",
                        column: x => x.dispositivo_id,
                        principalTable: "Dispositivos",
                        principalColumn: "dispositivo_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cajeros_download_id",
                table: "Cajeros",
                column: "download_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cajeros_key_id",
                table: "Cajeros",
                column: "key_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cajetines_denominacion_moneda_id",
                table: "Cajetines",
                column: "denominacion_moneda_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cajetines_dispositivo_id",
                table: "Cajetines",
                column: "dispositivo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_cajero_id",
                table: "Dispositivos",
                column: "cajero_id");

            migrationBuilder.CreateIndex(
                name: "IX_Downloads_formato_cajero_id",
                table: "Downloads",
                column: "formato_cajero_id");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_cajero_id",
                table: "Eventos",
                column: "cajero_id");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_codigo_evento_id",
                table: "Eventos",
                column: "codigo_evento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_servicio_id",
                table: "Eventos",
                column: "servicio_id");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_servicio_id",
                table: "Mensajes",
                column: "servicio_id");

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_sistema_comunicacion_id",
                table: "Servicios",
                column: "sistema_comunicacion_id");

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_tipo_mensaje_id",
                table: "Servicios",
                column: "tipo_mensaje_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_cajero_id",
                table: "Transacciones",
                column: "cajero_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_denominacion_moneda_id",
                table: "Transacciones",
                column: "denominacion_moneda_id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_rol_id",
                table: "Usuarios",
                column: "rol_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cajetines");

            migrationBuilder.DropTable(
                name: "Control");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Mensajes");

            migrationBuilder.DropTable(
                name: "Tipo_Eventos");

            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Dispositivos");

            migrationBuilder.DropTable(
                name: "Codigos_Evento");

            migrationBuilder.DropTable(
                name: "Servicios");

            migrationBuilder.DropTable(
                name: "Denominaciones_Monedas");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Cajeros");

            migrationBuilder.DropTable(
                name: "Sistemas_Comunicacion");

            migrationBuilder.DropTable(
                name: "Tipo_Mensaje");

            migrationBuilder.DropTable(
                name: "Downloads");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "Formato_Cajero");
        }
    }
}
