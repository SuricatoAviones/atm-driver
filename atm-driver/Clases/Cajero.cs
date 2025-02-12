using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    internal class Cajero
    {
        // Atributos
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string ClaveComunicacion { get; set; }
        public string ClaveMasterKey { get; set; }
        public string Localizacion { get; set; }
        public string Estado { get; set; }

        // Métodos
        public void Inicializar(string type)
        {
            // Implementación del método
        }

        public string VerificarEstado()
        {
            // Implementación del método
            return Estado;
        }

        public void ReportarTransaction()
        {
            // Implementación del método
        }

        public void DeterminarBilletesAdispensar()
        {
            // Implementación del método
        }

        public void EnviarMensajeSolicitado()
        {
            // Implementación del método
        }

        public void EnviarMensajeNoSolicitado()
        {
            // Implementación del método
        }

        public string ActualizarEstadoCajero()
        {
            // Implementación del método
            return Estado;
        }

        public void ProcessarMensajeSolicitado()
        {
            // Implementación del método
        }

        public void ProcessarMensajeNoSolicitado()
        {
            // Implementación del método
        }

        public void ProcessarMensaje()
        {
            // Implementación del método
        }

        public void ConstruirRecibo()
        {
            // Implementación del método
        }
    }
}