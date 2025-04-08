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
        private bool _esperandoRespuesta; // Flag para indicar si estamos esperando respuesta
        private bool _downloadDetenido; // Flag para indicar si el download fue detenido

        public async Task Inicializar(int downloadId, AppDbContext context)
        {
            var downloadModel = await context.Download.FindAsync(downloadId);
            if (downloadModel == null)
            {
                throw new InvalidOperationException($"No se encontró el download con ID {downloadId}");
            }

            Id = downloadModel.download_id;
            Nombre = downloadModel.nombre ?? string.Empty;
            Ruta = downloadModel.ruta ?? string.Empty;
            FormatoCajeroId = downloadModel.formato_cajero_id ?? 0;
            _esperandoRespuesta = false;
            _lineaActual = 0;
            _downloadDetenido = false;

            // Leer las líneas del archivo de configuración
            if (!string.IsNullOrEmpty(Ruta) && File.Exists(Ruta))
            {
                Lineas = await File.ReadAllLinesAsync(Ruta);
            }
            else
            {
                Lineas = Array.Empty<string>();
            }
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
            Console.WriteLine($"EnviarSiguienteLinea: Stream nulo? {stream == null}");

            if (_lineaActual >= Lineas.Length)
            {
                return false; // No hay más líneas para enviar
            }

            string linea = Lineas[_lineaActual];
            string mensajeFormateado = FormatoCajeroId == 1 ? StringUtils.FormatDownloadNDC(linea) : StringUtils.FormatDownloadTCS(linea);

            // Verificar el sexto carácter del mensaje formateado
            if (mensajeFormateado.Length >= 6)
            {
                char sextoCaracter = mensajeFormateado[5];
                if (sextoCaracter == 'A')
                {
                    // Insertar el código del cajero en el octavo carácter
                    mensajeFormateado = mensajeFormateado.Insert(7, cajero.Codigo.ToString());
                }
                else if (sextoCaracter == 'C')
                {
                    // Insertar la fecha y hora actual en el octavo carácter
                    string fechaHoraActual = DateTime.Now.ToString("yyyyMMddHHmmss");
                    mensajeFormateado = mensajeFormateado.Insert(7, fechaHoraActual);
                }
            }

            // Enviar mensaje al cajero
            await sistemasComunicacion.EnviarMensaje(mensajeFormateado, cajero, stream);
            _esperandoRespuesta = true;

            return true; // Indica que se envió una línea
        }

        public void AvanzarALaSiguienteLinea()
        {
            _lineaActual++;
            _esperandoRespuesta = false;
        }

        public bool EsperandoRespuesta()
        {
            return _esperandoRespuesta;
        }

        public bool HayMasLineas()
        {
            return _lineaActual < Lineas.Length;
        }

        public bool DownloadDetenido()
        {
            return _downloadDetenido;
        }

        public void SetDownloadDetenido(bool detenido)
        {
            _downloadDetenido = detenido;
        }

        public async Task<bool> IniciarDownload(Cajero cajero, Sistemas_Comunicacion sistemasComunicacion, NetworkStream stream)
        {
            Console.WriteLine("Iniciando Download...");

            // Seleccionar el método adecuado basado en el formato_cajero_id
            if (FormatoCajeroId == 1)
            {
                return await IniciarDownloadNDC(cajero, sistemasComunicacion, stream);
            }
            else if (FormatoCajeroId == 2)
            {
                return await IniciarDownloadTCS(cajero, sistemasComunicacion, stream);
            }
            else
            {
                Console.WriteLine($"Formato de cajero no soportado: {FormatoCajeroId}");
                Evento.GuardarEvento(CodigoEvento.Download, $"Formato de cajero no soportado: {FormatoCajeroId}", cajero.Id, sistemasComunicacion.ServicioId);
                _downloadDetenido = true;
                return false;
            }
        }

        private async Task<bool> IniciarDownloadNDC(Cajero cajero, Sistemas_Comunicacion sistemasComunicacion, NetworkStream stream)
        {
            Console.WriteLine("Iniciando Download NDC...");

            // Guardar evento cuando inicia la descarga
            Evento.GuardarEvento(CodigoEvento.Download, "Iniciando descarga del download NDC", cajero.Id, sistemasComunicacion.ServicioId);

            // Enviar la primera línea
            if (await EnviarSiguienteLinea(cajero, sistemasComunicacion, stream))
            {
                return true; // Se inició correctamente
            }

            return false; // No hay líneas para enviar
        }

        private async Task<bool> IniciarDownloadTCS(Cajero cajero, Sistemas_Comunicacion sistemasComunicacion, NetworkStream stream)
        {
            Console.WriteLine("Iniciando Download TCS...");

            // Guardar evento cuando inicia la descarga
            Evento.GuardarEvento(CodigoEvento.Download, "Iniciando descarga del download TCS", cajero.Id, sistemasComunicacion.ServicioId);

            // Enviar la primera línea
            if (await EnviarSiguienteLinea(cajero, sistemasComunicacion, stream))
            {
                return true; // Se inició correctamente
            }

            return false; // No hay líneas para enviar
        }

        public async Task<bool> ProcesarRespuesta(string mensajeRecibido, Cajero cajero, Sistemas_Comunicacion sistemasComunicacion)
        {
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
                    _downloadDetenido = true;
                    return true; // Indica que el download debe detenerse
                }
            }

            return false; // Continuar con el download
        }
    }
}