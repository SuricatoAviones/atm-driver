using atm_driver.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    internal class Servicio
    {
        public string ServerIp { get; private set; }
        public int Port { get; private set; }
        public int TiempoEsperaUno { get; private set; }
        public int ServicioId { get; private set; }
        private readonly AppDbContext _context;

        // Constructor para inicializar desde base de datos
        public Servicio(Servicio_Model servicioModel, AppDbContext context)
        {
            ServerIp = servicioModel.Sistemas_Comunicacion?.direccion_ip ?? throw new ArgumentNullException(nameof(servicioModel.Sistemas_Comunicacion.direccion_ip));
            Port = int.TryParse(servicioModel.Sistemas_Comunicacion?.puerto_tcp, out var port) ? port : throw new ArgumentNullException(nameof(servicioModel.Sistemas_Comunicacion.puerto_tcp));
            TiempoEsperaUno = servicioModel.tiempo_espera_uno ?? 10000; // Valor por defecto si es null
            ServicioId = servicioModel.servicio_id;
            _context = context;
        }

        public async Task Inicializar()
        {
            Console.WriteLine("Inicializando Servicio");

            // Clase Sistemas_Comunicacion
            Sistemas_Comunicacion sistemasComunicacion = new Sistemas_Comunicacion(ServerIp, Port, 5001, ServicioId, _context, TiempoEsperaUno);
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
                    .Include(s => s.Sistemas_Comunicacion)
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

        // Método para verificar si la IP del cajero está registrada en la base de datos
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
