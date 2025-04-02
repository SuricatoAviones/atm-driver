using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    public class Dispositivo
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string EstadoDispositivo { get; set; }
        public string EstadoSuministro { get; set; }

        public Dispositivo(int id, int codigo, string nombre, string estadoDispositivo, string estadoSuministro)
        {
            Id = id;
            Codigo = codigo;
            Nombre = nombre;
            EstadoDispositivo = estadoDispositivo;
            EstadoSuministro = estadoSuministro;
        }

        public virtual void Inicializar() { }
        public virtual void ActualizarDispositivo()
        {

        }
        public virtual void ActualizarSuministro() { 
        }
        public virtual string VerificarDispositivo() => EstadoDispositivo;
        public virtual string VerificarSuministro() => EstadoSuministro;
        public virtual void EnviarInformacion() { }
    }
}
