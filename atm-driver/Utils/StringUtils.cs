using System;
using System.Collections.Generic;
using atm_driver.Data;

namespace atm_driver.Utils
{
    public static class StringUtils
    {
        public static string FormatDownloadNDC(string linea)
        {
            foreach (var (clave, valor) in DiccionarioData.Reemplazos)
            {
                linea = linea.Replace(clave, valor);
            }
            return linea;
        }

        public static string FormatDownloadTCS(string linea)
        {
            foreach (var (clave, valor) in DiccionarioData.Reemplazos)
            {
                linea = linea.Replace(clave, valor);
            }
            return linea;
        }
    }
}