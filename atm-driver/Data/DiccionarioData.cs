using System;
using System.Collections.Generic;

namespace atm_driver.Data
{
    internal class DiccionarioData
    {
        // Diccionario para solo usar un replace de los Ascii
        public static readonly Dictionary<string, string> Reemplazos = new()
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

        public static readonly Dictionary<string, string> IllegalCommands = new Dictionary<string, string>
        {
            {"A","Comando Rechazado (Ilegal)" },
            /*
            { "A01", "Error de longitud del mensaje." },
            { "A02", "Separador de campo no encontrado o encontrado inesperadamente." },
            { "A03", "Demasiados grupos de impresión en un mensaje de Respuesta de Transacción." },
            { "A04", "Separador de grupo faltante o encontrado en una posición inesperada." },
            { "B01", "Clase de mensaje ilegal." },
            { "B02", "Subclase de mensaje o identificador ilegal." },
            { "B03", "Modificador de cambio de clave de cifrado ilegal." },
            { "B04", "Código de comando del terminal ilegal." },
            { "B05", "Modificador de comando del terminal ilegal." },
            { "B06", "Identificador de función de respuesta de transacción ilegal." },
            { "B07", "Campo de datos contiene un carácter no decimal." },
            { "B08", "Valor del campo fuera de rango." },
            { "B09", "Número de coordinación de mensaje inválido." },
            */
            {"C","Comando Especifico Rechazado" },
            /*
            { "C01", "Mensaje aceptado solo en servicio y esperando respuesta de transacción." },
            { "C02", "Mensaje no aceptado mientras se ejecutan diagnósticos o programas de borrado." },
            { "C03", "Mensaje no aceptado en los modos 'Fuera de Servicio' o 'Suministro'." },
            { "C04", "Mensaje inaceptable en el modo actual." },
            { "C08", "Mensaje de inicialización de clave rechazado porque ya está en estado válido." },
            { "1", "Fallo de autenticación MAC." }
            */
        };
    }
}
