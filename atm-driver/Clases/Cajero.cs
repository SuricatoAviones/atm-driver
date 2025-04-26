using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using atm_driver.Data;
using atm_driver.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using atm_driver.Clases;
using atm_driver.Enums;
namespace atm_driver.Clases
{
    public class Cajero
    {
        // Atributos
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string ClaveComunicacion { get; set; }
        public string ClaveMasterKey { get; set; }
        public string Localizacion { get; set; }
        public string Estado { get; set; }
        public NetworkStream StreamComunicacion { get; set; }
        public int DownloadId { get; set; }
        public TcpClient Cliente { get; set; }
        public Sistemas_Comunicacion SistemasComunicacion { get; set; }
        public AppDbContext Context { get; set; }
        public Servicio? Servicio { get; set; }

        public EstadoCajero estadoCajero;

        private bool _downloadEnProgreso = false;
        private Download _downloadActual;

        // Métodos
        public void Inicializar(string type)
        {
            // Implementación del método
        }

        public async Task OnService()
        {
            if (SistemasComunicacion == null)
            {
                Console.WriteLine("Error: SistemasComunicacion no está inicializado.");
                return;
            }
            string mensaje = "1" + (char)28 + (char)28 + (char)28 + "1";
            await SistemasComunicacion.EnviarMensaje(mensaje, this, Cliente.GetStream());
            estadoCajero = EstadoCajero.InService;
            Console.WriteLine(estadoCajero + "Estado Cajero");
            Console.WriteLine("Onservice Comando Enviado");
        }

        public async Task OutService()
        {
            if (SistemasComunicacion == null)
            {
                Console.WriteLine("Error: SistemasComunicacion no está inicializado.");
                return;
            }

            string mensaje = "1" + (char)28 + (char)28 + (char)28 + "2";
            await SistemasComunicacion.EnviarMensaje(mensaje, this, Cliente.GetStream());
            estadoCajero = EstadoCajero.OutService;
            Console.WriteLine(estadoCajero + "Estado Cajero");
            Console.WriteLine("Outservice Comando Enviado");
        }

        

        public async Task GetSupply()
        {
            if (SistemasComunicacion == null)
            {
                Console.WriteLine("Error: SistemasComunicacion no está inicializado.");
                return;
            }

            string mensaje = "1" + (char)28 + (char)28 + (char)28 + "4";
            await SistemasComunicacion.EnviarMensaje(mensaje, this, Cliente.GetStream());
            estadoCajero = EstadoCajero.GetSupply;
            Console.WriteLine(estadoCajero + "Estado Cajero");
            Console.WriteLine("GetSupply Comando Enviado");
        }

        public async Task GetConfigurationInformation()
        {
            if (SistemasComunicacion == null)
            {
                Console.WriteLine("Error: SistemasComunicacion no está inicializado.");
                return;
            }
            string mensaje = "1" + (char)28 + (char)28 + (char)28 + "7";
            await SistemasComunicacion.EnviarMensaje(mensaje, this, Cliente.GetStream());
            estadoCajero = EstadoCajero.GetConfigurationInformation;
            Console.WriteLine(estadoCajero + "Estado Cajero");
            Console.WriteLine("GetConfigurationInformation Comando Enviado");
        }

        public async Task EnviarDownload(Cajero cajero)
        {
            // Iniciar el proceso de descarga
            _downloadActual = new Download();
            await _downloadActual.Inicializar(cajero.DownloadId, Context);

            // Verificar que el stream no sea nulo
            if (StreamComunicacion == null)
            {
                Console.WriteLine("Error: StreamComunicacion es nulo en EnviarDownload");
                return;
            }

            // Iniciar estado de descarga
            _downloadEnProgreso = true;
            estadoCajero = EstadoCajero.Download;

            // Iniciar el proceso de envío de la primera línea
            bool downloadIniciado = await _downloadActual.IniciarDownload(cajero, cajero.SistemasComunicacion, StreamComunicacion);

            if (!downloadIniciado)
            {
                // No hay líneas para enviar
                Evento.GuardarEvento(CodigoEvento.Download, "Download terminado (no hay líneas)", cajero.Id, SistemasComunicacion.ServicioId);
                _downloadEnProgreso = false;
                estadoCajero = EstadoCajero.Normal;
                await cajero.OnService();
            }
        }

