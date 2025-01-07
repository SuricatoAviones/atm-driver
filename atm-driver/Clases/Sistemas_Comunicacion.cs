
using atm_driver.Models;

namespace atm_driver.Clases
{
    class Sistemas_Comunicacion
    {

        // Métodos
        public void Inicializar()

        {
            Console.WriteLine("Sistema de comunicación inicializado.");
        }

        public void Conectar()
        {
            Console.WriteLine("Conectando al sistema de comunicación...");
        }

        public string ObtenerEstadoConexion()
        {
            return "Conexión establecida.";
        }

        public void CerrarConexion()
        {
            Console.WriteLine("Conexión cerrada.");
        }

        public string EnviarMensaje()
        {
            return "Mensaje enviado correctamente.";
        }

        public void EnviarMensajeEsperarRespuesta()
        {
            Console.WriteLine("Enviando mensaje y esperando respuesta...");
        }
    }
}
