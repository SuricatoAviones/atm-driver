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

                // Clase Sistemas_Comunicacion
                Sistemas_Comunicacion sistemasComunicacion = new Sistemas_Comunicacion();
                sistemasComunicacion.Inicializar();
                sistemasComunicacion.Conectar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error: {ex.Message}");
            }

            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
