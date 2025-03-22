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

    public async Task HandleControlClientAsync(TcpClient client)
    {
        try
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            {
                Console.WriteLine("Panel de control conectado.");

                // Enviar la lista de cajeros conectados al panel de control
                var cajerosConectados = Program.ObtenerCajerosConectados();
                string mensaje = string.Join(",", cajerosConectados.Select(c => c.direccion_ip));
                byte[] mensajeBytes = Encoding.UTF8.GetBytes(mensaje);
                await stream.WriteAsync(mensajeBytes, 0, mensajeBytes.Length);
                Console.WriteLine("Lista de cajeros conectados enviada al panel de control.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error manejando cliente de control: {ex.Message}");
            Evento.GuardarEvento(CodigoEvento.ServidorCliente, $"Error manejando cliente de control: {ex.Message}", null, _servicioId);
        }
    }

    public void CerrarConexion()
    {
        _isRunning = false;
        _controlServer.Stop();
        Console.WriteLine("Servidor de control detenido.");
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
