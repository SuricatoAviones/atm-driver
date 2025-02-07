using atm_driver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    public class Mensaje
    {

        private readonly string _mensaje;
        private readonly string _origen;
        private readonly string _hora_entrada;
        private readonly Servicio_Model _servicio_id;

        public Mensaje(Mensaje_Model mensajeModel)
        {
            _mensaje = mensajeModel.mensaje ?? string.Empty;
            _origen = mensajeModel.origen ?? string.Empty;
            _hora_entrada = mensajeModel.hora_entrada.ToString();
            _servicio_id = mensajeModel.servicio_id ?? new Servicio_Model();

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

       
    }
}