        public async Task ProcesarMensaje(string mensajeRecibido, Cajero cajero)
        {
            string[] elementos = mensajeRecibido.Split((char)28);
            Console.WriteLine($"Elementos del mensaje: {string.Join(", ", elementos)}");

            // Busca el id del cajero en la base de datos
            var cajeroDb = await Context.Cajeros.FindAsync(Id);

            // Evalua si es mensaje Solicitado o No Solicitado
            if (elementos[0] == "22")
            {
                Console.WriteLine("Es Un Mensaje Solicitado");

                // Se Procede a Evaluar el estado actual del cajero
                if (estadoCajero == EstadoCajero.InService)
                {
                    Console.WriteLine("Esta en el PROCESO DE IN SERVICE ENTONCES SIGNIFICA QUE ES LA RESPUESTA DEL INSERVICE");
                    if (elementos[3] == "9")
                    {
                        Console.WriteLine("El Cajero se Puso En Servicio");
                        await ActualizarEstadoCajero(EstadoCajeroEvento.EnServicio, "El Cajero Se Coloco en Linea");
                    }
                    else
                    {
                        Console.WriteLine("El Cajero se Puso Fuera de Servicio");
                        await ActualizarEstadoCajero(EstadoCajeroEvento.FueraDeServicio, "El Cajero no Logro Ponerse en Linea");
                    }
                    estadoCajero = EstadoCajero.Normal;
                }
                else if (estadoCajero == EstadoCajero.OutService)
                {
                    if (elementos[3] == "9")
                    {
                        Console.WriteLine("El Cajero se Puso Fuera Fuera de Servicio");
                        await ActualizarEstadoCajero(EstadoCajeroEvento.FueraDeServicio, "El Cajero Se Coloco en Fuera de Servicio");
                    }
                }
                else if (estadoCajero == EstadoCajero.GetSupply)
                {
                    if (elementos[2] == "F") // COMANDO DE TERMINAL STATE
                    {
                        // Verificar que sea de Supply Counters
                        if (elementos[3].StartsWith("2"))
                        {
                            Console.WriteLine("ES UN MENSAJE DE CONTADORES");
                            await Cajetin.ProcesarInformacion(Context, Id, elementos);
                        }
                    }
                }
                else if (estadoCajero == EstadoCajero.GetConfigurationInformation)
                {
                    if (elementos[2] == "F") // COMANDO DE TERMINAL STATE
                    {
                        // Verificar que sea de Send Configuration Information Terminal Command message
                        if (elementos[3].StartsWith("1"))
                        {
                      
                            Console.WriteLine("Send Configuration Information Terminal Command message");
                            Console.WriteLine(elementos);
                            await Dispositivo.ProcesarInformacion(Context, Id, elementos);
                        }
                    }
                }
                else if (estadoCajero == EstadoCajero.Download && _downloadActual != null)
                {
                    Console.WriteLine("Procesando respuesta de Download");

                    // Verificar si hay comandos ilegales en la respuesta
                    bool detenerDownload = await _downloadActual.ProcesarRespuesta(mensajeRecibido, this, SistemasComunicacion);

                    if (detenerDownload || _downloadActual.DownloadDetenido())
                    {
                        Console.WriteLine("Download detenido por comando ilegal");
                        _downloadEnProgreso = false;
                        estadoCajero = EstadoCajero.Normal;
                        await OnService(); // Poner el cajero en servicio tras detener el download
                        return;
                    }

                    if (elementos[3] == "9")
                    {
                        Console.WriteLine("Respuesta Positiva - Avanzando a la siguiente línea");

                        // Avanzar a la siguiente línea del download
                        _downloadActual.AvanzarALaSiguienteLinea();

                        // Verificar si hay más líneas para enviar
                        if (_downloadActual.HayMasLineas())
                        {
                            // Enviar la siguiente línea
                            await _downloadActual.EnviarSiguienteLinea(this, SistemasComunicacion, StreamComunicacion);
                        }
                        else
                        {
                            // Download completado
                            Console.WriteLine("Download completado con éxito!");

                            // Guardar evento de acuerdo al formato del cajero
                            string tipoDownload = _downloadActual.FormatoCajeroId == 1 ? "NDC" : "TCS";
                            Evento.GuardarEvento(CodigoEvento.Download, $"Download {tipoDownload} terminado exitosamente", Id, SistemasComunicacion.ServicioId);

                            _downloadEnProgreso = false;
                            estadoCajero = EstadoCajero.Normal;
                            await OnService(); // Poner el cajero en servicio tras completar el download
                        }
                    }
                    else
                    {
                        Console.WriteLine("Comando Ilegal o rechazado durante el download");

                        // Registrar el evento
                        string tipoDownload = _downloadActual.FormatoCajeroId == 1 ? "NDC" : "TCS";
                        Evento.GuardarEvento(CodigoEvento.Download, $"Comando rechazado durante download {tipoDownload}: {mensajeRecibido}", Id, SistemasComunicacion.ServicioId);

                        // Dependiendo de la política: intentar la siguiente línea o abortar
                        _downloadActual.AvanzarALaSiguienteLinea();

                        if (_downloadActual.HayMasLineas())
                        {
                            await _downloadActual.EnviarSiguienteLinea(this, SistemasComunicacion, StreamComunicacion);
                        }
                        else
                        {
                            _downloadEnProgreso = false;
                            estadoCajero = EstadoCajero.Normal;
                            await OnService();
                        }
                    }
                }
            }
            else if (elementos[0] == "12")
            {
                Console.WriteLine("Mensaje No Solicitado");
                // Procesar mensaje no solicitado si es necesario
            }
            else if (elementos[0] == "11")
            {
                Console.WriteLine("Mensaje de Transaction Request");
                // Procesar Transaccion 
                Console.WriteLine(elementos);
                await new Transaccion().VerificarTransaccion(Context, cajero, elementos);
            }
        }

