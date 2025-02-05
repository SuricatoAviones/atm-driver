using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    public class Sistemas_Comunicacion
    {
        private readonly string _serverIp;
        private readonly int _port;
        private TcpListener _server;
        private bool _isRunning;

        public Sistemas_Comunicacion(string serverIp, int port)
        {
            _serverIp = serverIp;
            Console.WriteLine(serverIp);
            _port = port;
            _server = new TcpListener(IPAddress.Parse(serverIp), port);
            _isRunning = false;
        }

        public void Inicializar()
        {
            try
            {
                // Obtener las direcciones IP locales de la máquina
                var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                var ipAddresses = hostEntry.AddressList
                    .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork) // Solo IPv4
                    .ToList();

                // Mostrar las direcciones IP locales
                Console.WriteLine("Direcciones IP locales:");
                foreach (var ip in ipAddresses)
                {
                    Console.WriteLine(ip);
                }

                // Iniciar el servidor
                _server.Start();
                _isRunning = true;
                Console.WriteLine($"Servidor TCP escuchando en {_serverIp}:{_port}...");

                // Aceptar conexiones entrantes en un hilo separado
                _ = AcceptClientsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar el servidor: {ex.Message}");
            }
        }

        private async Task AcceptClientsAsync()
        {
            while (_isRunning)
            {
                try
                {
                    // Aceptar una conexión entrante
                    TcpClient client = await _server.AcceptTcpClientAsync();
                    Console.WriteLine("Cliente conectado.");

                    // Manejar la conexión en un hilo separado
                    _ = HandleClientAsync(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al aceptar cliente: {ex.Message}");
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using (client)
            {
                NetworkStream stream = client.GetStream();
                var buffer = new byte[1024];
                int bytesRead;

                try
                {
                    while ((bytesRead = await stream.ReadAsync(buffer, 0, 2)) > 0)
                    {
                        // Leer el array de bytes enviado por el cliente
                        byte[] receivedBytes = new byte[bytesRead];
                        Array.Copy(buffer, 0, receivedBytes, 0, bytesRead);

                        // Cantidad de bytes
                        Console.WriteLine($"Cantidad de Bytes Recibidos: {bytesRead}");

                        // Mostrar el array de bits recibido
                        Console.WriteLine($"Array de bytes recibido: [{string.Join(", ", receivedBytes)}]");

                        // Convertir el array de bytes al número original
                        short originalNumber = BitConverter.ToInt16(receivedBytes, 0);
                        Console.WriteLine($"Número original reconstruido: {originalNumber}");

                        // Enviar una respuesta al cliente
                        string response = $"Número original: {originalNumber}";
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                        Console.WriteLine("Respuesta enviada al cliente.");

                        // Leer el mensaje completo
                        bytesRead = await stream.ReadAsync(buffer, 0, originalNumber);
                        Console.WriteLine($"Bytes leídos: {bytesRead}");
                        Console.WriteLine($"Mensaje Recibido: {Encoding.UTF8.GetString(buffer, 0, bytesRead)}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al manejar cliente: {ex.Message}");
                }
            }
        }

        public void CerrarConexion()
        {
            _isRunning = false;
            _server.Stop();
            Console.WriteLine("Servidor detenido.");
        }
    }
}