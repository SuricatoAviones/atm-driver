﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    internal class HostAutorizador : Servicio
    {
        public HostAutorizador(string serverIp, int port) : base(serverIp, port)
        {
        }
    }
}