        // Método para actualizar el estado del cajero en la base de datos
        public async Task ActualizarEstadoCajero(EstadoCajeroEvento nuevoEstado, string mensajeEvento)
        {
            try
            {
                var cajeroDb = await Context.Cajeros.FindAsync(Id);
                if (cajeroDb != null)
                {
                    // Convertir el enum a su valor numérico
                    cajeroDb.estado = ((int)nuevoEstado).ToString();
                    await Context.SaveChangesAsync();
                    Console.WriteLine($"Estado del cajero {Id} actualizado a '{(int)nuevoEstado}' en la base de datos.");
                    Evento.GuardarEvento(CodigoEvento.Comunicaciones, mensajeEvento, cajeroDb.cajero_id, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error actualizando estado del cajero: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        public async Task<string> VerificarEstado()
        {
            // Busca el cajero en la base de datos
            var cajeroDb = await Context.Cajeros.FindAsync(Id);
            if (cajeroDb != null)
            {
                // Devuelve el estado del cajero
                return cajeroDb.estado;
            }
            else
            {
                Console.WriteLine($"Error: No se encontró el cajero con ID {Id} en la base de datos.");
                return "Estado no disponible";
            }
        }
        //Metodos de Verificacion de Dispositivos

        public async Task<bool> PuedoDispensarBilletes()
        {
            var dispositivo = await Context.Dispositivos.FirstOrDefaultAsync(d => d.codigo == 4);
            if (dispositivo != null)
            {
                var cajeroDispositivo = await Context.Cajeros_Dispositivos.FirstOrDefaultAsync(cd => cd.dispositivo_id == dispositivo.dispositivo_id && cd.cajero_id == Id);
                if (cajeroDispositivo != null)
                {
                    int estadoDispositivo = cajeroDispositivo.estado_dispositivo ?? 0;
                    return estadoDispositivo == 0 || estadoDispositivo == 1;
                }
            }
            return false;
        }

        public async Task<bool[]> ObtenerEstadoCajetines()
        {
            var codigosCajetines = new[] { 15, 16, 17, 18 };
            var estadoCajetines = new bool[codigosCajetines.Length];

            for (int i = 0; i < codigosCajetines.Length; i++)
            {
                var dispositivo = await Context.Dispositivos.FirstOrDefaultAsync(d => d.codigo == codigosCajetines[i]);

                if (dispositivo != null)
                {
                    var cajeroDispositivo = await Context.Cajeros_Dispositivos
                        .FirstOrDefaultAsync(cd => cd.dispositivo_id == dispositivo.dispositivo_id && cd.cajero_id == Id);

                    if (cajeroDispositivo != null)
                    {
                        int estadoDispositivo = cajeroDispositivo.estado_dispositivo ?? -1;
                        estadoCajetines[i] = estadoDispositivo == 0 || estadoDispositivo == 1;
                    }
                    else
                    {
                        estadoCajetines[i] = false;
                    }
                }
                else
                {
                    estadoCajetines[i] = false;
                }
            }

            return estadoCajetines;
        }


        public async Task<bool> VerificarJournalPrinter()
        {
            var dispositivo = await Context.Dispositivos.FirstOrDefaultAsync(d => d.codigo == 7);
            if (dispositivo != null)
            {
                var cajeroDispositivo = await Context.Cajeros_Dispositivos.FirstOrDefaultAsync(cd => cd.dispositivo_id == dispositivo.dispositivo_id && cd.cajero_id == Id);
                if (cajeroDispositivo != null)
                {
                    int estadoDispositivo = cajeroDispositivo.estado_dispositivo ?? 0;
                    return estadoDispositivo == 0 || estadoDispositivo == 1;
                }
            }
            return false;
        }

        public async Task<bool> VerificarReceiptPrinter()
        {
            var dispositivo = await Context.Dispositivos.FirstOrDefaultAsync(d => d.codigo == 6);
            if (dispositivo != null)
            {
                var cajeroDispositivo = await Context.Cajeros_Dispositivos.FirstOrDefaultAsync(cd => cd.dispositivo_id == dispositivo.dispositivo_id && cd.cajero_id == Id);
                if (cajeroDispositivo != null)
                {
                    int estadoDispositivo = cajeroDispositivo.estado_dispositivo ?? 0;
                    return estadoDispositivo == 0 || estadoDispositivo == 1;
                }
            }
            return false;
        }


        public async Task<Dictionary<int, int>> DeterminarBilletesAdispensar(decimal monto)
        {
            try
            {
                // Verificar si se pueden dispensar billetes
                if (!await PuedoDispensarBilletes())
                {
                    Console.WriteLine("Error: No se pueden dispensar billetes.");
                    return null;
                }

                // Obtener el estado de los cajetines
                var cajetinesDisponiblesEstado = await ObtenerEstadoCajetines();
                Console.WriteLine($"Estado de los cajetines: {string.Join(", ", cajetinesDisponiblesEstado)}");

                // Obtener las denominaciones disponibles y la cantidad de billetes por cajetín
                var cajetinesDenominacion = await ObtenerDenominacionesCajetines();
                var cantidadBilletesPorCajetin = await Cajetin.VerificarEfectivo(Context, Id, cajetinesDisponiblesEstado);

                // Convertir a los formatos necesarios para el algoritmo
                var denominaciones = new List<int>();
                var disponibilidad = new List<int>();

                foreach (var cajetin in cajetinesDenominacion)
                {
                    if (cantidadBilletesPorCajetin.ContainsKey(cajetin.Key))
                    {
                        denominaciones.Add(cajetin.Value);
                        disponibilidad.Add(cantidadBilletesPorCajetin[cajetin.Key]);
                    }
                }

                // Ejecutar el algoritmo de dispensación distribuida
                var resultadoDispensacion = DispensarDistribuido((int)monto, denominaciones, disponibilidad);

                // Convertir el resultado a un formato utilizable por el cajero
                var resultado = new Dictionary<int, int>();
                for (int i = 0; i < resultadoDispensacion.Count; i++)
                {
                    int numeroCajetin = -1;
                    foreach (var cajetin in cajetinesDenominacion)
                    {
                        if (cajetin.Value == resultadoDispensacion[i][0])
                        {
                            numeroCajetin = cajetin.Key;
                            break;
                        }
                    }

                    if (numeroCajetin != -1)
                    {
                        resultado[numeroCajetin] = resultadoDispensacion[i][1];
                    }
                }

                // Validar si se pudo dispensar el monto completo
                int montoDispensado = 0;
                foreach (var item in resultadoDispensacion)
                {
                    montoDispensado += item[0] * item[1];
                }

                if (montoDispensado != (int)monto)
                {
                    Console.WriteLine($"Advertencia: No se pudo dispensar el monto completo. Solicitado: {monto}, Dispensado: {montoDispensado}");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al determinar billetes a dispensar: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return null;
            }
        }

        /// <summary>
        /// Obtiene las denominaciones asociadas a cada cajetín del cajero.
        /// </summary>
        /// <summary>
        /// Obtiene las denominaciones asociadas a cada cajetín del cajero.
        /// </summary>
        private async Task<Dictionary<int, int>> ObtenerDenominacionesCajetines()
        {
            var resultado = new Dictionary<int, int>();

            try
            {
                // Obtener las denominaciones de los cajetines desde la base de datos
                var cajetines = await Context.Cajetines
                    .Where(c => c.cajero_dispositivo_id == Id)
                    .ToListAsync();

                foreach (var cajetin in cajetines)
                {
                    // Agregar la denominación directamente al diccionario
                    resultado[cajetin.numero_cajetin] = cajetin.denominacion ?? 0; // Usar 0 si la denominación es null
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener denominaciones de cajetines: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }

            return resultado;
        }


        /// <summary>
        /// Algoritmo para dispensar billetes de manera distribuida según la disponibilidad de cada denominación.
        /// </summary>
        /// <param name="monto">Monto total a dispensar</param>
        /// <param name="denominaciones">Lista de denominaciones disponibles</param>
        /// <param name="disponibilidad">Lista de cantidades disponibles para cada denominación</param>
        /// <returns>Lista de pares [denominación, cantidad] a dispensar</returns>
        private List<int[]> DispensarDistribuido(int monto, List<int> denominaciones, List<int> disponibilidad)
        {
            var resultado = new List<int[]>();
            int montoRestante = monto;
            
            if (denominaciones.Count == 0 || disponibilidad.Count == 0 || denominaciones.Count != disponibilidad.Count)
            {
                Console.WriteLine("Error: Denominaciones o disponibilidad inválidas.");
                return resultado;
            }

            // Calcular porcentaje objetivo para cada denominación
            var porcentajeObjetivo = new List<double>();
            int totalDisponible = disponibilidad.Sum();
            
            if (totalDisponible == 0)
            {
                Console.WriteLine("Error: No hay billetes disponibles.");
                return resultado;
            }

            // Calcular porcentajes de distribución
            for (int i = 0; i < denominaciones.Count; i++)
            {
                porcentajeObjetivo.Add((double)disponibilidad[i] / totalDisponible);
            }

            // Primera pasada: asignar según porcentajes
            for (int i = 0; i < denominaciones.Count; i++)
            {
                double montoObjetivo = monto * porcentajeObjetivo[i];
                int cantidadBilletes = (int)Math.Min(Math.Floor(montoObjetivo / denominaciones[i]), disponibilidad[i]);
                
                if (cantidadBilletes > 0)
                {
                    resultado.Add(new int[] { denominaciones[i], cantidadBilletes });
                    montoRestante -= denominaciones[i] * cantidadBilletes;
                    disponibilidad[i] -= cantidadBilletes;
                }
            }

            // Segunda pasada: completar monto restante usando el algoritmo de mínimos billetes
            if (montoRestante > 0)
            {
                var resultadoAdicional = DispensarMinimoBilletes(montoRestante, denominaciones, disponibilidad);
                resultado.AddRange(resultadoAdicional);
            }

            return resultado;
        }

        /// <summary>
        /// Algoritmo para dispensar el mínimo número de billetes para un monto dado.
        /// </summary>
        private List<int[]> DispensarMinimoBilletes(int monto, List<int> denominaciones, List<int> disponibilidad)
        {
            var resultado = new List<int[]>();
            int montoRestante = monto;

            // Ordenar las denominaciones de mayor a menor
            var denominacionesOrdenadas = denominaciones.Select((valor, indice) => new { Valor = valor, Indice = indice })
                                                       .OrderByDescending(x => x.Valor)
                                                       .ToList();

            foreach (var den in denominacionesOrdenadas)
            {
                int cantidadNecesaria = montoRestante / den.Valor;
                int cantidadDisponible = disponibilidad[den.Indice];
                int cantidadUsar = Math.Min(cantidadNecesaria, cantidadDisponible);

                if (cantidadUsar > 0)
                {
                    resultado.Add(new int[] { den.Valor, cantidadUsar });
                    montoRestante -= den.Valor * cantidadUsar;
                    disponibilidad[den.Indice] -= cantidadUsar;
                }
            }

            // Si aún queda monto por dispensar, intentar una segunda pasada con denominaciones más pequeñas
            if (montoRestante > 0)
            {
                bool dispensado = false;
                
                // Ordenar de menor a mayor para intentar completar
                var denMenorAMayor = denominacionesOrdenadas.OrderBy(x => x.Valor).ToList();
                
                foreach (var den in denMenorAMayor)
                {
                    while (montoRestante >= den.Valor && disponibilidad[den.Indice] > 0)
                    {
                        // Buscar si ya existe esta denominación en el resultado
                        var existente = resultado.FirstOrDefault(item => item[0] == den.Valor);
                        if (existente != null)
                        {
                            existente[1]++;
                        }
                        else
                        {
                            resultado.Add(new int[] { den.Valor, 1 });
                        }
                        
                        montoRestante -= den.Valor;
                        disponibilidad[den.Indice]--;
                        dispensado = true;
                    }
                }
                
                if (!dispensado && montoRestante > 0)
                {
                    Console.WriteLine($"Advertencia: No se puede dispensar el monto completo. Faltan {montoRestante} unidades.");
                }
            }

            return resultado;
        }

        /// <summary>
        /// Suma todos los elementos de una lista.
        /// </summary>
        private int SumarTodos(List<int> lista)
        {
            int total = 0;
            foreach (var item in lista)
            {
                total += item;
            }
            return total;
        }


        // Resto de los métodos sin cambios
        public void ReportarTransaction() { /* Implementación del método */ }
        public void EnviarMensajeSolicitado() { /* Implementación del método */ }
        public void EnviarMensajeNoSolicitado() { /* Implementación del método */ }
        public void ProcesarMensajeSolicitado() { /* Implementación del método */ }
        public void ProcesarMensajeNoSolicitado() { /* Implementación del método */ }
        public void ProcesarMensaje() { /* Implementación del método */ }
        public void ConstruirRecibo() { /* Implementación del método */ }
    }
}