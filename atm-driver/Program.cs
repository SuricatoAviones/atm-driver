using System;
using atm_driver.Clases;

namespace AtmDriver
{
    class Program
    {
        static void Main(string[] args)
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
                    Console.WriteLine("2. Salir");
                    Console.Write("Opción: ");
                    string opcion = Console.ReadLine();

                    switch (opcion)
                    {
                        case "1":
                            try
                            {
                                var servicio = Servicio.ObtenerServicioDesdeBaseDeDatos(1); // Reemplaza 1 con el ID del servicio que deseas obtener
                                servicio.Inicializar();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case "2":
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


}


