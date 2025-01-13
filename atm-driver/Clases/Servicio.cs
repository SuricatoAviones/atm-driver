using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    internal class Servicio
    {

     
        private readonly string _serverIp;
        private readonly int _port;

        // Constructor para inicializar desde base de datos
        /*public Servicio(int id, string codigo, string nombre, string descripcion, string estado, int tiempoEsperaUno, int tiempoEsperaDos, int? tipoMensajeId, int tipoComunicacionId)
        {
            this.id = id;
            this.codigo = codigo;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.estado = estado;
            this.tiempoEsperaUno = tiempoEsperaUno;
            this.tiempoEsperaDos = tiempoEsperaDos;
            this.tipoMensajeId = tipoMensajeId;
            this.tipoComunicacionId = tipoComunicacionId;
        }*/

        public Servicio(string serverIp, int port)
        {
            _serverIp = serverIp;
            _port = port;

            // Variables de constructor 

        }

        public void Inicializar()
        {
            Console.WriteLine("Inicializando Servicio");
            // Clase Sistemas_Comunicacion
            Sistemas_Comunicacion sistemasComunicacion = new Sistemas_Comunicacion(_serverIp, _port);
            sistemasComunicacion.Inicializar();
            sistemasComunicacion.Conectar();
            sistemasComunicacion.EnviarMensajeEsperarRespuesta();
            sistemasComunicacion.CerrarConexion();
        }

        public void VerificarEstado()
        {
            Console.WriteLine("Verificando Estado");
        }

        public void ReiniciarServicio()
        {
            Console.WriteLine("Reiniciando Servicio");
        }

    }
}
