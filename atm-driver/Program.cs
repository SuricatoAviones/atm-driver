using System;
using Microsoft.Data.SqlClient;

namespace AtmDriver
{
    // Clase de Conexion a Server SQL
    class Conexion
    {
        SqlConnection conex = new SqlConnection();

        static string servidor = "localhost";
        static string bd = "atm-driver";
        static string usuario = "sa";
        static string password = "151199";
        static string puerto = "1433";

        string cadenaConexion = $"Data Source={servidor},{puerto};" +
                                $"User ID={usuario};" +
                                $"Password={password};" +
                                $"Initial Catalog={bd};" +
                                $"Persist Security Info=True";

        public SqlConnection establecerConexion()
        {
            try
            {
                conex.ConnectionString = cadenaConexion;
                conex.Open();
                Console.WriteLine("Se conectó correctamente a la Base de Datos.");
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Error al conectar: {e.Message}");
            }

            return conex;
        }
    }

    // Clase Sistemas de Comunicacion
    public class SistemaComunicacion
    {
        // Atributos (Propiedades)
        public int Id { get; set; }
        public int TipoConexion { get; set; }
        public int TipoConexionTcp { get; set; }
        public string? DireccionIp { get; set; }
        public int PuertoTcp { get; set; }
        public int SocketTcp { get; set; }
        public string? ServidorSoapApi { get; set; }
        public int PuertoSoapApi { get; set; }
        public string? Encabezado { get; set; }
        public int LongitudPaquete { get; set; }
        public bool Ebcdc { get; set; }
        public bool Empaquetado { get; set; }

        // Métodos
        public void Inicializar()
        {
            Console.WriteLine("Sistema de comunicación inicializado.");
        }

        public void Conectar()
        {
            Console.WriteLine("Conectando al sistema de comunicación...");
        }

        public string ObtenerEstadoConexion()
        {
            return "Conexión establecida.";
        }

        public void CerrarConexion()
        {
            Console.WriteLine("Conexión cerrada.");
        }

        public string EnviarMensaje()
        {
            return "Mensaje enviado correctamente.";
        }

        public void EnviarMensajeEsperarRespuesta()
        {
            Console.WriteLine("Enviando mensaje y esperando respuesta...");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Conexion a SQL (clase Conexion)
                Conexion conexion = new Conexion();
                SqlConnection miConexion = conexion.establecerConexion();

                if (miConexion.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Conexión a SQL abierta.");
                }

                // Clase Sistemas Comunicacion
                SistemaComunicacion sistema = new SistemaComunicacion();

                // Inicializar sistema
                sistema.Inicializar();

                // Configurar propiedades
                sistema.DireccionIp = "192.168.1.1";
                sistema.PuertoTcp = 8080;
                sistema.Encabezado = "TestHeader";

                // Conectar al sistema
                sistema.Conectar();

                // Obtener estado
                string estado = sistema.ObtenerEstadoConexion();
                Console.WriteLine($"Estado de la conexión: {estado}");

                // Enviar mensaje
                string respuesta = sistema.EnviarMensaje();
                Console.WriteLine(respuesta);

                // Enviar mensaje esperando respuesta
                sistema.EnviarMensajeEsperarRespuesta();

                // Cerrar conexiones
                sistema.CerrarConexion();

                if (miConexion.State == System.Data.ConnectionState.Open)
                {
                    miConexion.Close();
                    Console.WriteLine("Conexión a SQL cerrada correctamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
            }

            // Mantener la consola abierta
            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
