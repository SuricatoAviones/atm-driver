// atm-driver/Program.cs
using System;
using System.Collections.Generic;
using atm_driver.Clases;
using atm_driver.Models;

namespace AtmDriver
{
    class Program
    {
        // Lista estática para almacenar objetos de cajeros
        private static List<Cajeros_Model> cajerosList = new List<Cajeros_Model>();

        static async Task Main(string[] args)
        {
            try
            {
                // Verificar conexión
                using (var context = AppDbContext.Create())
                {
                    if (context.Database.CanConnect())
                    {
                        Console.WriteLine("Conexión a la base de datos exitosa.");
                    }
                    else
                    {
                        Console.WriteLine("Error al conectar a la base de datos.");
                    }
                }

                // Mostrar menú
                while (true)
                {
                    Console.WriteLine("Seleccione una opción:");
                    Console.WriteLine("1. Red de Cajeros");
                    Console.WriteLine("2. Encriptador");
                    Console.WriteLine("3. Host Autorizador");
                    Console.WriteLine("4. Salir");
                    Console.Write("Opción: ");
                    string opcion = Console.ReadLine();

                    switch (opcion)
                    {
                        case "1":
                            try
                            {
                                var servicio = Servicio.ObtenerServicioDesdeBaseDeDatos(1); // Reemplaza 1 con el ID del servicio que deseas obtener
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
                                /*var host_autorizador = new HostAutorizador();
                                await host_autorizador.Inicializar();*/
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case "3":
                            try
                            {
                                var servicio = Servicio.ObtenerServicioDesdeBaseDeDatos(3); // Reemplaza 3 con el ID del servicio que deseas obtener
                                await servicio.Inicializar();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case "4":
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

        // Método estático para agregar un cajero a la lista
        public static void AgregarCajero(Cajeros_Model cajero)
        {
            cajerosList.Add(cajero);
            Console.WriteLine($"Cajero {cajero.nombre} agregado a la lista.");
        }
    }
}
