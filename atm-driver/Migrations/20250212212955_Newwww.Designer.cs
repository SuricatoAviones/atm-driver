﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using atm_driver.Clases;

#nullable disable

namespace atm_driver.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250212212955_Newwww")]
    partial class Newwww
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Control_Model", b =>
                {
                    b.Property<int>("control_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("control");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("control_id"));

                    b.Property<int>("codigo_institucion")
                        .HasColumnType("int")
                        .HasColumnName("codigo_institucion");

                    b.Property<string>("estado_sistema")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("estado_sistema");

                    b.Property<DateTime>("fecha_negocio")
                        .HasColumnType("datetime2")
                        .HasColumnName("fecha_negocio");

                    b.Property<string>("nombre")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("nombre");

                    b.HasKey("control_id");

                    b.ToTable("Control");
                });

            modelBuilder.Entity("atm_driver.Models.Cajeros_Model", b =>
                {
                    b.Property<int>("cajero_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("cajero_id"));

                    b.Property<string>("codigo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("direccion_ip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("download_id1")
                        .HasColumnType("int");

                    b.Property<string>("estado")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("key_id1")
                        .HasColumnType("int");

                    b.Property<string>("localizacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("marca")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("modelo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombre")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("cajero_id");

                    b.HasIndex("download_id1");

                    b.HasIndex("key_id1");

                    b.ToTable("Cajeros");
                });

            modelBuilder.Entity("atm_driver.Models.Codigos_Evento_Model", b =>
                {
                    b.Property<int>("codigo_evento_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("codigo_evento_id"));

                    b.Property<string>("descripcion")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("descripcion");

                    b.Property<string>("nombre")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("nombre");

                    b.Property<int?>("tipo_evento_id1")
                        .HasColumnType("int");

                    b.HasKey("codigo_evento_id");

                    b.HasIndex("tipo_evento_id1");

                    b.ToTable("Codigos_Evento");
                });

            modelBuilder.Entity("atm_driver.Models.Denominaciones_Monedas_Model", b =>
                {
                    b.Property<int>("denominacion_moneda_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("denominacion_moneda_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("denominacion_moneda_id"));

                    b.Property<string>("descripcion")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("descripcion");

                    b.Property<string>("nombre")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("nombre");

                    b.HasKey("denominacion_moneda_id");

                    b.ToTable("Denominaciones_Monedas");
                });

            modelBuilder.Entity("atm_driver.Models.Dispositivos_Model", b =>
                {
                    b.Property<int>("dispositivo_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("dispositivo_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("dispositivo_id"));

                    b.Property<int?>("cajero_id1")
                        .HasColumnType("int");

                    b.Property<int?>("codigo_moneda_iddenominacion_moneda_id")
                        .HasColumnType("int");

                    b.Property<string>("estado_dispositivo")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("estado_dispositivo");

                    b.Property<string>("estado_suministro")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("estado_suministro");

                    b.Property<string>("nombre")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("nombre");

                    b.HasKey("dispositivo_id");

                    b.HasIndex("cajero_id1");

                    b.HasIndex("codigo_moneda_iddenominacion_moneda_id");

                    b.ToTable("Dispositivos");
                });

            modelBuilder.Entity("atm_driver.Models.Download_Model", b =>
                {
                    b.Property<int>("download_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("download_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("download_id"));

                    b.Property<int?>("FormatoCajero")
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("nombre");

                    b.Property<string>("ruta")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("ruta");

                    b.HasKey("download_id");

                    b.HasIndex("FormatoCajero");

                    b.ToTable("Downloads");
                });

            modelBuilder.Entity("atm_driver.Models.Eventos_Model", b =>
                {
                    b.Property<int>("evento_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("evento_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("evento_id"));

                    b.Property<int?>("cajero_id1")
                        .HasColumnType("int");

                    b.Property<int?>("codigo_evento_id")
                        .HasColumnType("int")
                        .HasColumnName("codigo_evento_id");

                    b.Property<DateTime>("fecha")
                        .HasColumnType("datetime2")
                        .HasColumnName("fecha");

                    b.Property<string>("nombre")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("nombre");

                    b.Property<string>("observaciones")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("observaciones");

                    b.Property<int?>("servicio_id1")
                        .HasColumnType("int");

                    b.Property<string>("tipo")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("tipo");

                    b.HasKey("evento_id");

                    b.HasIndex("cajero_id1");

                    b.HasIndex("servicio_id1");

                    b.ToTable("Eventos");
                });

            modelBuilder.Entity("atm_driver.Models.Formato_Cajero_Model", b =>
                {
                    b.Property<int>("formato_cajero_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("formato_cajero_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("formato_cajero_id"));

                    b.Property<string>("descripcion")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("descripcion");

                    b.Property<string>("nombre")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("nombre");

                    b.HasKey("formato_cajero_id");

                    b.ToTable("Formato_Cajero");
                });

            modelBuilder.Entity("atm_driver.Models.Keys_Model", b =>
                {
                    b.Property<int>("key_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("key_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("key_id"));

                    b.Property<string>("clave_comunicacion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("clave_comunicacion");

                    b.Property<string>("clave_masterKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("clave_masterKey");

                    b.HasKey("key_id");

                    b.ToTable("Keys");
                });

            modelBuilder.Entity("atm_driver.Models.Mensaje_Model", b =>
                {
                    b.Property<int>("mensaje_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("mensaje_id"));

                    b.Property<DateTime>("hora_entrada")
                        .HasColumnType("datetime2");

                    b.Property<string>("mensaje")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("origen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("servicio_id1")
                        .HasColumnType("int");

                    b.HasKey("mensaje_id");

                    b.HasIndex("servicio_id1");

                    b.ToTable("Mensajes");
                });

            modelBuilder.Entity("atm_driver.Models.Rol_Model", b =>
                {
                    b.Property<int>("rol_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("rol_id"));

                    b.Property<string>("descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("rol_id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("atm_driver.Models.Servicio_Model", b =>
                {
                    b.Property<int>("servicio_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("servicio_id"));

                    b.Property<string>("codigo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("estado")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("sistema_comunicacion_id1")
                        .HasColumnType("int");

                    b.Property<int?>("tiempo_espera_dos")
                        .HasColumnType("int");

                    b.Property<int?>("tiempo_espera_uno")
                        .HasColumnType("int");

                    b.Property<int?>("tipo_mensaje_id1")
                        .HasColumnType("int");

                    b.HasKey("servicio_id");

                    b.HasIndex("sistema_comunicacion_id1");

                    b.HasIndex("tipo_mensaje_id1");

                    b.ToTable("Servicios");
                });

            modelBuilder.Entity("atm_driver.Models.Sistemas_Comunicacion_Model", b =>
                {
                    b.Property<int>("sistema_comunicacion_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("sistema_comunicacion_id"));

                    b.Property<string>("direccion_ip")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("ebcdc")
                        .HasColumnType("bit");

                    b.Property<bool>("empaquetado")
                        .HasColumnType("bit");

                    b.Property<string>("encabezado")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("longitud_paquete")
                        .HasColumnType("int");

                    b.Property<int?>("puerto_soap_api")
                        .HasColumnType("int");

                    b.Property<string>("puerto_tcp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("servidor_soap_api")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("socket_tcp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("tipo_conexion")
                        .HasColumnType("int");

                    b.Property<int>("tipo_conexion_tcp")
                        .HasColumnType("int");

                    b.HasKey("sistema_comunicacion_id");

                    b.ToTable("Sistemas_Comunicacion");
                });

            modelBuilder.Entity("atm_driver.Models.Tipo_Evento_Model", b =>
                {
                    b.Property<int>("tipo_evento_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("tipo_evento_id"));

                    b.Property<string>("descripcion")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("descripcion");

                    b.Property<string>("nombre")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("nombre");

                    b.HasKey("tipo_evento_id");

                    b.ToTable("Tipo_Eventos");
                });

            modelBuilder.Entity("atm_driver.Models.Tipo_Mensaje_Model", b =>
                {
                    b.Property<int>("tipo_mensaje_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("tipo_mensaje_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("tipo_mensaje_id"));

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombre_tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("tipo_mensaje_id");

                    b.ToTable("Tipo_Mensaje");
                });

            modelBuilder.Entity("atm_driver.Models.Transacciones_Model", b =>
                {
                    b.Property<int>("transaccion_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("transaccion_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("transaccion_id"));

                    b.Property<int?>("NumeroAutorizacion")
                        .HasColumnType("int")
                        .HasColumnName("numero_autorizacion");

                    b.Property<string>("codigo")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("codigo");

                    b.Property<int?>("codigo_respuesta")
                        .HasColumnType("int")
                        .HasColumnName("codigo_respuesta");

                    b.Property<DateTime?>("fecha_mensaje")
                        .HasColumnType("datetime2")
                        .HasColumnName("fecha_mensaje");

                    b.Property<DateTime?>("fecha_mensaje_recibido")
                        .HasColumnType("datetime2")
                        .HasColumnName("fecha_mensaje_recibido");

                    b.Property<DateTime?>("fecha_negocio")
                        .HasColumnType("datetime2")
                        .HasColumnName("fecha_negocio");

                    b.Property<DateTime?>("fecha_operacion")
                        .HasColumnType("datetime2")
                        .HasColumnName("fecha_operacion");

                    b.Property<int?>("monto")
                        .HasColumnType("int")
                        .HasColumnName("monto");

                    b.Property<string>("origen")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("origen");

                    b.Property<string>("pin")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("pin");

                    b.Property<string>("tarjeta")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("tarjeta");

                    b.Property<string>("tipo_cuenta")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("tipo_cuenta");

                    b.Property<string>("tipo_cuenta_destino")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("tipo_cuenta_destino");

                    b.Property<string>("trace")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("trace");

                    b.HasKey("transaccion_id");

                    b.ToTable("Transacciones");
                });

            modelBuilder.Entity("atm_driver.Models.Usuarios_Model", b =>
                {
                    b.Property<int>("usuario_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("usuario_id"));

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("rol_id1")
                        .HasColumnType("int");

                    b.HasKey("usuario_id");

                    b.HasIndex("rol_id1");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("atm_driver.Models.Cajeros_Model", b =>
                {
                    b.HasOne("atm_driver.Models.Download_Model", "download_id")
                        .WithMany()
                        .HasForeignKey("download_id1");

                    b.HasOne("atm_driver.Models.Keys_Model", "key_id")
                        .WithMany()
                        .HasForeignKey("key_id1");

                    b.Navigation("download_id");

                    b.Navigation("key_id");
                });

            modelBuilder.Entity("atm_driver.Models.Codigos_Evento_Model", b =>
                {
                    b.HasOne("atm_driver.Models.Tipo_Evento_Model", "tipo_evento_id")
                        .WithMany()
                        .HasForeignKey("tipo_evento_id1");

                    b.Navigation("tipo_evento_id");
                });

            modelBuilder.Entity("atm_driver.Models.Dispositivos_Model", b =>
                {
                    b.HasOne("atm_driver.Models.Cajeros_Model", "cajero_id")
                        .WithMany()
                        .HasForeignKey("cajero_id1");

                    b.HasOne("atm_driver.Models.Denominaciones_Monedas_Model", "codigo_moneda_id")
                        .WithMany()
                        .HasForeignKey("codigo_moneda_iddenominacion_moneda_id");

                    b.Navigation("cajero_id");

                    b.Navigation("codigo_moneda_id");
                });

            modelBuilder.Entity("atm_driver.Models.Download_Model", b =>
                {
                    b.HasOne("atm_driver.Models.Formato_Cajero_Model", "formato_cajero_id")
                        .WithMany()
                        .HasForeignKey("FormatoCajero");

                    b.Navigation("formato_cajero_id");
                });

            modelBuilder.Entity("atm_driver.Models.Eventos_Model", b =>
                {
                    b.HasOne("atm_driver.Models.Cajeros_Model", "cajero_id")
                        .WithMany()
                        .HasForeignKey("cajero_id1");

                    b.HasOne("atm_driver.Models.Servicio_Model", "servicio_id")
                        .WithMany()
                        .HasForeignKey("servicio_id1");

                    b.Navigation("cajero_id");

                    b.Navigation("servicio_id");
                });

            modelBuilder.Entity("atm_driver.Models.Mensaje_Model", b =>
                {
                    b.HasOne("atm_driver.Models.Servicio_Model", "servicio_id")
                        .WithMany()
                        .HasForeignKey("servicio_id1");

                    b.Navigation("servicio_id");
                });

            modelBuilder.Entity("atm_driver.Models.Servicio_Model", b =>
                {
                    b.HasOne("atm_driver.Models.Sistemas_Comunicacion_Model", "sistema_comunicacion_id")
                        .WithMany()
                        .HasForeignKey("sistema_comunicacion_id1");

                    b.HasOne("atm_driver.Models.Tipo_Mensaje_Model", "tipo_mensaje_id")
                        .WithMany()
                        .HasForeignKey("tipo_mensaje_id1");

                    b.Navigation("sistema_comunicacion_id");

                    b.Navigation("tipo_mensaje_id");
                });

            modelBuilder.Entity("atm_driver.Models.Usuarios_Model", b =>
                {
                    b.HasOne("atm_driver.Models.Rol_Model", "rol_id")
                        .WithMany()
                        .HasForeignKey("rol_id1");

                    b.Navigation("rol_id");
                });
#pragma warning restore 612, 618
        }
    }
}
