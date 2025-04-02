using atm_driver.Models;
using System;

namespace atm_driver.Clases
{
    public class Evento
    {
        public CodigoEvento CodigoEvento { get; set; }
        public string? Observaciones { get; set; }
        public int? CajeroId { get; set; }
        public int? ServicioId { get; set; }

        public Evento(CodigoEvento codigoEvento, string? observaciones = null, int? cajeroId = null, int? servicioId = null)
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
            switch (CodigoEvento)
            {
                case CodigoEvento.Comunicaciones:
                case CodigoEvento.ComunicacionesNoRegistrado:
                    Console.WriteLine("Evento de Comunicaciones identificado.");
                    break;
                case CodigoEvento.BaseDeDatos:
                    Console.WriteLine("Evento de Base de Datos identificado.");
                    break;
                case CodigoEvento.ServidorCliente:
                    Console.WriteLine("Evento de Servidor o Cliente identificado.");
                    break;
                default:
                    Console.WriteLine("Tipo de evento no identificado.");
                    break;
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
                CodigoEvento = (CodigoEvento)(int)CodigoEvento,
                Observaciones = Observaciones,
                CajeroId = CajeroId,
                ServicioId = ServicioId
            };
            manejador.RecibirEvento();
            manejador.ProcesarEvento();
        }

        public static void GuardarEvento(CodigoEvento codigoEvento, string observaciones, int? cajeroId, int? servicioId)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var evento = new Eventos_Model
                    {
                        codigo_evento_id = (int)codigoEvento,
                        fecha = DateTime.Now,
                        observaciones = observaciones,
                        cajero_id = cajeroId,
                        servicio_id = servicioId
                    };

                    context.Eventos.Add(evento);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error guardando evento: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

    }
}
