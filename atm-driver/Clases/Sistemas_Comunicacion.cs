using atm_driver.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace atm_driver.Clases
{
    class Sistemas_Comunicacion
    {
        // Dirección y puerto del servidor
        private readonly string serverIp;
        private readonly int port;
        private readonly int receiveTimeout;
        private readonly int sendTimeout;
        private TcpListener? listener;
        private bool isRunning;

        // Constructor
        public Sistemas_Comunicacion(string serverIp, int port, int receiveTimeout, int sendTimeout)
        {
            this.serverIp = serverIp;
            this.port = port;
            this.receiveTimeout = receiveTimeout;
            this.sendTimeout = sendTimeout;
            Console.WriteLine($"Sistema de comunicación configurado con IP: {serverIp}, Puerto: {port}, ReceiveTimeout: {receiveTimeout}, SendTimeout: {sendTimeout}");
        }

        // Métodos

        public void Inicializar()
        {
            Console.WriteLine("Sistema de comunicación inicializado.");

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            isRunning = true;
            Console.WriteLine($"Servidor iniciado en {serverIp}:{port}");

            while (isRunning)
            {
                try
                {
                    if (listener.Pending())
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Cliente conectado.");
                        Thread clientThread = new Thread(() => ManejarCliente(client));
                        clientThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en el servidor: {ex.Message}");
                }
            }
        }

        private void ManejarCliente(TcpClient client)
        {
            using NetworkStream stream = client.GetStream();
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);

            try
            {
                while (true)
                {
                    string? message = reader.ReadLine();
                    if (message == null) break;
                    Console.WriteLine($"[Recibido] {message}");

                    // Procesar el mensaje y enviar una respuesta
                    string response = ProcesarMensaje(message);
                    writer.WriteLine(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al manejar cliente: {ex.Message}");
            }
            finally
            {
                client.Close();
                Console.WriteLine("Cliente desconectado.");
            }
        }

        private string ProcesarMensaje(string message)
        {
            // Aquí puedes agregar lógica para procesar diferentes tipos de mensajes
            // y generar respuestas adecuadas. Por ejemplo:
            if (message.Contains("opcion1"))
            {
                return "Respuesta a la opción 1";
            }
            else if (message.Contains("opcion2"))
            {
                return "Respuesta a la opción 2";
            }
            else
            {
                return "Mensaje no reconocido";
            }
        }

        public void Conectar()
        {
            Console.WriteLine("Conectando al sistema de comunicación...");
            // Implementar lógica de conexión si es necesario
        }

        public string ObtenerEstadoConexion()
        {
            return isRunning ? "Conexión establecida." : "Conexión cerrada.";
        }

        public void CerrarConexion()
        {
            Console.WriteLine("Conexión cerrada.");
            isRunning = false;
            listener?.Stop();
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }

        public string EnviarMensaje(string mensaje)
        {
            try
            {
                using TcpClient client = new TcpClient(serverIp, port)
                {
                    ReceiveTimeout = receiveTimeout,
                    SendTimeout = sendTimeout
                };

                using NetworkStream stream = client.GetStream();
                using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                writer.WriteLine(mensaje);
                string respuesta = reader.ReadLine() ?? "Sin respuesta";
                return $"Mensaje enviado correctamente. Respuesta: {respuesta}";
            }
            catch (Exception ex)
            {
                return $"Error al enviar mensaje: {ex.Message}";
            }
        }

        public void EnviarMensajeEsperarRespuesta(string mensaje)
        {
            try
            {
                using TcpClient client = new TcpClient(serverIp, port)
                {
                    ReceiveTimeout = receiveTimeout,
                    SendTimeout = sendTimeout
                };

                using NetworkStream stream = client.GetStream();
                using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                writer.WriteLine(mensaje);
                string respuesta = reader.ReadLine();
                Console.WriteLine($"Respuesta del servidor: {respuesta}");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Error: No se pudo conectar al servidor.");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error inesperado: " + ex.Message);
            }
        }
    }
}
