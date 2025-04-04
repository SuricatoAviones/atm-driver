using atm_driver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    public class ManejadorEventos
    {
        public int Id { get; set; }
        public CodigoEvento CodigoEvento { get; set; }
        public string? Observaciones { get; set; }
        public int? CajeroId { get; set; }
        public int? ServicioId { get; set; }

        public void Inicializar()
        {
            // Lógica para inicializar el manejador de eventos
            Console.WriteLine("Inicializando el manejador de eventos...");
        }

        public void RecibirEvento()
        {
            // Lógica para recibir un evento
            Console.WriteLine($"Recibiendo evento con código: {CodigoEvento}");
        }

        public void ProcesarEvento()
        {
            // Lógica para procesar el evento
            Console.WriteLine($"Procesando evento con código: {CodigoEvento}");
            // Aquí se guarda el evento en la base de datos con las observaciones originales
            Evento.GuardarEvento(CodigoEvento, Observaciones ?? "Sin observaciones", CajeroId, ServicioId);
        }


        public void NotificarEvento()
        {
            // Lógica para notificar el evento
            Console.WriteLine($"Notificando evento con código: {CodigoEvento}");
        }
    }
}
