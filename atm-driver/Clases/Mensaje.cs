using atm_driver.Models;
using System;

namespace atm_driver.Clases
{
    public class Mensaje
    {
        private readonly string _mensaje;
        private readonly string _origen;
        private readonly string _hora_entrada;
        private readonly Servicio_Model _servicio;

        public Mensaje(Mensaje_Model mensajeModel)
        {
            _mensaje = mensajeModel.mensaje ?? string.Empty;
            _origen = mensajeModel.origen?.ToString() ?? string.Empty; // Fix for CS0029 and CS8601
            _hora_entrada = mensajeModel.hora_entrada.ToString();
            _servicio = new Servicio_Model { servicio_id = mensajeModel.servicio_id ?? 0 };
        }

        public void Inicializar()
        {
            Console.WriteLine("Inicializando Mensaje");
        }

        public void ValidarFormatoMensaje()
        {
            Console.WriteLine("Validando Formato Mensaje");
        }

        public void ProcesarMensaje()
        {
            Console.WriteLine("Procesando Mensaje");
        }

        public void VerificarEnvioMensaje()
        {
            Console.WriteLine("Enviando Mensaje");
        }

        public void EnviarMensajeCajero()
        {
            Console.WriteLine("Enviando Mensaje al Cajero");
        }

        public void GuardarMensaje()
        {
            using (var context = new AppDbContext())
            {
                var mensajeModel = new Mensaje_Model
                {
                    mensaje = _mensaje,
                    origen = bool.TryParse(_origen, out var origenBool) ? (bool?)origenBool : null,
                    hora_entrada = DateTime.Parse(_hora_entrada),
                    servicio_id = _servicio.servicio_id
                };

                context.Mensajes.Add(mensajeModel);
                context.SaveChanges();
            }
        }
    }
}
