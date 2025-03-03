using System;
using System.IO;
using System.Threading.Tasks;
using atm_driver.Models;

namespace atm_driver.Clases
{
    public class Download
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public string[] Lineas { get; private set; } // Nueva propiedad para almacenar todas las líneas

        public async Task Inicializar(int downloadId, AppDbContext context)
        {
            var downloadModel = await context.Download.FindAsync(downloadId);
            if (downloadModel == null)
            {
                throw new InvalidOperationException($"No se encontró el download con ID {downloadId}");
            }

            Id = downloadModel.download_id;
            Nombre = downloadModel.nombre;
            Ruta = downloadModel.ruta;

            // Mostrar los datos de la tabla Download
            Console.WriteLine($"Datos de Download para el ID {downloadId}:");
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Ruta: {Ruta}");
            EstablecerConfiguracion();
        }

 

        public void EstablecerConfiguracion()
        {
            try
            {
                // Leer el archivo de configuración
                string filePath = Path.Combine(Ruta /*, "download.txt"*/);
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("El archivo de configuración no se encontró.", filePath);
                }

                Lineas = File.ReadAllLines(filePath);
                Console.WriteLine("Contenido del archivo de configuración:");
                foreach (var line in Lineas)
                {
                    Console.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el archivo de configuración: {ex.Message}");
                Evento.GuardarEvento(CodigoEvento.BaseDeDatos, $"Error al leer el archivo de configuración: {ex.Message}", null, null);
            }
        }
    }
}
