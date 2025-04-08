using atm_driver.Clases;
using AtmDriver;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using atm_driver.Models;
using System.IO;
using atm_driver.Enums;

public class ControlServer
{
    private static ControlServer? _instance;
    private static readonly object _lock = new object();
    private TcpListener _controlServer;
    private bool _isRunning;
    private readonly int _controlPort;
    private readonly AppDbContext _context;
    private readonly int _servicioId;
    private readonly char FieldSeparator = (char)28; // ASCII 28
    //private bool _downloadEnProgreso;
    private ControlServer(int controlPort, AppDbContext context, int servicioId)
    {
        _controlPort = controlPort;
        _context = context;
        _servicioId = servicioId;
        _controlServer = new TcpListener(IPAddress.Any, controlPort);
        //_downloadEnProgreso = false;
    }

    public static ControlServer GetInstance(int controlPort, AppDbContext context, int servicioId)
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new ControlServer(controlPort, context, servicioId);
            }
        }
        return _instance;
    }

    public async Task Inicializar()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            await EjecutarServidorControlAsync();
        }
    }

    private async Task EjecutarServidorControlAsync()
    {
        try
        {
            _controlServer.Start();
            string ip = ObtenerIpLocal();
            Console.WriteLine($"El panel de control debe conectarse a la IP: {ip} en el puerto: {_controlPort}");

            while (_isRunning)
            {
                TcpClient client = await _controlServer.AcceptTcpClientAsync();
                _ = HandleControlClientAsync(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en el servidor de control: {ex.Message}");
            Evento evento = new Evento(CodigoEvento.ServidorCliente, $"Error en el servidor de control: {ex.Message}", null, _servicioId);
            evento.IdentificarEvento();
            evento.ValidarObservaciones();
            evento.EnviarManejadorEventos();
        }
    }

    private async Task HandleControlClientAsync(TcpClient client)
    {
        try
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            {
                Console.WriteLine("Panel de control conectado.");

                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string mensaje = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Mensaje recibido: {mensaje}");
                    ProcesarMensaje(mensaje, stream);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error manejando cliente de control: {ex.Message}");
            Evento.GuardarEvento(CodigoEvento.ServidorCliente, $"Error manejando cliente de control: {ex.Message}", null, _servicioId);
        }
    }

    public async void ProcesarMensaje(string mensaje, NetworkStream stream)
    {
        try
        {
            string[] partes = mensaje.Split(FieldSeparator);
            if (partes.Length < 2) return;

            string codigoCajero = partes[0];
            string codigoComando = partes[1];
            Console.WriteLine($"Procesando comando {codigoComando} para cajero {codigoCajero}");

            var cajeroModel = _context.Cajeros.FirstOrDefault(c => c.codigo == codigoCajero);
            if (cajeroModel == null)
            {
                Console.WriteLine($"Cajero con código {codigoCajero} no encontrado en la base de datos.");
                Evento.GuardarEvento(CodigoEvento.ServidorCliente, $"Cajero con código {codigoCajero} no encontrado en la base de datos.", null, _servicioId);

                // Enviar mensaje de error al panel de control
                string mensajeError = $"{FieldSeparator}02";
                EnviarMensaje(mensajeError, stream);
                return;
            }

            // Buscar la instancia de Cajero directamente          
            Cajero cajero = Program.ObtenerCajerosConectados()
                .FirstOrDefault(c => c.Codigo == codigoCajero);

            if (cajero == null)
            {
                Console.WriteLine($"El cajero no está conectado. {codigoCajero}");
                Evento.GuardarEvento(CodigoEvento.ServidorCliente, $"Cajero con código {codigoCajero} no está conectado.", null, _servicioId);

                // Enviar mensaje de error al panel de control
                string mensajeError = $"{FieldSeparator}04";
                EnviarMensaje(mensajeError, stream);
                return;
            }

            // Verificar el estado del cajero en la base de datos
            string estadoCajero = await cajero.VerificarEstado();
            Console.WriteLine($"Estado actual del cajero {codigoCajero}: {estadoCajero}");

            switch (codigoComando)
            {
                case "01":
                    ColocarEnServicio(cajero, stream);
                    break;
                case "02":
                    FueraDeServicio(cajero, stream);
                    break;
                case "03":
                    EnviarDownload(cajero, stream);
                    break;
                case "04":
                    EnviarContadores(cajero, stream);
                    break;
                case "05":
                    FueraDeLinea(cajero, stream);
                    break;
                case "06":
                    ObtenerStatus(cajero, stream);
                    break;
                case "07":
                    InactivarCajero(cajero, stream);
                    break;
                default:
                    ComandoDesconocido(stream);
                    Console.WriteLine("Comando desconocido");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al procesar mensaje: {ex.Message}");
            Evento.GuardarEvento(CodigoEvento.ServidorCliente, $"Error al procesar mensaje: {ex.Message}", null, _servicioId);
        }
    }

    public void EnviarMensaje(string mensaje, NetworkStream stream)
    {
        try
        {
            byte[] mensajeBytes = Encoding.UTF8.GetBytes(mensaje);
            stream.Write(mensajeBytes, 0, mensajeBytes.Length);
            Console.WriteLine($"Mensaje enviado: {mensaje}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al enviar mensaje: {ex.Message}");
            Evento.GuardarEvento(CodigoEvento.ServidorCliente, $"Error al enviar mensaje: {ex.Message}", null, _servicioId);
        }
        finally
        {
            // Cerrar la conexión después de enviar el mensaje
            stream.Close();
        }
    }

    public void ColocarEnServicio(Cajero cajero, NetworkStream stream)
    {
        if (cajero != null)
        {
            // Llamar al método OnService del cajero
            cajero.OnService();
            string mensaje = $"{FieldSeparator}01";
            EnviarMensaje(mensaje, stream);
        }
        else
        {
            string mensaje = $"{FieldSeparator}03";
            EnviarMensaje(mensaje, stream);
            Console.WriteLine($"Cajero no está conectado.");
        }
    }

    public void FueraDeServicio(Cajero cajero, NetworkStream stream)
    {
        if (cajero != null)
        {
            // Llamar al método OnService del cajero
            cajero.OutService();
            string mensaje = $"{FieldSeparator}01";
            EnviarMensaje(mensaje, stream);
        }
        else
        {
            string mensaje = $"{FieldSeparator}03";
            EnviarMensaje(mensaje, stream);
            Console.WriteLine($"Cajero no está conectado.");
        }
    }

    public void InactivarCajero(Cajero cajero, NetworkStream stream)
    {
        if (cajero != null)
        {
            // Llamar al método ActualizarEstadoCajero para poner el cajero como Inactivo
            cajero.ActualizarEstadoCajero(EstadoCajeroEvento.Inactivo, "El Cajero ha sido inactivado").Wait();
            string mensaje = $"{FieldSeparator}01";
            EnviarMensaje(mensaje, stream);
        }
        else
        {
            string mensaje = $"{FieldSeparator}03";
            EnviarMensaje(mensaje, stream);
            Console.WriteLine($"Cajero no está conectado.");
        }
    }

    public async Task ObtenerStatus(Cajero cajero, NetworkStream stream)
    {
        // Implementación del método ObtenerStatus
        if (cajero != null)
        {
            // Llamar al método ActualizarEstadoCajero para poner el cajero como Inactivo
            await cajero.GetConfigurationInformation();
            string mensaje = $"{FieldSeparator}01";
            EnviarMensaje(mensaje, stream);
        }
        else
        {
            string mensaje = $"{FieldSeparator}03";
            EnviarMensaje(mensaje, stream);
            Console.WriteLine($"Cajero no está conectado.");
        }
    }

    //TODO: Revisar el método EnviarDownload Ya que esta fallando
    public async Task EnviarDownload(Cajero cajero, NetworkStream stream)
    {
        if (cajero != null)
        {
            try
            {
                string mensaje = $"{FieldSeparator}01";
                await cajero.EnviarDownload(cajero);
                EnviarMensaje(mensaje, stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar download: {ex.Message}");
                Evento.GuardarEvento(CodigoEvento.ServidorCliente, $"Error al enviar download: {ex.Message}", cajero.Id, _servicioId);
                string mensaje = $"{FieldSeparator}03";
                EnviarMensaje(mensaje, stream);
            }
        }
        else
        {
            string mensaje = $"{FieldSeparator}03";
            EnviarMensaje(mensaje, stream);
            Console.WriteLine($"Cajero no está conectado.");
        }
    }

    public void EnviarContadores(Cajero cajero, NetworkStream stream)
    {
        if (cajero != null)
        {
            // Llamar al método CerrarConexion del sistema de comunicación
            cajero.GetSupply();
            string mensaje = $"{FieldSeparator}01";
            EnviarMensaje(mensaje, stream);
        }
        else
        {
            string mensaje = $"{FieldSeparator}03";
            EnviarMensaje(mensaje, stream);
            Console.WriteLine($"Cajero no está conectado.");
        }
    }

    public void FueraDeLinea(Cajero cajero, NetworkStream stream)
    {
        if (cajero != null)
        {
            // Llamar al método CerrarConexion del sistema de comunicación
            cajero.SistemasComunicacion.CerrarConexion(cajero);
            string mensaje = $"{FieldSeparator}01";
            EnviarMensaje(mensaje, stream);
        }
        else
        {
            string mensaje = $"{FieldSeparator}03";
            EnviarMensaje(mensaje, stream);
            Console.WriteLine($"Cajero no está conectado.");
        }
    }

    public void ComandoDesconocido(NetworkStream stream)
    {
        string mensaje = $"{FieldSeparator}05";
        EnviarMensaje(mensaje, stream);
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
