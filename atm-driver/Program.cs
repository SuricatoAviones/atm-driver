using atm_driver.Clases;
using atm_driver.Data;
using atm_driver.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AtmDriver
{
    class Program
    {
        // Lista estática para almacenar objetos de cajeros
        private static List<Cajero> _cajerosConectados = new List<Cajero>();


        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    if (context.Database.CanConnect())
                    {
                        Console.WriteLine("Conexión a la base de datos exitosa.");
                    }
                    else
                    {
                        Console.WriteLine("Error al conectar a la base de datos.");
                    }

                    

                    // Mostrar menú
                    while (true)
                    {
                        Console.WriteLine("Seleccione una opción:");
                        Console.WriteLine("1. Red de Cajeros");
                        Console.WriteLine("2. Encriptador");
                      

                        var opcion = Console.ReadLine();
                        

                        // Manejar otras opciones del menú
                        switch (opcion)
                        {
                            case "1":
                                // Iniciar el servidor de comunicación
                                var servicio = Servicio.ObtenerServicioDesdeBaseDeDatos(1, context); // Reemplaza  con el ID del servicio que deseas obtener
                                var sistemasComunicacion = new Sistemas_Comunicacion(servicio.ServerIp, servicio.Port, 5001, servicio.ServicioId, context, servicio.TiempoEsperaUno);
                                await sistemasComunicacion.Inicializar();
                                break;
                            default:
                                Console.WriteLine("Opción no válida. Intente de nuevo.");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public static void AgregarCajero(Cajero cajero)
        {
            _cajerosConectados.Add(cajero);
        }




        public static void RetirarCajero(int cajeroId)
        {
            var cajero = _cajerosConectados.FirstOrDefault(c => c.Id == cajeroId);
            if (cajero != null)
            {
                _cajerosConectados.Remove(cajero);
            }
        }


        public static List<Cajero> ObtenerCajerosConectados()
        {
            return _cajerosConectados;
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<AppDbContext>();
                });
    }
}
