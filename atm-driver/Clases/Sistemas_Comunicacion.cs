using atm_driver.Clases;
using atm_driver.Models;
using AtmDriver;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class Sistemas_Comunicacion
{
    private readonly string _serverIp;
    private readonly int _port;
    private readonly int _tiempoEsperaUno;
    private TcpListener _server;
    private bool _isRunning;
    private readonly int _servicioId;
    private readonly AppDbContext _context;
    private bool _downloadEnProgreso; // Nueva propiedad para rastrear el estado del download

    // Constructor de la clase Sistemas_Comunicacion
    public Sistemas_Comunicacion(string serverIp, int port, int servicioId, AppDbContext context, int tiempoEsperaUno)
    {
        _serverIp = serverIp;
        _port = port;
        _server = new TcpListener(IPAddress.Any, port);
        _servicioId = servicioId;
        _context = context;
        _downloadEnProgreso = false; // Inicializar la propiedad
        _tiempoEsperaUno = tiempoEsperaUno;
    }

    // Propiedad para obtener el ID del servicio
    public int ServicioId => _servicioId;

    // Método para inicializar el servidor
    public async Task Inicializar()
    {
        Console.WriteLine("Servidor iniciando...");
        _isRunning = true;
        await EjecutarServidorAsync();
    }

    // Método privado para ejecutar el servidor de manera asíncrona
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

    // Método para manejar la conexión de un cliente de manera asíncrona
    public async Task HandleClientAsync(TcpClient client)
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

                // Verificar si la IP del cliente está registrada
                cajeroModel = Servicio.VerificarIpCajero(clientIp, _context);
                if (cajeroModel == null)
                {
                    Console.WriteLine($"La IP {clientIp} no está registrada. Cerrando conexión.");
                    Evento.GuardarEvento(CodigoEvento.ComunicacionesNoRegistrado, $"Cajero con IP {clientIp} no registrado. Conexión cerrada.", null, _servicioId);
                    client.Close();
                    return;
                }

                // Obtener las claves de comunicación del cajero
                var keyModel = await _context.Keys.FindAsync(cajeroModel.key_id);
                var download = new Download();
                await download.Inicializar(cajeroModel.download_id.Value, _context);

                // Crear una instancia de Cajero con los datos obtenidos
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
                    Cliente = client,
                    SistemasComunicacion = this, // Asignar la instancia de Sistemas_Comunicacion
                    Context = _context // Asignar el contexto de la base de datos
                };

                // Agregar el cajero a la lista de cajeros conectados
                Program.AgregarCajero(cajeroModel);
                Evento.GuardarEvento(CodigoEvento.Comunicaciones, $"Cajero con IP {clientIp} conectado.", cajeroModel.cajero_id, _servicioId);

                // Iniciar el proceso de descarga
                _downloadEnProgreso = true; // Marcar que el download está en progreso
                bool downloadDetenido = await download.EnvioDownload(cajero, this, stream);

                if (!downloadDetenido)
                {
                    // Guardar evento al finalizar el download
                    Console.WriteLine($"Cajero en Línea: ID = {cajero.Id}, IP = {clientIp}");
                    Evento.GuardarEvento(CodigoEvento.Download, "Download terminado", cajeroModel.cajero_id, _servicioId);
                    _downloadEnProgreso = false; // Marcar que el download ha terminado
                }

                // Continuar recibiendo mensajes del cajero
                while (true)
                {
                    await RecibirMensaje(stream, download, cajero);
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

                // Actualizar el estado del cajero en la base de datos a "Desconectado"
                var cajero = await _context.Cajeros.FindAsync(cajeroModel.cajero_id);
                if (cajero != null)
                {
                    cajero.estado = "Desconectado";
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Estado del cajero {cajeroModel.cajero_id} actualizado a 'Desconectado' en la base de datos.");
                }
            }
        }
    }


    // Método para enviar un mensaje al cajero y esperar una respuesta

    public async Task EnviarMensaje_EsperarRespuesta(string mensaje, Cajero cajero, NetworkStream stream)
    {
        // Enviar el mensaje al cajero
        await EnviarMensaje(mensaje, cajero, stream);

        // Crear un CancellationTokenSource con el tiempo de espera
        using (var cts = new CancellationTokenSource(_tiempoEsperaUno))
        {
            try
            {
                // Esperar la respuesta del cajero con el token de cancelación
                await RecibirMensaje(stream, null, cajero).WaitAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Error: Tiempo de espera agotado para recibir la respuesta del cajero.");
            }
        }
    }


    // Método para enviar un mensaje al cajero
    public async Task EnviarMensaje(string mensaje, Cajero cajero, NetworkStream stream)
    {
        // Convertir la longitud del mensaje a 2 bytes en formato Little-Endian
        byte[] lengthBytes = BitConverter.GetBytes((short)mensaje.Length);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(lengthBytes); // Asegurar que se envíe en Little-Endian
        }

        // Convertir el mensaje a bytes UTF-8
        byte[] mensajeBytes = Encoding.UTF8.GetBytes(mensaje);

        // Crear el paquete final (longitud + mensaje)
        byte[] finalMessage = new byte[lengthBytes.Length + mensajeBytes.Length];
        Array.Copy(lengthBytes, 0, finalMessage, 0, lengthBytes.Length);
        Array.Copy(mensajeBytes, 0, finalMessage, lengthBytes.Length, mensajeBytes.Length);

        // Enviar el mensaje completo al cajero
        await stream.WriteAsync(finalMessage, 0, finalMessage.Length);
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

    // Método para recibir un mensaje del cajero
    public async Task<string> RecibirMensaje(NetworkStream stream, Download? download, Cajero cajero)
    {
        // BufferSize es 1024 y esta en la clase Control
        byte[] buffer = new byte[Control.BufferSize];

        // Leer los primeros 2 bytes (longitud del mensaje)
        int bytesRead = await LeerTotalAsync(stream, buffer, 2);
        if (bytesRead < 2) throw new Exception("No se recibieron los bytes de longitud completos.");

        // Mostrar el array de bytes recibido (longitud)
        byte[] receivedBytes = buffer.Take(2).ToArray();
        Array.Reverse(receivedBytes); // Convertir de Little-Endian a Big-Endian
        Console.WriteLine($"Cantidad de Bytes Recibidos (Longitud): {bytesRead}");
        Console.WriteLine($"Array de bytes recibido: [{string.Join(", ", receivedBytes)}]");

        // Convertir los 2 bytes a un número (Longitud esperada)
        short originalNumber = BitConverter.ToInt16(receivedBytes, 0);
        Console.WriteLine($"Número original reconstruido (Longitud esperada): {originalNumber}");

        // Leer el mensaje completo basado en la longitud recibida
        bytesRead = await LeerTotalAsync(stream, buffer, originalNumber);
        if (bytesRead < originalNumber) throw new Exception("No se recibió el mensaje completo.");

        // Mostrar el array de bytes recibido (mensaje completo)
        byte[] messageBytes = buffer.Take(bytesRead).ToArray();
        Console.WriteLine($"Cantidad de Bytes Recibidos (Mensaje): {bytesRead}");
        Console.WriteLine($"Array de bytes recibido: [{string.Join(", ", messageBytes)}]");

        // Convertir el mensaje a string UTF-8
        string mensajeRecibido = Encoding.UTF8.GetString(messageBytes);
        Console.WriteLine($"Mensaje Recibido: {mensajeRecibido}");

        // Guardar en la base de datos
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

   

    // Función auxiliar para asegurar que se leen todos los bytes esperados
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

    // Método para cerrar la conexión del servidor
    public void CerrarConexion()
    {
        _isRunning = false;
        _server.Stop();
        Console.WriteLine("Servidor detenido.");
    }

    // Método para obtener la IP local del servidor
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
