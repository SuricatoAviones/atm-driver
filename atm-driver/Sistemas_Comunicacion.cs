using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver
{
    internal class Sistemas_Comunicacion
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
