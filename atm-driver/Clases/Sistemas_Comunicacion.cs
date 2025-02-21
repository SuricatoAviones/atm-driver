using Azure;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using atm_driver.Models;
using AtmDriver;

namespace atm_driver.Clases
{
    public class Sistemas_Comunicacion
    {
        private readonly string _serverIp;
        private readonly int _port;
        private TcpListener _server;
        private bool _isRunning;
        private readonly int _servicioId;

        public Sistemas_Comunicacion(string serverIp, int port, int servicioId)
        {
            _serverIp = serverIp;
            _port = port;
            _server = new TcpListener(IPAddress.Any, port);
            _servicioId = servicioId;
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
                string ip = ObtenerIpLocal();
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
                Evento evento = new Evento(CodigoEvento.ServidorCliente, $"Error en el servidor: {ex.Message}", null, _servicioId);
                evento.IdentificarEvento();
                evento.ValidarObservaciones();
                evento.EnviarManejadorEventos();
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            Cajeros_Model? cajeroModel = null;
            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                {
                    // Obtener la IP del cliente
                    IPEndPoint remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                    string clientIp = remoteEndPoint?.Address.ToString();
                    Console.WriteLine($"Cliente conectado desde la IP: {clientIp}");

                    // Verificar si la IP está en la base de datos
                    cajeroModel = Servicio.VerificarIpCajero(clientIp);
                    if (cajeroModel == null)
                    {
                        Console.WriteLine($"La IP {clientIp} no está registrada en la base de datos de cajeros. Cerrando conexión.");
                        Evento evento = new Evento(CodigoEvento.ComunicacionesNoRegistrado, $"Cajero con IP {clientIp} no registrado. Conexión cerrada.", null, _servicioId);
                        evento.IdentificarEvento();
                        evento.ValidarObservaciones();
                        evento.EnviarManejadorEventos();
                        client.Close();
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"La IP {clientIp} está registrada en la base de datos de cajeros.");
                        Cajero cajero = new Cajero
                        {
                            Id = cajeroModel.cajero_id,
                            Codigo = int.TryParse(cajeroModel.codigo, out int codigo) ? codigo : 0,
                            Nombre = cajeroModel.nombre,
                            Marca = cajeroModel.marca,
                            Modelo = cajeroModel.modelo,
                            ClaveComunicacion = cajeroModel.key_id?.clave_comunicacion,
                            ClaveMasterKey = cajeroModel.key_id?.clave_masterKey,
                            Localizacion = cajeroModel.localizacion,
                            Estado = cajeroModel.estado,
                            Cliente = client // Asignar el TcpClient al cajero
                        };
                        Program.AgregarCajero(cajeroModel);
                        Evento evento = new Evento(CodigoEvento.Comunicaciones, $"Cajero con IP {clientIp} conectado.", cajeroModel.cajero_id, _servicioId);
                        evento.IdentificarEvento();
                        evento.ValidarObservaciones();
                        evento.EnviarManejadorEventos();
                    }

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
                        string mensajeRecibido = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Mensaje Recibido: {mensajeRecibido}");

                        // Guardar el mensaje en la base de datos utilizando la clase Mensaje
                        var mensajeModel = new Mensaje_Model
                        {
                            mensaje = mensajeRecibido,
                            origen = true, // Asumiendo que el origen es siempre true
                            hora_entrada = DateTime.Now,
                            servicio_id = _servicioId
                        };
                        var mensaje = new Mensaje(mensajeModel);
                        mensaje.GuardarMensaje();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error manejando cliente: {ex.Message}");
                Evento evento = new Evento(CodigoEvento.ServidorCliente, $"Error manejando cliente: {ex.Message}", cajeroModel?.cajero_id, _servicioId);
                evento.IdentificarEvento();
                evento.ValidarObservaciones();
                evento.EnviarManejadorEventos();
            }
            finally
            {
                // Retirar el cajero de la lista cuando se desconecte
                if (cajeroModel != null)
                {
                    Program.RetirarCajero(cajeroModel.cajero_id);
                }
            }
        }

        public void CerrarConexion()
        {
            _isRunning = false;
            _server.Stop();
            Console.WriteLine("Servidor detenido.");
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
