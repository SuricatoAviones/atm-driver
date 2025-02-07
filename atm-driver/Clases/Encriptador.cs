using atm_driver.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace atm_driver.Clases
{
    internal class Encriptador : Servicio
    {
        // Propiedades específicas de Encriptador
        private readonly string _claveMasterKey;
        private readonly DateTime _fechaRecepcionInfo;
        private readonly DateTime _fechaEnvioInfo;
        private readonly string _claveCommunications;

        // Constructor para inicializar desde Servicio_Model
        public Encriptador(Servicio_Model servicioModel, string claveMasterKey, DateTime fechaRecepcionInfo, DateTime fechaEnvioInfo, string claveCommunications)
            : base(servicioModel) // Llamada al constructor de la clase base (Servicio)
        {
            _claveMasterKey = claveMasterKey ?? throw new ArgumentNullException(nameof(claveMasterKey));
            _fechaRecepcionInfo = fechaRecepcionInfo;
            _fechaEnvioInfo = fechaEnvioInfo;
            _claveCommunications = claveCommunications ?? throw new ArgumentNullException(nameof(claveCommunications));
        }

        // Método para inicializar el encriptador
        public new void Inicializar()
        {
            Console.WriteLine("Inicializando Encriptador");
            base.Inicializar(); // Llama al método Inicializar de la clase base (Servicio)
        }

        // Método para validar las claves
        public string ValidarClaves()
        {
            Console.WriteLine("Validando Claves");
            // Lógica de validación aquí
            return "Claves válidas"; // Ejemplo de retorno
        }

        // Método para encriptar la información
        public void EncriptarInformacion()
        {
            Console.WriteLine("Encriptando Información");
            // Lógica de encriptación aquí
        }

        
    }
}