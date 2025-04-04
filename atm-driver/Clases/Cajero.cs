using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using atm_driver.Data;
using atm_driver.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using atm_driver.Clases;
using atm_driver.Enums;
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
        public NetworkStream StreamComunicacion { get; set; }
        public int DownloadId { get; set; }
        public TcpClient Cliente { get; set; }
        public Sistemas_Comunicacion SistemasComunicacion { get; set; }
        public AppDbContext Context { get; set; }

        public EstadoCajero estadoCajero;

        private bool _downloadEnProgreso = false;



        // Métodos
        public void Inicializar(string type)
        {
            // Implementación del método
        }

        public async Task OnService()
        {
            if (SistemasComunicacion == null)
            {
                Console.WriteLine("Error: SistemasComunicacion no está inicializado.");
                return;
            }
            string mensaje = "1"+(char)28+(char)28+(char)28+"1";
            await SistemasComunicacion.EnviarMensaje(mensaje, this, Cliente.GetStream());
            estadoCajero = EstadoCajero.InService;
            Console.WriteLine(estadoCajero + "Estado Cajero");
            Console.WriteLine("Onservice Comando Enviado");
        }



        public async Task OutService()
        {
            if (SistemasComunicacion == null)
            {
                Console.WriteLine("Error: SistemasComunicacion no está inicializado.");
                return;
            }

            string mensaje = "1" + (char)28 + (char)28 + (char)28 + "2";
            await SistemasComunicacion.EnviarMensaje(mensaje, this, Cliente.GetStream());
            estadoCajero = EstadoCajero.OutService;
            Console.WriteLine(estadoCajero + "Estado Cajero");
            Console.WriteLine("Outservice Comando Enviado");


        }

        public async Task GetSupply()
        {
            if (SistemasComunicacion == null)
            {
                Console.WriteLine("Error: SistemasComunicacion no está inicializado.");
                return;
            }

            string mensaje = "1" + (char)28 + (char)28 + (char)28 + "4";
            await SistemasComunicacion.EnviarMensaje(mensaje, this, Cliente.GetStream());
            estadoCajero = EstadoCajero.GetSupply;
            Console.WriteLine(estadoCajero + "Estado Cajero");
            Console.WriteLine("GetSupply Comando Enviado");
        }

        public async Task ReenviarDownload(Cajero cajero)
        {
            // Iniciar el proceso de descarga
            var download = new Download();
            await download.Inicializar(cajero.DownloadId, Context);

            // Verificar que el stream no sea nulo
            if (StreamComunicacion == null)
            {
                Console.WriteLine("Error: StreamComunicacion es nulo en ReenviarDownload");
                return;
            }

            // Enviar las líneas del archivo de configuración al cajero
            _downloadEnProgreso = true;
            bool downloadDetenido = await download.EnvioDownload(cajero, cajero.SistemasComunicacion, StreamComunicacion);
            if (!downloadDetenido)
            {
                // Guardar evento al finalizar el download
                //Evento.GuardarEvento(CodigoEvento.Download, "Download terminado", cajero.Id, _servicioId);
                _downloadEnProgreso = false;
                await cajero.OnService();
            }
        }




        public async Task ProcesarMensaje(string mensajeRecibido, Cajero cajero)
        {
            string[] elementos = mensajeRecibido.Split((char)28);
            Console.WriteLine(elementos);
            // Busca el id del cajero en la base de datos
            var cajeroDb = await Context.Cajeros.FindAsync(Id);
            // Evalua si es mensaje Solicitado o No Solicitado
            if (elementos[0] == "22")
            {
                Console.WriteLine("Es Un Mensaje Solicitado");
                // Se Procede a Evaluar si esta InService
                if (estadoCajero == EstadoCajero.InService)
                {
                    Console.WriteLine("Esta en el PROCESO DE IN SERVICE ENTONCES SIGNIFICA QUE ES LA RESPUESTA DEL INSERVICE");
                    if (elementos[3] == "9")
                    {
                        Console.WriteLine("El Cajero se Puso En Servicio");
                        await ActualizarEstadoCajero(EstadoCajeroEvento.EnServicio, "El Cajero Se Coloco en Linea");
                    }
                    else
                    {
                        Console.WriteLine("El Cajero se Puso Fuera de Servicio");
                        await ActualizarEstadoCajero(EstadoCajeroEvento.FueraDeServicio, "El Cajero no Logro Ponerse en Linea");
                        

                    }
                    estadoCajero = EstadoCajero.Normal;


                }
                else if (estadoCajero == EstadoCajero.OutService)
                {
                    if (elementos[3] == "9")
                    {
                        Console.WriteLine("El Cajero se Puso Fuera Fuera de Servicio");
                        await ActualizarEstadoCajero(EstadoCajeroEvento.FueraDeServicio, "El Cajero Se Coloco en Fuera de Servicio");
                    }
                }
                else if (estadoCajero == EstadoCajero.GetSupply)
                {
                    if (elementos[2] == "F") // COMANDO DE TERMINAL STATE
                    {
                        // Verificar que sea de Supply Counters
                        if (elementos[3].StartsWith("2") )
                        {
                            Console.WriteLine("ES UN MENSAJE DE CONTADORES");
                            await Cajetin.ProcesarInformacion(Context, Id, elementos);
                        }
                    }

                }
            }
            else if (elementos[0] == "12")
            {
                Console.WriteLine("Mensaje No Solicitado");
            }

            //return 0;
        }

        // Método para actualizar el estado del cajero en la base de datos
        public async Task ActualizarEstadoCajero(EstadoCajeroEvento nuevoEstado, string mensajeEvento)
        {
            try
            {
                var cajeroDb = await Context.Cajeros.FindAsync(Id);
                if (cajeroDb != null)
                {
                    // Convertir el enum a su valor numérico
                    cajeroDb.estado = ((int)nuevoEstado).ToString();
                    await Context.SaveChangesAsync();
                    Console.WriteLine($"Estado del cajero {Id} actualizado a '{(int)nuevoEstado}' en la base de datos.");
                    Evento.GuardarEvento(CodigoEvento.Comunicaciones, mensajeEvento, cajeroDb.cajero_id, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error actualizando estado del cajero: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }

        }




        public async Task<string> VerificarEstado()
        {
            // Busca el cajero en la base de datos
            var cajeroDb = await Context.Cajeros.FindAsync(Id);
            if (cajeroDb != null)
            {
                // Devuelve el estado del cajero
                return cajeroDb.estado;
            }
            else
            {
                Console.WriteLine($"Error: No se encontró el cajero con ID {Id} en la base de datos.");
                return "Estado no disponible";
            }
        }


        /*public NetworkStream GetValidStream()
        {
            try
            {
                // Si el stream es nulo o el cliente no está conectado, intentar obtener uno nuevo
                if (StreamComunicacion == null || !Cliente.Connected)
                {
                    if (Cliente.Connected)
                    {
                        StreamComunicacion = Cliente.GetStream();
                        Console.WriteLine("Se creó un nuevo stream para el cajero.");
                    }
                    else
                    {
                        Console.WriteLine("Error: El cliente no está conectado, no se puede obtener el stream.");
                        return null;
                    }
                }

                return StreamComunicacion;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener stream válido: {ex.Message}");
                return null;
            }
        } */


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

