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
                string serverIp = "192.168.1.1"; // Ejemplo de dirección IP
                int port = 8080; // Ejemplo de puerto TCP

                Servicio servicio = new Servicio(serverIp, port);
                servicio.Inicializar();

                // Host Autorizador
                // Si necesitas inicializar HostAutorizador
                /* HostAutorizador hostAutorizador = new HostAutorizador(serverIp, port);
                hostAutorizador.Inicializar();*/

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error: {ex.Message}");
            }

            
        }
    }
}
