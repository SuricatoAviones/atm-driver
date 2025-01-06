using System;
using atm_driver;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AtmDriver
{
    

    class Program
    {
        static void Main(string[] args)
        {
            // Conexion base de datos
            var services = new ServiceCollection();
            // Configurar el contexto de base de datos
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("Data Source=localhost;Initial Catalog=atm-driver;User ID=sa;Password=151199"));

            var serviceProvider = services.BuildServiceProvider();

            // Verificar conexión
            using (var context = serviceProvider.GetRequiredService<AppDbContext>())
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
            

            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
            
        }
    }
}
