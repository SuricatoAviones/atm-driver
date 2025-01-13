using atm_driver.Models;
using System.Net.Sockets;
using System.Text;

namespace atm_driver.Clases
{
    class Sistemas_Comunicacion
    {
        private readonly int id;
        // Dirección y puerto del servidor
        private readonly string serverIp;
        private readonly int port;

        // Constructor
        public Sistemas_Comunicacion(string serverIp, int port)
        {
            this.serverIp = serverIp;
            this.port = port;
            Console.WriteLine($"Sistema de comunicación configurado con IP: {serverIp} y Puerto: {port}");
        }

        // Métodos
        public void Inicializar()
        {
            Console.WriteLine("Sistema de comunicación inicializado.");
        }

        public void Conectar()
        {
            Console.WriteLine("Conectando al sistema de comunicación...");
        }

        public string ObtenerEstadoConexion()
        {
            return "Conexión establecida.";
        }

        public void CerrarConexion()
        {
            Console.WriteLine("Conexión cerrada.");
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }

        public string EnviarMensaje()
        {
            return "Mensaje enviado correctamente.";
        }

        public void EnviarMensajeEsperarRespuesta()
        {
            try
            {
                // Crear TcpClient
                using TcpClient client = new TcpClient
                {
                    ReceiveTimeout = 10000, // Tiempo de espera para lectura: 10 segundos
                    SendTimeout = 10000    // Tiempo de espera para escritura: 10 segundos
                };

                Console.WriteLine("Intentando conectar al servidor...");

                // Intentar conectar con un tiempo de espera de 10 segundos
                IAsyncResult result = client.BeginConnect(serverIp, port, null, null);
                if (!result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10)))
                {
                    throw new TimeoutException("La conexión al servidor agotó el tiempo de espera.");
                }

                client.EndConnect(result); // Finalizar la conexión
                Console.WriteLine("Conectado al servidor.");

                using NetworkStream stream = client.GetStream();

                // Enviar mensaje
                string message = "Hola desde el cliente.";
                byte[] messageData = Encoding.UTF8.GetBytes(message);
                stream.Write(messageData, 0, messageData.Length);
                Console.WriteLine("Mensaje enviado: " + message);

                // Leer respuesta del servidor
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string serverResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Respuesta del servidor: " + serverResponse);

                Console.WriteLine("Conexión cerrada.");
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
