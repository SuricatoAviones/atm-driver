using atm_driver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    internal class Host_Autorizador : Servicio
    {
        public Host_Autorizador(Servicio_Model servicioModel) : base(servicioModel)
        {
        }
    }
}
