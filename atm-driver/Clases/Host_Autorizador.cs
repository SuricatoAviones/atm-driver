using atm_driver.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    internal class HostAutorizador : Servicio
    {
        // Propiedades específicas de HostAutorizador
        private readonly DateTime _fechaEnvio;
        private readonly DateTime _fechaRecepcion;

        // Constructor para inicializar desde Servicio_Model
        public HostAutorizador(Servicio_Model servicioModel, AppDbContext context, DateTime fechaEnvio, DateTime fechaRecepcion)
            : base(servicioModel, context) // Llamada al constructor de la clase base (Servicio)
        {
            _fechaEnvio = fechaEnvio;
            _fechaRecepcion = fechaRecepcion;
        }

        // Método para inicializar el host autorizador
        public async new Task Inicializar()
        {
            Console.WriteLine("Inicializando Host Autorizador");
            await base.Inicializar(); // Llama al método Inicializar de la clase base (Servicio)
        }

        // Método para verificar la conexión
        public void VerificarConexion()
        {
            Console.WriteLine("Verificando Conexión");
            // Lógica de verificación de conexión aquí
        }

        // Método para verificar la transacción
        public void VerificarTransaccion()
        {
            Console.WriteLine("Verificando Transacción");
            // Lógica de verificación de transacción aquí
        }

        // Método para enviar la transacción
        public void EnviarTransaccion()
        {
            Console.WriteLine("Enviando Transacción");
            // Lógica de envío de transacción aquí
        }

        // Método estático para obtener un host autorizador desde la base de datos
        public static HostAutorizador ObtenerHostAutorizadorDesdeBaseDeDatos(int servicioId, DateTime fechaEnvio, DateTime fechaRecepcion, AppDbContext context)
        {
            var servicioModel = context.Servicios
                .Include(s => s.sistema_comunicacion_id)
                .Include(s => s.tipo_mensaje_id)
                .FirstOrDefault(s => s.servicio_id == servicioId);

            if (servicioModel == null)
            {
                throw new InvalidOperationException($"No se encontró el servicio con ID {servicioId}");
            }

            return new HostAutorizador(servicioModel, context, fechaEnvio, fechaRecepcion);
        }
    }
}
