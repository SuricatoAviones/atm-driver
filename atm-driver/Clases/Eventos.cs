using atm_driver.Models;
using System;

namespace atm_driver.Clases
{
    public class Evento
    {
        public int CodigoEvento { get; set; }
        public string? Observaciones { get; set; }
        public int? CajeroId { get; set; }
        public int? ServicioId { get; set; }

        public Evento(int codigoEvento, string? observaciones = null, int? cajeroId = null, int? servicioId = null)
        {
            CodigoEvento = codigoEvento;
            Observaciones = observaciones;
            CajeroId = cajeroId;
            ServicioId = servicioId;
        }

        public void IdentificarEvento()
        {
            // Lógica para identificar el tipo de evento
            Console.WriteLine("Identificando tipo de evento...");
            if (CodigoEvento == 1 || CodigoEvento == 2)
            {
                Console.WriteLine("Evento de Comunicaciones identificado.");
            }
            else if (CodigoEvento == 3)
            {
                Console.WriteLine("Evento de Base de Datos identificado.");
            }
            else
            {
                Console.WriteLine("Tipo de evento no identificado.");
            }
        }

        public void ValidarObservaciones()
        {
            // Lógica para validar observaciones
            Console.WriteLine("Validando observaciones...");
            if (string.IsNullOrEmpty(Observaciones))
            {
                Console.WriteLine("No hay observaciones.");
            }
            else
            {
                Console.WriteLine($"Observaciones: {Observaciones}");
            }
        }

        public void EnviarManejadorEventos()
        {
            // Lógica para enviar al manejador de eventos
            Console.WriteLine("Enviando evento al manejador de eventos...");
            ManejadorEventos manejador = new ManejadorEventos
            {
                CodigoEvento = CodigoEvento,
                Observaciones = Observaciones,
                CajeroId = CajeroId,
                ServicioId = ServicioId
            };
            manejador.RecibirEvento();
            manejador.ProcesarEvento();
        }

        public static void GuardarEvento(int codigoEvento, string observaciones, int? cajeroId, int? servicioId)
        {
            using (var context = new AppDbContext())
            {
                var evento = new Eventos_Model
                {
                    codigo_evento_id = codigoEvento,
                    fecha = DateTime.Now,
                    observaciones = observaciones,
                    cajero_id = cajeroId,
                    servicio_id = servicioId
                };

                context.Eventos.Add(evento);
                context.SaveChanges();
            }
        }
    }
}
