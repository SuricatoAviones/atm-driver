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

                //Clase servicio
                Servicio servicio1 = new Servicio();
                Servicio servicio2 = new Servicio();
                servicio1.Inicializar();
                servicio2.Inicializar();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error: {ex.Message}");
            }

            
        }
    }
}
