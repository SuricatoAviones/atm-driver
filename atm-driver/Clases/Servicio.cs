using atm_driver.Models;
using Microsoft.EntityFrameworkCore;
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
        private readonly string? _codigo;
        private readonly string? _nombre;
        private readonly string? _descripcion;
        private readonly string? _estado;
        private readonly int? _tiempoEsperaUno;
        private readonly int? _tiempoEsperaDos;
        private readonly Tipo_Mensaje_Model? _tipoMensaje;
        private readonly Sistemas_Comunicacion_Model? _tipoComunicacion;

        // Constructor para inicializar desde base de datos
        public Servicio(Servicio_Model servicioModel)
        {
            _serverIp = servicioModel.tipo_comunicacion_id?.DireccionIp ?? throw new ArgumentNullException(nameof(servicioModel.tipo_comunicacion_id.DireccionIp));
            _port = servicioModel.tipo_comunicacion_id?.PuertoTcp ?? throw new ArgumentNullException(nameof(servicioModel.tipo_comunicacion_id.PuertoTcp));
            _codigo = servicioModel.codigo;
            _nombre = servicioModel.nombre;
            _descripcion = servicioModel.descripcion;
            _estado = servicioModel.estado;
            _tiempoEsperaUno = servicioModel.tiempo_espera_uno ?? 10000; // Valor por defecto si es null
            _tiempoEsperaDos = servicioModel.tiempo_espera_dos ?? 10000; // Valor por defecto si es null
            _tipoMensaje = servicioModel.tipo_mensaje;
            _tipoComunicacion = servicioModel.tipo_comunicacion_id;
        }

        public void Inicializar()
        {
            Console.WriteLine("Inicializando Servicio");
            // Clase Sistemas_Comunicacion
            Sistemas_Comunicacion sistemasComunicacion = new Sistemas_Comunicacion(_serverIp, _port, _tiempoEsperaUno.Value, _tiempoEsperaDos.Value);
            sistemasComunicacion.Inicializar();
            sistemasComunicacion.Conectar();
            /*sistemasComunicacion.EnviarMensajeEsperarRespuesta();*/
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

        public static Servicio ObtenerServicioDesdeBaseDeDatos(int servicioId)
        {
            using (var context = new AppDbContext())
            {
                var servicioModel = context.Servicios
                    .Include(s => s.tipo_comunicacion_id)
                    .Include(s => s.tipo_mensaje)
                    .FirstOrDefault(s => s.Id == servicioId);

                if (servicioModel == null)
                {
                    throw new InvalidOperationException($"No se encontró el servicio con ID {servicioId}");
                }

                return new Servicio(servicioModel);
            }
        }

    }
}
