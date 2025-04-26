using atm_driver.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    public class Cajetin : Dispositivo
    {
        // Base de datos
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

        public static async Task ProcesarInformacion(AppDbContext context, int dispositivoId, string[] elementos)
        {
            try
            {
                string data = elementos[3];
                string numeroSerie = data.Substring(1, 4);
                string totalTransacciones = data.Substring(5, 7);

                // Primer grupo de 20: Billetes en cajetín
                int billetesCajetin1 = int.Parse(data.Substring(12, 5));
                int billetesCajetin2 = int.Parse(data.Substring(17, 5));
                int billetesCajetin3 = int.Parse(data.Substring(22, 5));
                int billetesCajetin4 = int.Parse(data.Substring(27, 5));

                // Segundo grupo de 20: Billetes en cajetín de rechazo
                int billetesRechazoCajetin1 = int.Parse(data.Substring(32, 5));
                int billetesRechazoCajetin2 = int.Parse(data.Substring(37, 5));
                int billetesRechazoCajetin3 = int.Parse(data.Substring(42, 5));
                int billetesRechazoCajetin4 = int.Parse(data.Substring(47, 5));

                // Tercer grupo de 20: Billetes dispensados
                int billetesDispensadosCajetin1 = int.Parse(data.Substring(52, 5));
                int billetesDispensadosCajetin2 = int.Parse(data.Substring(57, 5));
                int billetesDispensadosCajetin3 = int.Parse(data.Substring(62, 5));
                int billetesDispensadosCajetin4 = int.Parse(data.Substring(67, 5));

                // Cuarto grupo de 20: Billetes dispensados en la última transacción
                int billetesUltimaTransaccionCajetin1 = int.Parse(data.Substring(72, 5));
                int billetesUltimaTransaccionCajetin2 = int.Parse(data.Substring(77, 5));
                int billetesUltimaTransaccionCajetin3 = int.Parse(data.Substring(82, 5));
                int billetesUltimaTransaccionCajetin4 = int.Parse(data.Substring(87, 5));

                // Actualizar la base de datos
                await ActualizarCajetin(context, dispositivoId, 15, billetesCajetin1, billetesRechazoCajetin1, billetesDispensadosCajetin1, billetesUltimaTransaccionCajetin1);
                await ActualizarCajetin(context, dispositivoId, 16, billetesCajetin2, billetesRechazoCajetin2, billetesDispensadosCajetin2, billetesUltimaTransaccionCajetin2);
                await ActualizarCajetin(context, dispositivoId, 17, billetesCajetin3, billetesRechazoCajetin3, billetesDispensadosCajetin3, billetesUltimaTransaccionCajetin3);
                await ActualizarCajetin(context, dispositivoId, 18, billetesCajetin4, billetesRechazoCajetin4, billetesDispensadosCajetin4, billetesUltimaTransaccionCajetin4);

                await context.SaveChangesAsync();
                Console.WriteLine("Información de los cajetines procesada y guardada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando la información de los cajetines: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        private static async Task ActualizarCajetin(AppDbContext context, int dispositivoId, int codigoDispositivo, int cantidadDisponible, int cantidadRechazada, int cantidadDispensada, int cantidadUltimaTransaccion)
        {
            var dispositivo = await context.Dispositivos.FirstOrDefaultAsync(d => d.codigo == codigoDispositivo);
            if (dispositivo != null)
            {
                var cajeroDispositivo = await context.Cajeros_Dispositivos.FirstOrDefaultAsync(cd => cd.dispositivo_id == dispositivo.dispositivo_id && cd.cajero_id == dispositivoId);
                if (cajeroDispositivo != null)
                {
                    var cajetinDb = await context.Cajetines.FirstOrDefaultAsync(c => c.cajero_dispositivo_id == cajeroDispositivo.cajero_dispositivo_id);

                    if (cajetinDb != null)
                    {
                        cajetinDb.cantidad_disponible = cantidadDisponible;
                        cajetinDb.cantidad_rechazada = cantidadRechazada;
                        cajetinDb.cantidad_dispensada = cantidadDispensada;
                        cajetinDb.cantidad_ultima_transaccion = cantidadUltimaTransaccion;
                    }
                }
            }
        }

        public static async Task<Dictionary<int, int>> VerificarEfectivo(AppDbContext context, int cajeroId, bool[] cajetinesDisponiblesEstado)
        {
            // Crear un diccionario para almacenar la cantidad de billetes por cajetín
            var billetesPorCajetin = new Dictionary<int, int>();

            // Obtener los cajetines asociados al cajero desde la base de datos
            var cajetines = await context.Cajetines
                .Where(c => c.cajetin_id == cajeroId)
                .ToListAsync();

            // Iterar sobre los cajetines y verificar su estado
            for (int i = 0; i < cajetines.Count; i++)
            {
                if (i < cajetinesDisponiblesEstado.Length && cajetinesDisponiblesEstado[i])
                {
                    // Agregar la cantidad disponible al diccionario
                    billetesPorCajetin[cajetines[i].numero_cajetin] = cajetines[i].cantidad_disponible;
                }
                else
                {
                    // Si el cajetín no está disponible, agregarlo con cantidad 0
                    billetesPorCajetin[cajetines[i].numero_cajetin] = 0;
                }
            }

            return billetesPorCajetin;
        }





        public void RecibirInformacion() { }
        public void ProcesarInformacion() { }
        
        public double ActualizarEfectivo() => CantidadDisponible - CantidadDispensada;
    }
}
