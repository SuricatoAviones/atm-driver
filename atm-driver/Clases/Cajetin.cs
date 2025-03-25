using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    public class Cajetin : Dispositivo
    {
        public string Denominacion { get; set; }
        public string TipoDenominacion { get; set; }
        public string Tipo { get; set; }
        public double CantidadDisponible { get; set; }
        public double CantidadDispensada { get; set; }
        public double CantidadRechazada { get; set; }
        public DateTime FechaHabilitacion { get; set; }
        public int NumeroCajetin { get; set; }

        public Cajetin(int id, int codigo, string nombre, string estadoDispositivo, string estadoSuministro,
                       string denominacion, string tipoDenominacion, string tipo, double cantidadDisponible,
                       double cantidadDispensada, double cantidadRechazada, DateTime fechaHabilitacion, int numeroCajetin)
            : base(id, codigo, nombre, estadoDispositivo, estadoSuministro)
        {
            Denominacion = denominacion;
            TipoDenominacion = tipoDenominacion;
            Tipo = tipo;
            CantidadDisponible = cantidadDisponible;
            CantidadDispensada = cantidadDispensada;
            CantidadRechazada = cantidadRechazada;
            FechaHabilitacion = fechaHabilitacion;
            NumeroCajetin = numeroCajetin;
        }

        public override void Inicializar() { }
        public void RecibirInformacion() { }
        public void ProcesarInformacion() { }
        public double VerificarEfectivo() => CantidadDisponible;
        public double ActualizarEfectivo() => CantidadDisponible - CantidadDispensada;
    }
}
