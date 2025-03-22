using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using atm_driver.Data;
using atm_driver.Models;

namespace atm_driver.Clases
{
    public class Cajero
    {
        // Atributos
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string ClaveComunicacion { get; set; }
        public string ClaveMasterKey { get; set; }
        public string Localizacion { get; set; }
        public string Estado { get; set; }

        // Nueva propiedad para manejar la conexión del cliente
        public TcpClient Cliente { get; set; }

        // Nueva propiedad para la instancia de Sistemas_Comunicacion
        public Sistemas_Comunicacion SistemasComunicacion { get; set; }

        // Nueva propiedad para el contexto de la base de datos
        public AppDbContext Context { get; set; }

        // Métodos
        public void Inicializar(string type)
        {
            // Implementación del método
        }

        public async void OnService()
        {
            if (SistemasComunicacion == null)
            {
                Console.WriteLine("Error: SistemasComunicacion no está inicializado.");
                return;
            }

            string mensaje = "1" + (char)28 + (char)28 + (char)28 + "1";
             
            await SistemasComunicacion.EnviarMensaje_EsperarRespuesta(mensaje, this, Cliente.GetStream());

            // Recibir el mensaje de respuesta del cajero
            string mensajeRecibido = await SistemasComunicacion.RecibirMensaje(Cliente.GetStream(), null, this);

            // Separar el mensaje recibido en un arreglo usando el separador de campo (ASCII 28)
            string[] elementos = mensajeRecibido.Split((char)28);

            // Verificar si alguno de los elementos contiene un comando ilegal
            bool comandoIlegalEncontrado = false;
            foreach (var elemento in elementos)
            {
                if (DiccionarioData.IllegalCommands.ContainsKey(elemento))
                {
                    string observacion = DiccionarioData.IllegalCommands[elemento];
                    Evento.GuardarEvento(CodigoEvento.Comunicaciones, observacion, Id, SistemasComunicacion.ServicioId);
                    Console.WriteLine($"Cajero fuera de línea: {observacion}");
                    comandoIlegalEncontrado = true;
                    break; // Detener el procesamiento
                }
            }

            if (comandoIlegalEncontrado)
            {
                // Actualizar el estado del cajero en la base de datos a "Fuera de Línea"
                var cajero = await Context.Cajeros.FindAsync(Id);
                if (cajero != null)
                {
                    cajero.estado = "Fuera de Línea";
                    await Context.SaveChangesAsync();
                    Console.WriteLine($"Estado del cajero {Id} actualizado a 'Fuera de Línea' en la base de datos.");
                }
            }
            else
            {
                // Actualizar el estado del cajero en la base de datos a "En Línea"
                var cajero = await Context.Cajeros.FindAsync(Id);
                if (cajero != null)
                {
                    cajero.estado = "En Línea";
                    await Context.SaveChangesAsync();
                    Console.WriteLine($"Estado del cajero {Id} actualizado a 'En Línea' en la base de datos.");
                }

                // Mostrar mensaje de cajero en línea solo si no se recibió un comando ilegal
                Console.WriteLine($"Cajero en Línea: ID = {Id}, IP = {Cliente.Client.RemoteEndPoint}");
            }
        }



        public async void OutService()
        {
            if (SistemasComunicacion == null)
            {
                Console.WriteLine("Error: SistemasComunicacion no está inicializado.");
                return;
            }

            string mensaje = "1" + (char)28 + (char)28 + (char)28 + "1";
            await SistemasComunicacion.EnviarMensaje_EsperarRespuesta(mensaje, this, Cliente.GetStream());
            Console.WriteLine($"Cajero en Línea: ID = {Id}, IP = {Cliente.Client.RemoteEndPoint}");
        }

        public string VerificarEstado()
        {
            // Implementación del método
            return Estado;
        }

        public void ReportarTransaction()
        {
            // Implementación del método
        }

        public void DeterminarBilletesAdispensar()
        {
            // Implementación del método
        }

        public void EnviarMensajeSolicitado()
        {
            // Implementación del método
        }

        public void EnviarMensajeNoSolicitado()
        {
            // Implementación del método
        }

        public string ActualizarEstadoCajero()
        {
            // Implementación del método
            return Estado;
        }

        public void ProcessarMensajeSolicitado()
        {
            // Implementación del método
        }

        public void ProcessarMensajeNoSolicitado()
        {
            // Implementación del método
        }

        public void ProcessarMensaje()
        {
            // Implementación del método
        }

        public void ConstruirRecibo()
        {
            // Implementación del método
        }
    }
}

