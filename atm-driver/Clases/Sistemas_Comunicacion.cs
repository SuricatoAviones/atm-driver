using System;
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
            _server = new TcpListener(IPAddress.Parse(serverIp), port);
        }

        public void Inicializar()
        {
            Console.WriteLine("Servidor iniciando...");
            _isRunning = true;
            _ = EjecutarServidorAsync();
        }

        private async Task EjecutarServidorAsync()
        {
            try
            {
                _server.Start();
                Console.WriteLine($"Servidor TCP escuchando en el puerto {_port}...");

                while (_isRunning)
                {
                    using (TcpClient client = await _server.AcceptTcpClientAsync())
                    using (NetworkStream stream = client.GetStream())
                    {
                        Console.WriteLine("Cliente conectado.");

                        byte[] buffer = new byte[1024];
                        int bytesRead = await stream.ReadAsync(buffer, 0, 2);

                        if (bytesRead == 2)
                        {
                            short originalNumber = BitConverter.ToInt16(buffer, 0);
                            Console.WriteLine($"Número recibido: {originalNumber}");

                            byte[] responseBytes = Encoding.UTF8.GetBytes($"Número: {originalNumber}");
                            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                            Console.WriteLine("Respuesta enviada.");

                            bytesRead = await stream.ReadAsync(buffer, 0, originalNumber);
                            Console.WriteLine($"Mensaje recibido: {Encoding.UTF8.GetString(buffer, 0, bytesRead)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el servidor: {ex.Message}");
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
