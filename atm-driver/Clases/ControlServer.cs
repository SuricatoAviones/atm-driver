using atm_driver.Clases;
using AtmDriver;
using System.Net.Sockets;
using System.Net;
using System.Text;

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

    private ControlServer(int controlPort, AppDbContext context, int servicioId)
    {
        _controlPort = controlPort;
        _context = context;
        _servicioId = servicioId;
        _controlServer = new TcpListener(IPAddress.Any, controlPort);
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
                    ProcesarMensaje(mensaje);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error manejando cliente de control: {ex.Message}");
            Evento.GuardarEvento(CodigoEvento.ServidorCliente, $"Error manejando cliente de control: {ex.Message}", null, _servicioId);
        }
    }

    public void EnviarMensaje(string mensaje)
    {
        try
        {
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al enviar mensaje: {ex.Message}");
            Evento.GuardarEvento(CodigoEvento.ServidorCliente, $"Error al enviar mensaje: {ex.Message}", null, _servicioId);
        }
    }

    public void RecibirMensaje(string mensaje)
    {
        try
        {
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al recibir mensaje: {ex.Message}");
            Evento.GuardarEvento(CodigoEvento.ServidorCliente, $"Error al recibir mensaje: {ex.Message}", null, _servicioId);
        }
    }

    public void ProcesarMensaje(string mensaje)
    {
        try
        {
            string[] partes = mensaje.Split(FieldSeparator);
            if (partes.Length < 2) return;

            string codigoCajero = partes[0];
            string codigoComando = partes[1];
            Console.WriteLine($"Procesando comando {codigoComando} para cajero {codigoCajero}");

            switch (codigoComando)
            {
                case "01":
                    EnviarCajerosConectados();
                    break;
                case "02":
                    FueraDeServicio();
                    break;
                case "03":
                    EnviarContadores();
                    break;
                default:
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

    public void CerrarConexion()
    {
        _isRunning = false;
        _controlServer.Stop();
        Console.WriteLine("Servidor de control detenido.");
    }

    public void EnviarCajerosConectados()
    {
    }

    public void EnviarContadores()
    {
    }

    public void FueraDeServicio()
    {
    }

    public void FueraDeLinea()
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
