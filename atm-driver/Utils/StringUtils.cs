using System;
using System.Collections.Generic;

namespace atm_driver.Utils
{
    public static class StringUtils
    {
        // Diccionario para solo usar un replace
        private static readonly Dictionary<string, string> Reemplazos = new()
        {
            { "#28", ((char)28).ToString() },
            { "#12", ((char)12).ToString() },
            { "#15", ((char)15).ToString() },
            { "#27", ((char)27).ToString() },
            { "#14", ((char)14).ToString() },
            { "#08", ((char)8).ToString() },
            { "#13", ((char)13).ToString() },
            { "#09", ((char)9).ToString() },
            { "#10", ((char)10).ToString() },
            { "#11", ((char)11).ToString() },
            { "#27#194", ((char)27).ToString() + ((char)194).ToString() },
            { "#27#196", ((char)27).ToString() + ((char)196).ToString() },
            { "#27#199", ((char)27).ToString() + ((char)199).ToString() },
            { "#27#201", ((char)27).ToString() + ((char)201).ToString() },
            { "#27#211#27#216", ((char)27).ToString() + ((char)211).ToString() + ((char)27).ToString() + ((char)216).ToString() },
            { "#27#216", ((char)27).ToString() + ((char)216).ToString() },
            { "#27#229", ((char)27).ToString() + ((char)229).ToString() }
        };

        public static string FormatDownloadString(string linea)
        {
            foreach (var (clave, valor) in Reemplazos)
            {
                linea = linea.Replace(clave, valor);
            }
            return linea;
        }
    }
}
