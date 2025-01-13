using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    internal class Encriptador : Servicio
    {
        public Encriptador(string serverIp, int port) : base(serverIp, port)
        {
        }
    }
}
