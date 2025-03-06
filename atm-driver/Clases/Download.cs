using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using atm_driver.Data;
using atm_driver.Models;
using atm_driver.Utils;

namespace atm_driver.Clases
{
    public class Download
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public string[] Lineas { get; private set; } // Propiedad para almacenar las líneas
        public int FormatoCajeroId { get; set; } // Propiedad para almacenar el formato del cajero
        private int _lineaActual; // Propiedad para rastrear la línea actual

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
            FormatoCajeroId = downloadModel.formato_cajero_id ?? 0;

            Console.WriteLine($"Datos de Download para el ID {downloadId}:");
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Ruta: {Ruta}");
            Console.WriteLine($"Formato Cajero ID: {FormatoCajeroId}");
            EstablecerConfiguracion();
        }

        public void EstablecerConfiguracion()
        {
            try
            {
                string filePath = Path.Combine(Ruta);
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("El archivo de configuración no se encontró.", filePath);
                }

                Lineas = File.ReadAllLines(filePath);
                _lineaActual = 0; // Inicializar la línea actual
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

        public async Task<bool> EnviarSiguienteLinea(Cajero cajero, Sistemas_Comunicacion sistemasComunicacion, NetworkStream stream)
        {
            if (_lineaActual >= Lineas.Length)
            {
                return false; // No hay más líneas para enviar
            }

            string linea = Lineas[_lineaActual];
            string mensajeFormateado = FormatoCajeroId == 1 ? StringUtils.FormatDownloadNDC(linea) : StringUtils.FormatDownloadTCS(linea);

            // Enviar mensaje al cajero y esperar respuesta
            await sistemasComunicacion.EnviarMensaje(mensajeFormateado, cajero, stream);

            _lineaActual++; // Incrementar la línea actual

            return true; // Aún hay más líneas para enviar
        }

        public async Task<bool> EnvioDownload(Cajero cajero, Sistemas_Comunicacion sistemasComunicacion, NetworkStream stream)
        {
            Console.WriteLine("Enviando Download ...");

            bool downloadDetenido = false;

            // Método local para EnvioDownloadNDC
            async Task<bool> EnvioDownloadNDC()
            {
                Console.WriteLine("Enviando Download NDC ...");

                // Guardar evento cuando inicia la descarga
                Evento.GuardarEvento(CodigoEvento.Download, "Iniciando descarga del download NDC", cajero.Id, sistemasComunicacion.ServicioId);

                bool downloadDetenidoLocal = false;

                // Enviar las líneas del archivo de configuración al cajero
                while (_lineaActual < Lineas.Length)
                {
                    bool hayMasLineas = await EnviarSiguienteLinea(cajero, sistemasComunicacion, stream);
                    if (!hayMasLineas)
                    {
                        break;
                    }

                    // Recibir el mensaje de respuesta del cajero
                    string mensajeRecibido = await sistemasComunicacion.RecibirMensaje(stream, this, cajero);

                    // Separar el mensaje recibido en un arreglo usando el separador de campo (ASCII 28)
                    string[] elementos = mensajeRecibido.Split((char)28);

                    // Verificar si alguno de los elementos contiene un comando ilegal
                    foreach (var elemento in elementos)
                    {
                        if (DiccionarioData.IllegalCommands.ContainsKey(elemento))
                        {
                            string observacion = DiccionarioData.IllegalCommands[elemento];
                            Evento.GuardarEvento(CodigoEvento.Download, observacion, cajero.Id, sistemasComunicacion.ServicioId);
                            Console.WriteLine($"Deteniendo envío del download: {observacion}");
                            downloadDetenidoLocal = true;
                            break; // Detener el envío sin cerrar la conexión
                        }
                    }

                    if (downloadDetenidoLocal)
                    {
                        break;
                    }
                }

                if (!downloadDetenidoLocal)
                {
                    // Guardar evento al finalizar el download
                    Console.WriteLine($"Cajero en Línea: ID = {cajero.Id}, IP = {cajero.Cliente.Client.RemoteEndPoint}");
                    Evento.GuardarEvento(CodigoEvento.Download, "Download NDC terminado", cajero.Id, sistemasComunicacion.ServicioId);
                }

                return downloadDetenidoLocal;
            }

            // Método local para EnvioDownloadTCS
            async Task<bool> EnvioDownloadTCS()
            {
                Console.WriteLine("Enviando Download TCS ...");

                // Guardar evento cuando inicia la descarga
                Evento.GuardarEvento(CodigoEvento.Download, "Iniciando descarga del download TCS", cajero.Id, sistemasComunicacion.ServicioId);

                bool downloadDetenidoLocal = false;

                // Enviar las líneas del archivo de configuración al cajero
                while (_lineaActual < Lineas.Length)
                {
                    bool hayMasLineas = await EnviarSiguienteLinea(cajero, sistemasComunicacion, stream);
                    if (!hayMasLineas)
                    {
                        break;
                    }

                    // Recibir el mensaje de respuesta del cajero
                    string mensajeRecibido = await sistemasComunicacion.RecibirMensaje(stream, this, cajero);

                    // Separar el mensaje recibido en un arreglo usando el separador de campo (ASCII 28)
                    string[] elementos = mensajeRecibido.Split((char)28);

                    // Verificar si alguno de los elementos contiene un comando ilegal
                    foreach (var elemento in elementos)
                    {
                        if (DiccionarioData.IllegalCommands.ContainsKey(elemento))
                        {
                            string observacion = DiccionarioData.IllegalCommands[elemento];
                            Evento.GuardarEvento(CodigoEvento.Download, observacion, cajero.Id, sistemasComunicacion.ServicioId);
                            Console.WriteLine($"Deteniendo envío del download: {observacion}");
                            downloadDetenidoLocal = true;
                            break; // Detener el envío sin cerrar la conexión
                        }
                    }

                    if (downloadDetenidoLocal)
                    {
                        break;
                    }
                }

                if (!downloadDetenidoLocal)
                {
                    // Guardar evento al finalizar el download
                    Console.WriteLine($"Cajero en Línea: ID = {cajero.Id}, IP = {cajero.Cliente.Client.RemoteEndPoint}");
                    Evento.GuardarEvento(CodigoEvento.Download, "Download TCS terminado", cajero.Id, sistemasComunicacion.ServicioId);
                }

                return downloadDetenidoLocal;
            }

            // Seleccionar el método adecuado basado en el formato_cajero_id
            if (FormatoCajeroId == 1)
            {
                downloadDetenido = await EnvioDownloadNDC();
            }
            else if (FormatoCajeroId == 2)
            {
                downloadDetenido = await EnvioDownloadTCS();
            }
            else
            {
                Console.WriteLine($"Formato de cajero no soportado: {FormatoCajeroId}");
                Evento.GuardarEvento(CodigoEvento.Download, $"Formato de cajero no soportado: {FormatoCajeroId}", cajero.Id, sistemasComunicacion.ServicioId);
                downloadDetenido = true;
            }

            return downloadDetenido;
        }
    }


}
