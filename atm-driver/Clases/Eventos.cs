using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using atm_driver.Models;

namespace atm_driver.Clases
{
    public class Eventos
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public bool PublicarMonitor { get; set; }
        public string TipoNotificacion { get; set; }

        public void Inicializar()
        {
            // Lógica para inicializar el evento
        }

        public void IdentificarEvento()
        {
            // Lógica para identificar el evento
        }

        public void IdentificarObservaciones()
        {
            // Lógica para identificar las observaciones
        }

        public void EnviarManejadorEvento()
        {
            // Lógica para enviar el evento al manejador
        }

        // Nuevo método para guardar el evento en la base de datos
        public static void GuardarEvento(int codigoEvento, string observaciones)
        {
            using (var context = new AppDbContext())
            {
                var evento = new Eventos_Model
                {
                    codigo_evento_id = codigoEvento,
                    fecha = DateTime.Now,
                    observaciones = observaciones,
                    /*publicar_monitor = true,
                    tipo_notificacion = "Automática"*/
                };

                context.Eventos.Add(evento);
                context.SaveChanges();
            }
        }
    }
}
