using atm_driver.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    internal class Servicio
    {
        private readonly string _serverIp;
        private readonly int _port;
        private readonly string? _codigo;
        private readonly string? _nombre;
        private readonly string? _descripcion;
        private readonly string? _estado;
        private readonly int _tiempoEsperaUno;
        private readonly int? _tiempoEsperaDos;
        private readonly Tipo_Mensaje_Model? _tipoMensaje;
        private readonly Sistemas_Comunicacion_Model? _tipoComunicacion;
        private readonly int _servicioId;
        private readonly AppDbContext _context;

        // Constructor para inicializar desde base de datos
        public Servicio(Servicio_Model servicioModel, AppDbContext context)
        {
            _serverIp = servicioModel.sistema_comunicacion_id?.direccion_ip ?? throw new ArgumentNullException(nameof(servicioModel.sistema_comunicacion_id.direccion_ip));
            _port = int.TryParse(servicioModel.sistema_comunicacion_id?.puerto_tcp, out var port) ? port : throw new ArgumentNullException(nameof(servicioModel.sistema_comunicacion_id.puerto_tcp));
            _codigo = servicioModel.codigo;
            _nombre = servicioModel.nombre;
            _descripcion = servicioModel.descripcion;
            _estado = servicioModel.estado;
            _tiempoEsperaUno = servicioModel.tiempo_espera_uno ?? 10000; // Valor por defecto si es null
            _tiempoEsperaDos = servicioModel.tiempo_espera_dos ?? 10000; // Valor por defecto si es null
            _tipoMensaje = servicioModel.tipo_mensaje_id;
            _tipoComunicacion = servicioModel.sistema_comunicacion_id;
            _servicioId = servicioModel.servicio_id;
            _context = context;
        }

        public async Task Inicializar()
        {
            Console.WriteLine("Inicializando Servicio");

            // Clase Sistemas_Comunicacion
            Sistemas_Comunicacion sistemasComunicacion = new Sistemas_Comunicacion(_serverIp, _port, _servicioId, _context, _tiempoEsperaUno);
            await sistemasComunicacion.Inicializar();
        }

        public void VerificarEstado()
        {
            Console.WriteLine("Verificando Estado");
        }

        public void ReiniciarServicio()
        {
            Console.WriteLine("Reiniciando Servicio");
        }

        public static Servicio ObtenerServicioDesdeBaseDeDatos(int servicioId, AppDbContext context)
        {
            try
            {
                var servicioModel = context.Servicios
                    .Include(s => s.sistema_comunicacion_id)
                    .Include(s => s.tipo_mensaje_id)
                    .FirstOrDefault(s => s.servicio_id == servicioId);

                if (servicioModel == null)
                {
                    throw new InvalidOperationException($"No se encontró el servicio con ID {servicioId}");
                }

                return new Servicio(servicioModel, context);
            }
            catch (Exception ex)
            {
                RegistrarErrorBaseDatos(ex.Message);
                throw;
            }
        }

        // Modificar método para devolver el objeto Cajeros_Model si la IP está en la base de datos
        public static Cajeros_Model? VerificarIpCajero(string ip, AppDbContext context)
        {
            try
            {
                return context.Cajeros.FirstOrDefault(c => c.direccion_ip == ip);
            }
            catch (Exception ex)
            {
                RegistrarErrorBaseDatos(ex.Message);
                throw;
            }
        }

        private static void RegistrarErrorBaseDatos(string mensajeError)
        {
            Evento evento = new Evento(CodigoEvento.BaseDeDatos, mensajeError, null, 1); // Código de evento 3 para errores de base de datos
            evento.IdentificarEvento();
            evento.ValidarObservaciones();
            evento.EnviarManejadorEventos();
        }
    }
}
