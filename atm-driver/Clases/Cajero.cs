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
        public TcpClient Cliente { get; set; }
        public Sistemas_Comunicacion SistemasComunicacion { get; set; }
        public AppDbContext Context { get; set; }
        public EstadoCajero estadoCajero;


        
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
            string mensaje = "1"+(char)28+(char)28+(char)28+"1";
            await SistemasComunicacion.EnviarMensaje(mensaje, this, Cliente.GetStream());
            estadoCajero = EstadoCajero.InService;
            Console.WriteLine(estadoCajero + "Estado Cajero");
            Console.WriteLine("Onservice Comando Enviado");
        }



        public async void OutService()
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

        public async void GetSupply()
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

        public async Task ProcesarMensaje(string mensajeRecibido, Cajero cajero)
        {
            string[] elementos = mensajeRecibido.Split((char)28);
            // Busca el id del cajero en la base de datos
            var cajeroDb = await Context.Cajeros.FindAsync(Id);
            // Evalua si es mensaje Solicitado o No Solicitado
            if (elementos[1] == "22")
            {
                

                Console.WriteLine("Es Un Mensaje Solicitado");
                // Se Procede a Evaluar si esta InService
                if (estadoCajero == EstadoCajero.InService)
                {
                    Console.WriteLine("Esta en el PROCESO DE IN SERVICE ENTONCES SIGNIFICA QUE ES LA RESPUESTA DEL INSERVICE");
                    if (elementos[4] == "99")
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
                } else if(estadoCajero == EstadoCajero.OutService)
                    {
                    if (elementos[4] == "99")
                    {
                        Console.WriteLine("El Cajero se Puso Fuera Fuera de Servicio");
                        await ActualizarEstadoCajero(EstadoCajeroEvento.FueraDeServicio, "El Cajero Se Coloco en Fuera de Servicio");
                    }
                }
            }
            else if (elementos[1] == "12")
            {
                Console.WriteLine("Mensaje No Solicitado");
            }

            //return 0;
        }

        // Método para actualizar el estado del cajero en la base de datos
        public async Task ActualizarEstadoCajero(EstadoCajeroEvento nuevoEstado, string mensajeEvento)
        {
            var cajeroDb = await Context.Cajeros.FindAsync(Id);
            if (cajeroDb != null)
            {
                // Convertir el enum a su representación en cadena
                cajeroDb.estado = nuevoEstado.ToString();
                await Context.SaveChangesAsync();
                Console.WriteLine($"Estado del cajero {Id} actualizado a '{nuevoEstado}' en la base de datos.");
                Evento.GuardarEvento(CodigoEvento.Comunicaciones, mensajeEvento, cajeroDb.cajero_id, 1);
            }
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

