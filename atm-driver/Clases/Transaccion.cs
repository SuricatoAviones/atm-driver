using System;
using System.Threading.Tasks;
using atm_driver.Data;
using atm_driver.Models;
using atm_driver.Enums;
using Microsoft.EntityFrameworkCore;

namespace atm_driver.Clases
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int NumeroCajero { get; set; }
        public DateTime FechaOperacion { get; set; }
        public DateTime FechaNegocio { get; set; }
        public DateTime FechaMensajeRecibido { get; set; }
        public DateTime FechaMensajeEnviado { get; set; }
        public string Codigo { get; set; } // Enum esperado
        public string EstadoTransaccion { get; set; } // Enum esperado
        public DateTime FechaEnvioEstado { get; set; }
        public string Trace { get; set; }
        public decimal Monto { get; set; }
        public string CodigoRespuesta { get; set; } // Enum esperado
        //Tipo Cuenta Origen
        public string TipoCuenta { get; set; } // Enum esperado
        public string Origen { get; set; }
        //Tipo Cuenta Destino   
        public string TipoCuentaDestino { get; set; } // Enum esperado
        public int NumeroAutorizacion { get; set; }
        public string CodigoMoneda { get; set; } // Enum esperado
        public string Tarjeta { get; set; }
        public string Cedula { get; set; }

        //Datos del Chip de la tarjeta
        public string EMV { get; set; }

        // Variables a procesar de Elementos
        public string LUNO { get; set; }
        public string TimeVariantNumber { get; set; }
        public string TransactionFlag { get; set; }
        public string CoOrdinatorNumber { get; set; }
        public string Track_2 { get; set; }
        public string Track_3 { get; set; }
        public string OperationDataCode { get; set; }
        public string AmountEntryField { get; set; }
        public string PinBufferA { get; set; }
        public string BufferB { get; set; }
        public string BufferC { get; set; }
        public string TransactionStatusData { get; set; }
        public string LastTransactionSerialNumber { get; set; }
        public string LastStatusIssued { get; set; }
        public string LastTransactionNotesDispensed { get; set; }

        public void Inicializar()
        {
            // Inicialización de campos 
            FechaNegocio = DateTime.Today;
            FechaOperacion = DateTime.Today;
            FechaMensajeRecibido = DateTime.Now;
            Trace = "1";
            CodigoRespuesta = Control.CodigoRespuestaInicial; 
        }

        public async Task VerificarTransaccion(AppDbContext context, Cajero cajero, string[] elementos)
        {
            try
            {
                Inicializar();
                // Asignar valores a las propiedades de la transacción desde el array elementos[]
                LUNO = elementos[1];
                TimeVariantNumber = elementos[3];
                TransactionFlag = elementos[4][0].ToString();
                CoOrdinatorNumber = elementos[4][1].ToString();
                Track_2 = elementos[5];
                Track_3 = elementos[6];
                OperationDataCode = elementos[7].Trim();
                AmountEntryField = elementos[8];
                PinBufferA = elementos[9];

                Tarjeta = Control.ExtraerTarjeta(Track_2);

                // Guardar la transacción en la base de datos
                var nuevaTransaccion = new Transacciones_Model
                {
                    // Asignar valores a las propiedades del modelo de transacción
                    transaccion_id = Id, // Este Id probablemente es null o 0 si es autogenerado
                    origen = cajero.Servicio.Codigo,
                    fecha_operacion = FechaOperacion,
                    fecha_negocio = FechaNegocio,
                    fecha_mensaje_recibido = FechaMensajeRecibido,
                    fecha_mensaje_respuesta = FechaMensajeEnviado,
                    monto = (int)Monto,
                    trace = Trace,
                    codigo_respuesta = CodigoRespuesta,
                    tipo_cuenta = TipoCuenta,
                    //origen = Origen,
                    tipo_cuenta_destino = TipoCuentaDestino,
                    NumeroAutorizacion = NumeroAutorizacion,
                    tarjeta = Tarjeta,
                    cajero_id = cajero.Id,
                    denominacion_moneda_id = Control.CodigoMonedaBolivares
                };

                // Agregar al contexto y guardar para obtener el ID
                context.Transacciones.Add(nuevaTransaccion);
                await context.SaveChangesAsync();

                // Actualizar el Id en tu objeto Transaccion con el generado por la base de datos
                Id = nuevaTransaccion.transaccion_id;
                Console.WriteLine("Transacción verificada y guardada correctamente.");

                // Verificacion de Tarjeta Valida
                if (Tarjeta == "")
                {
                    Console.WriteLine("Error: Tarjeta es Vacia la Transaccion es Rechazada");
                    return; 
                }

                // Verificación de OperationDataCode
                var transaccionCodigo = await context.Transaccion_Codigo.FirstOrDefaultAsync(tc => tc.codigo_operacion == OperationDataCode);
                if (transaccionCodigo == null)
                {
                    Console.WriteLine($"Error: El código de operación {OperationDataCode} no existe en la tabla Transaccion_Codigo.");
                    return;
                }

                // Verificar el tipo de transacción
                TipoTransaccion tipoTransaccion;
                if (!Enum.TryParse(transaccionCodigo.tipo_transaccion, out tipoTransaccion))
                {
                    Console.WriteLine($"Error: El tipo de transacción {transaccionCodigo.tipo_transaccion} no es válido.");
                    return;
                }

                // Actualizar la transacción con los valores de tipo_transaccion, tipo_cuenta_origen y tipo_cuenta_destino
                if (transaccionCodigo != null)
                {
                    // No es necesario buscar nuevamente, ya tienes el objeto
                    nuevaTransaccion.codigo = $"{transaccionCodigo.tipo_transaccion}{transaccionCodigo.tipo_cuenta_origen}{transaccionCodigo.tipo_cuenta_destino}";
                    nuevaTransaccion.tipo_cuenta = transaccionCodigo.tipo_cuenta_origen;
                    nuevaTransaccion.tipo_cuenta_destino = transaccionCodigo.tipo_cuenta_destino;

                    // Entity Framework detectará automáticamente los cambios
                    await context.SaveChangesAsync();
                    Console.WriteLine("Transacción actualizada correctamente.");
                }

                //Verificar Journal Printer
                if (!await cajero.VerificarJournalPrinter())
                {
                    Console.WriteLine("Error: La impresora de journal no está disponible.");
                    return;
                }

                // Realizar verificaciones adicionales según el tipo de transacción
                if (tipoTransaccion == TipoTransaccion.RETIRO)
                {
                    if (!await cajero.PuedoDispensarBilletes())
                    {
                        Console.WriteLine("Error: No se pueden dispensar billetes.");
                        return;
                    }


                    // Verificar estado de los cajetines
                    var cajetinesDisponiblesEstado = await cajero.ObtenerEstadoCajetines();
                    Console.WriteLine($"Estado de los cajetines: {string.Join(", ", cajetinesDisponiblesEstado)}");

                    var billetesPorCajetin = await Cajetin.VerificarEfectivo(context, cajero.Id, cajetinesDisponiblesEstado);


                    // Empezar a preparar todo para Dispensar Billetes

                }

                

                // Simular el envío a un host autorizador y recibir una respuesta
                string respuestaHost = await EnviarAHostAutorizador();

                // Procesar la respuesta del host
                await ProcesarResultadoTransaccion(context, cajero, respuestaHost);

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verificando la transacción: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        private async Task<string> EnviarAHostAutorizador()
        {
            // Simular el envío a un host autorizador y recibir una respuesta
            await Task.Delay(1000); // Simular tiempo de espera
            Random random = new Random();
            return random.Next(0, 2) == 0 ? "99" : "00"; // Simular respuesta del host (99: rechazado, 00: aprobado)
        }

        private async Task ProcesarResultadoTransaccion(AppDbContext context, Cajero cajero, string respuestaHost)
        {
            // Procesar la respuesta del host y elaborar la traza a enviar de vuelta al cajero
            if (respuestaHost == Control.CodigoRespuestaAprobado)
            {
                // Transacción aprobada
                CodigoRespuesta = Control.CodigoRespuestaAprobado; // Código de respuesta para aprobado
                EstadoTransaccion = "Aprobada";
            }
            else
            {
                // Transacción rechazada
                CodigoRespuesta = Control.CodigoRespuestaRechazado; // Código de respuesta para rechazado
                EstadoTransaccion = "Rechazada";
            }

            // Crear La Traza de Respuesta
        }

        public string ProcesarResultadoTransaccion()
        {
            // Lógica para obtener resultado, retorna string como ejemplo
            return "Resultado de ejemplo";
        }

        public void GuardarDatos()
        {
            // Lógica para persistencia en base de datos
        }

        public void ActualizarDatos()
        {
            // Lógica para actualización de datos
        }
    }
}
