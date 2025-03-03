using Azure;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using atm_driver.Models;
using AtmDriver;
using Microsoft.EntityFrameworkCore;
using atm_driver.Utils;

namespace atm_driver.Clases
{
    public class Sistemas_Comunicacion
    {
        private readonly string _serverIp;
        private readonly int _port;
        private TcpListener _server;
        private bool _isRunning;
        private readonly int _servicioId;
        private readonly AppDbContext _context;

        public Sistemas_Comunicacion(string serverIp, int port, int servicioId, AppDbContext context)
        {
            _serverIp = serverIp;
            _port = port;
            _server = new TcpListener(IPAddress.Any, port);
            _servicioId = servicioId;
            _context = context;
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
                    Console.WriteLine($"🔗 Cliente conectado desde la IP: {clientIp}");

                    // Verificar si la IP está en la base de datos
                    cajeroModel = Servicio.VerificarIpCajero(clientIp, _context);
                    if (cajeroModel == null)
                    {
                        Console.WriteLine($"⚠️ La IP {clientIp} no está registrada en la base de datos de cajeros. Cerrando conexión.");
                        new Evento(CodigoEvento.ComunicacionesNoRegistrado, $"Cajero con IP {clientIp} no registrado. Conexión cerrada.", null, _servicioId);
                        client.Close();
                        return;
                    }

                    Console.WriteLine($"✅ Cajero {cajeroModel.nombre} (ID: {cajeroModel.cajero_id}, IP: {clientIp}) está registrado.");

                    // Cargar la entidad Keys_Model relacionada
                    var keyModel = await _context.Keys.FindAsync(cajeroModel.key_id);
                    // Inicializar la clase Download
                    var download = new Download();
                    await download.Inicializar(cajeroModel.download_id.Value, _context);

                    Cajero cajero = new Cajero
                    {
                        Id = cajeroModel.cajero_id,
                        Codigo = int.TryParse(cajeroModel.codigo, out int codigo) ? codigo : 0,
                        Nombre = cajeroModel.nombre,
                        Marca = cajeroModel.marca,
                        Modelo = cajeroModel.modelo,
                        Localizacion = cajeroModel.localizacion,
                        Estado = cajeroModel.estado,
                        ClaveComunicacion = keyModel?.clave_comunicacion ?? string.Empty,
                        ClaveMasterKey = keyModel?.clave_masterKey ?? string.Empty,
                        Cliente = client
                    };

                    Program.AgregarCajero(cajeroModel);
                    new Evento(CodigoEvento.Comunicaciones, $"Cajero {cajeroModel.nombre} (ID: {cajeroModel.cajero_id}, IP: {clientIp}) conectado.", cajeroModel.cajero_id, _servicioId);

                    // Enviar las líneas del archivo de configuración al cajero
                    foreach (var linea in download.Lineas)
                    {
                        await EnviarMensaje_EsperarRespuesta(StringUtils.FormatDownloadString(linea), cajero, stream);
                    }

                    // Bucle para recibir mensajes
                    while (client.Connected)
                    {
                        try
                        {
                            await RecibirMensaje(stream);
                        }
                        catch (IOException)
                        {
                            // Detectamos desconexión abrupta
                            Console.WriteLine($"❌ Cajero {cajeroModel.nombre} (ID: {cajeroModel.cajero_id}, IP: {clientIp}) se ha desconectado abruptamente.");
                            new Evento(CodigoEvento.Comunicaciones, $"Cajero {cajeroModel.nombre} (ID: {cajeroModel.cajero_id}, IP: {clientIp}) se ha desconectado.", cajeroModel.cajero_id, _servicioId);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"⚠️ Error procesando mensaje de {cajeroModel.nombre} (ID: {cajeroModel.cajero_id}): {ex.Message}");
                            new Evento(CodigoEvento.ServidorCliente, $"Error procesando mensaje de {cajeroModel.nombre}: {ex.Message}", cajeroModel.cajero_id, _servicioId);
                            break;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"❌ Conexión perdida con cajero {cajeroModel?.nombre} (ID: {cajeroModel?.cajero_id}, IP: {cajeroModel?.direccion_ip}): {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error manejando cliente {cajeroModel?.nombre} (ID: {cajeroModel?.cajero_id}, IP: {cajeroModel?.direccion_ip}): {ex.Message}");
                new Evento(CodigoEvento.ServidorCliente, $"Error manejando cliente {cajeroModel?.nombre}: {ex.Message}", cajeroModel?.cajero_id, _servicioId);
            }
            finally
            {
                if (cajeroModel != null)
                {
                    Console.WriteLine($"🔌 Desconectando cajero {cajeroModel.nombre} (ID: {cajeroModel.cajero_id}, IP: {cajeroModel.direccion_ip})...");
                    Program.RetirarCajero(cajeroModel.cajero_id);
                }
            }
        }


        private async Task EnviarMensaje_EsperarRespuesta(string mensaje, Cajero cajero, NetworkStream stream)
        {
            // Enviar el mensaje al cajero
            await EnviarMensaje(mensaje, cajero, stream);

            // Esperar la respuesta del cajero
            await RecibirMensaje(stream);
        }

        private async Task EnviarMensaje(string mensaje, Cajero cajero, NetworkStream stream)
        {
            // Enviar el mensaje al cajero
            byte[] mensajeBytes = Encoding.UTF8.GetBytes(mensaje);
            await stream.WriteAsync(mensajeBytes, 0, mensajeBytes.Length);
            Console.WriteLine($"Enviado: {mensaje}");

            // Guardar el mensaje enviado en la base de datos con origen false
            var mensajeModel = new Mensaje_Model
            {
                mensaje = mensaje,
                origen = false,
                hora_entrada = DateTime.Now,
                servicio_id = _servicioId
            };
            var mensajeGuardado = new Mensaje(mensajeModel);
            mensajeGuardado.GuardarMensaje();
        }

        private async Task<string> RecibirMensaje(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, 2);

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

            return mensajeRecibido;
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
