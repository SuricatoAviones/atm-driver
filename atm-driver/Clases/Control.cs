namespace atm_driver.Clases
{
    public class Control
    {
        public const int BufferSize = 1024;

        //Codigos de Respuesta Fijos
        public const string CodigoRespuestaInicial = "80";
        public const string CodigoRespuestaAprobado = "00";
        public const string CodigoRespuestaRechazado = "99";

        // Codigo ID de las Monedas en la BD
        public const int CodigoMonedaBolivares = 1; 


        // Constantes de Servicios
        public const string ServicioRedCajerosNDC = "01";

        public static string ExtraerTarjeta(string texto)
        {
            int indiceIgual = texto.IndexOf('=');

            if (indiceIgual > 0)
            {
                // Extraer la parte del string hasta antes del '='
                return texto.Substring(1, indiceIgual - 1);
            }

            return ""; // Devuelve vacio si no se encuentra '='
        }


    }


}
