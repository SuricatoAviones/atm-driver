using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using atm_driver.Clases;
using atm_driver.Models;
using Microsoft.EntityFrameworkCore;

namespace AtmDriver
{
    class Program
    {
        // Lista estática para almacenar objetos de cajeros
        private static List<Cajeros_Model> cajerosList = new List<Cajeros_Model>();

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
                        Console.WriteLine("3. Host Autorizador");
                        Console.WriteLine("4. Ver Cajeros Conectados");
                        Console.WriteLine("5. Salir");
                        Console.Write("Opción: ");
                        string opcion = Console.ReadLine();

                        switch (opcion)
                        {
                            case "1":
                                try
                                {
                                    var servicio = Servicio.ObtenerServicioDesdeBaseDeDatos(1, context); // Reemplaza 1 con el ID del servicio que deseas obtener
                                    await servicio.Inicializar();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                                break;
                            case "2":
                                try
                                {
                                    // Lógica para inicializar el encriptador
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                                break;
                            case "3":
                                try
                                {
                                    var hostAutorizador = HostAutorizador.ObtenerHostAutorizadorDesdeBaseDeDatos(3, DateTime.Now, DateTime.Now, context); // Reemplaza 3 con el ID del servicio que deseas obtener
                                    await hostAutorizador.Inicializar();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                                break;
                            case "4":
                                MostrarCajeros();
                                break;
                            case "5":
                                return;
                            default:
                                Console.WriteLine("Opción no válida. Intente de nuevo.");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Se produjo un error: {ex.Message}");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer("Server=LAPTOP-BQF70VD3\\SQLEXPRESS;Database=atm-driver;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True"))
                    .AddScoped<Servicio>());

        // Método estático para agregar un cajero a la lista
        public static void AgregarCajero(Cajeros_Model cajero)
        {
            cajerosList.Add(cajero);
            Console.WriteLine($"Cajero {cajero.nombre} agregado a la lista.");
        }

        // Método estático para retirar un cajero de la lista
        public static void RetirarCajero(int cajeroId)
        {
            var cajero = cajerosList.FirstOrDefault(c => c.cajero_id == cajeroId);
            if (cajero != null)
            {
                cajerosList.Remove(cajero);
                Console.WriteLine($"Cajero {cajero.nombre} retirado de la lista.");
            }
            else
            {
                Console.WriteLine($"Cajero con ID {cajeroId} no encontrado en la lista.");
            }
        }

        // Método estático para mostrar los cajeros en la lista
        public static void MostrarCajeros()
        {
            if (cajerosList.Count > 0)
            {
                Console.WriteLine("Lista de cajeros:");
                foreach (var cajero in cajerosList)
                {
                    Console.WriteLine($"ID: {cajero.cajero_id}, Nombre: {cajero.nombre}, Estado: {cajero.estado}");
                }
            }
            else
            {
                Console.WriteLine("No hay cajeros en la lista.");
            }
        }
    }
}
