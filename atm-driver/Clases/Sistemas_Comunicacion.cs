using Azure;
using System;
using System.IO;
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
            _port = port;
            // Validar si la IP es 0.0.0.0 para escuchar en todas las direcciones
            if (_serverIp == "0.0.0.0")
            {
                _server = new TcpListener(IPAddress.Any, port);
            }
            else
            {
                _server = new TcpListener(IPAddress.Parse(serverIp), port);
            }
        }

        public async Task Inicializar()
        {
            Console.WriteLine("Servidor iniciando...");
            _isRunning = true;
            await EjecutarServidorAsync();
        }

        private async Task EjecutarServidorAsync()
        {
            try
            {
                _server.Start();
                string ip = _serverIp == "0.0.0.0" ? ObtenerIpLocal() : _serverIp;
                Console.WriteLine($"El cliente debe conectarse a la IP: {ip} en el puerto: {_port}");


                while (_isRunning)
                {
                    TcpClient client = await _server.AcceptTcpClientAsync();
                    _ = HandleClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el servidor: {ex.Message}");
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                {
                    // Obtener la IP del cliente
                    IPEndPoint remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                    string clientIp = remoteEndPoint?.Address.ToString();
                    Console.WriteLine($"Cliente conectado desde la IP: {clientIp}");

                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, 2)) > 0)
                    {
                        // Leer el array de bytes enviado por el cliente
                        byte[] receivedBytes = new byte[bytesRead];
                        Array.Copy(buffer, 0, receivedBytes, 0, bytesRead);

                        // Cantidad de bytes
                        Console.WriteLine($"Cantidad de Bytes Recibidos: {bytesRead}");

                        // Mostrar el array de bytes recibido
                        Console.WriteLine($"Array de bytes recibido: [{string.Join(", ", receivedBytes)}]");

                        // Convertir el array de bytes al número original
                        short originalNumber = BitConverter.ToInt16(receivedBytes, 0);
                        Console.WriteLine($"Número original reconstruido: {originalNumber}");

                        bytesRead = await stream.ReadAsync(buffer, 0, originalNumber);
                        Console.WriteLine($"Cantidad de Bytes Recibidos: {bytesRead}");
                        Console.WriteLine($"Mensaje Recibido: {Encoding.UTF8.GetString(buffer, 0, bytesRead)}");

                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error manejando cliente: {ex.Message}");
            }
        }

        public void CerrarConexion()
        {
            _isRunning = false;
            _server.Stop();
            Console.WriteLine("Servidor detenido.");
        }

        public void EnviarMensaje()
        {
            // Enviar una respuesta al cliente
            /*string response = $"Número original: {originalNumber}";
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            Console.WriteLine("Respuesta enviada al cliente.");*/

        }

        public void EnviarMensaje_EsperarRespuesta()
        {

        }


        private string ObtenerIpLocal()
        {
            string localIP = "";
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }
    }
}

