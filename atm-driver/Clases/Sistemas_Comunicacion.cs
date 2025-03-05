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
using atm_driver.Data;

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

        /* private async Task HandleClientAsync(TcpClient client)
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
                     cajeroModel = Servicio.VerificarIpCajero(clientIp, _context);
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
                         Evento evento = new Evento(CodigoEvento.Comunicaciones, $"Cajero con IP {clientIp} conectado.", cajeroModel.cajero_id, _servicioId);
                         evento.IdentificarEvento();
                         evento.ValidarObservaciones();
                         evento.EnviarManejadorEventos();

                         // Enviar las líneas del archivo de configuración al cajero
                         foreach (var linea in download.Lineas)
                         {
                             await EnviarMensaje_EsperarRespuesta(StringUtils.FormatDownloadString(linea), cajero, stream);
                         }

                         // Mostrar mensaje y guardar evento cuando se terminen todas las líneas del Download
                         Console.WriteLine($"Cajero en Línea: ID = {cajero.Id}, IP = {clientIp}");
                         Evento eventoDownload = new Evento(CodigoEvento.Download, "Download terminado", cajeroModel.cajero_id, _servicioId);
                         eventoDownload.IdentificarEvento();
                         eventoDownload.ValidarObservaciones();
                         eventoDownload.EnviarManejadorEventos();
                     }

                     while (true)
                     {
                         await RecibirMensaje(stream);
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
                     Evento evento = new Evento(CodigoEvento.Comunicaciones, $"Cajero con IP {cajeroModel.direccion_ip} desconectado.", cajeroModel.cajero_id, _servicioId);
                     evento.IdentificarEvento();
                     evento.ValidarObservaciones();
                     evento.EnviarManejadorEventos();
                 }
             }
         } */

        private async Task HandleClientAsync(TcpClient client)
        {
            Cajeros_Model? cajeroModel = null;
            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                {
                    IPEndPoint remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                    string clientIp = remoteEndPoint?.Address.ToString();
                    Console.WriteLine($"Cliente conectado desde la IP: {clientIp}");

                    cajeroModel = Servicio.VerificarIpCajero(clientIp, _context);
                    if (cajeroModel == null)
                    {
                        Console.WriteLine($"La IP {clientIp} no está registrada. Cerrando conexión.");
                        Evento.GuardarEvento(CodigoEvento.ComunicacionesNoRegistrado, $"Cajero con IP {clientIp} no registrado. Conexión cerrada.", null, _servicioId);
                        client.Close();
                        return;
                    }

                    var keyModel = await _context.Keys.FindAsync(cajeroModel.key_id);
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
                    Evento.GuardarEvento(CodigoEvento.Comunicaciones, $"Cajero con IP {clientIp} conectado.", cajeroModel.cajero_id, _servicioId);

                    // Guardar evento cuando inicia la descarga
                    Evento.GuardarEvento(CodigoEvento.Download, "Iniciando descarga del download", cajeroModel.cajero_id, _servicioId);

                    bool downloadDetenido = false;

                    // Enviar las líneas del archivo de configuración al cajero
                    foreach (var linea in download.Lineas)
                    {
                        string mensajeFormateado = StringUtils.FormatDownloadString(linea);

                        // Enviar mensaje al cajero y esperar respuesta
                        await EnviarMensaje_EsperarRespuesta(mensajeFormateado, cajero, stream);

                        // Recibir el mensaje de respuesta del cajero
                        string mensajeRecibido = await RecibirMensaje(stream);

                        if (!string.IsNullOrEmpty(mensajeRecibido))
                        {
                            char ultimoCaracter = mensajeRecibido[mensajeRecibido.Length - 1];

                            if (DiccionarioData.IllegalCommands.ContainsKey(ultimoCaracter.ToString()))
                            {
                                string observacion = DiccionarioData.IllegalCommands[ultimoCaracter.ToString()];
                                Evento.GuardarEvento(CodigoEvento.Download, observacion, cajeroModel.cajero_id, _servicioId);
                                Console.WriteLine($"Deteniendo envío del download: {observacion}");
                                downloadDetenido = true;
                                break; // Detener el envío sin cerrar la conexión
                            }
                        }
                    }

                    if (!downloadDetenido)
                    {
                        // Guardar evento al finalizar el download
                        Console.WriteLine($"Cajero en Línea: ID = {cajero.Id}, IP = {clientIp}");
                        Evento.GuardarEvento(CodigoEvento.Download, "Download terminado", cajeroModel.cajero_id, _servicioId);
                    }

                    while (true)
                    {
                        await RecibirMensaje(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error manejando cliente: {ex.Message}");
                Evento.GuardarEvento(CodigoEvento.ServidorCliente, $"Error manejando cliente: {ex.Message}", cajeroModel?.cajero_id, _servicioId);
            }
            finally
            {
                // Retirar el cajero de la lista cuando se desconecte
                if (cajeroModel != null)
                {
                    Program.RetirarCajero(cajeroModel.cajero_id);
                    Evento.GuardarEvento(CodigoEvento.Comunicaciones, $"Cajero con IP {cajeroModel.direccion_ip} desconectado inesperadamente.", cajeroModel.cajero_id, _servicioId);
                    Console.WriteLine($"Cajero con IP {cajeroModel.direccion_ip} desconectado inesperadamente.");
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
            // 🔹 1. Convertir la longitud del mensaje a 2 bytes en formato Little-Endian
            byte[] lengthBytes = BitConverter.GetBytes((short)mensaje.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthBytes); // Asegurar que se envíe en Little-Endian
            }

            // 🔹 2. Convertir el mensaje a bytes UTF-8
            byte[] mensajeBytes = Encoding.UTF8.GetBytes(mensaje);

            // 🔹 3. Crear el paquete final (longitud + mensaje)
            byte[] finalMessage = new byte[lengthBytes.Length + mensajeBytes.Length];
            Array.Copy(lengthBytes, 0, finalMessage, 0, lengthBytes.Length);
            Array.Copy(mensajeBytes, 0, finalMessage, lengthBytes.Length, mensajeBytes.Length);

            // 🔹 4. Enviar el mensaje completo al cajero
            await stream.WriteAsync(finalMessage, 0, finalMessage.Length);
            Console.WriteLine($"Enviado: {mensaje}");

            // 🔹 5. Guardar el mensaje enviado en la base de datos con origen false
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

            // 🔹 Leer los primeros 2 bytes (longitud del mensaje)
            int bytesRead = await LeerTotalAsync(stream, buffer, 2);
            if (bytesRead < 2) throw new Exception("No se recibieron los bytes de longitud completos.");

            // 🔹 Mostrar el array de bytes recibido (longitud)
            byte[] receivedBytes = buffer.Take(2).ToArray();
            Console.WriteLine($"Cantidad de Bytes Recibidos (Longitud): {bytesRead}");
            Console.WriteLine($"Array de bytes recibido: [{string.Join(", ", receivedBytes)}]");

            // 🔹 Convertir los 2 bytes a un número (Longitud esperada)
            short originalNumber = BitConverter.ToInt16(receivedBytes, 0);
            Console.WriteLine($"Número original reconstruido (Longitud esperada): {originalNumber}");

            // 🔹 Leer el mensaje completo basado en la longitud recibida
            bytesRead = await LeerTotalAsync(stream, buffer, originalNumber);
            if (bytesRead < originalNumber) throw new Exception("No se recibió el mensaje completo.");

            // 🔹 Mostrar el array de bytes recibido (mensaje completo)
            byte[] messageBytes = buffer.Take(bytesRead).ToArray();
            Console.WriteLine($"Cantidad de Bytes Recibidos (Mensaje): {bytesRead}");
            Console.WriteLine($"Array de bytes recibido: [{string.Join(", ", messageBytes)}]");

            // 🔹 Convertir el mensaje a string UTF-8
            string mensajeRecibido = Encoding.UTF8.GetString(messageBytes);
            Console.WriteLine($"Mensaje Recibido: {mensajeRecibido}");

            // 🔹 Guardar en la base de datos
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

        // 🔹 Función auxiliar para asegurar que se leen todos los bytes esperados
        async Task<int> LeerTotalAsync(NetworkStream stream, byte[] buffer, int length)
        {
            int totalRead = 0;
            while (totalRead < length)
            {
                int read = await stream.ReadAsync(buffer, totalRead, length - totalRead);
                if (read == 0) break; // La conexión se cerró
                totalRead += read;
            }
            return totalRead;
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