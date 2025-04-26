using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    public class Dispositivo
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string EstadoDispositivo { get; set; }
        public string EstadoSuministro { get; set; }

        public Dispositivo(int id, int codigo, string nombre, string estadoDispositivo, string estadoSuministro)
        {
            Id = id;
            Codigo = codigo;
            Nombre = nombre;
            EstadoDispositivo = estadoDispositivo;
            EstadoSuministro = estadoSuministro;
        }

        public static async Task ProcesarInformacion(AppDbContext context, int dispositivoId, string[] elementos)
        {
            try
            {
                string estadosDispositivos = elementos[4];
                string estadosSuministros = elementos[6];

                // Calcular el número de dispositivos en la tabla Dispositivos
                int numeroDispositivos = await context.Dispositivos.CountAsync();

                // Completar con ceros si es necesario
                if (estadosDispositivos.Length < numeroDispositivos)
                {
                    estadosDispositivos = estadosDispositivos.PadRight(numeroDispositivos, '0');
                }

                if (estadosSuministros.Length < numeroDispositivos)
                {
                    estadosSuministros = estadosSuministros.PadRight(numeroDispositivos, '0');
                }

                for (int i = 0; i < numeroDispositivos; i++)
                {
                    int estadoDispositivo = int.Parse(estadosDispositivos[i].ToString());
                    int estadoSuministro = int.Parse(estadosSuministros[i].ToString());

                    await ActualizarEstadoDispositivo(context, dispositivoId, i, estadoDispositivo, estadoSuministro);
                }

                await context.SaveChangesAsync();
                Console.WriteLine("Información de los dispositivos procesada y guardada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando la información de los dispositivos: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        private static async Task ActualizarEstadoDispositivo(AppDbContext context, int dispositivoId, int codigoDispositivo, int estadoDispositivo, int estadoSuministro)
        {
            var dispositivo = await context.Dispositivos.FirstOrDefaultAsync(d => d.codigo == codigoDispositivo);
            if (dispositivo != null)
            {
                var cajeroDispositivo = await context.Cajeros_Dispositivos.FirstOrDefaultAsync(cd => cd.dispositivo_id == dispositivo.dispositivo_id && cd.cajero_id == dispositivoId);
                if (cajeroDispositivo != null)
                {
                    cajeroDispositivo.estado_dispositivo = estadoDispositivo;
                    cajeroDispositivo.estado_suministro = estadoSuministro;
                }
            }
        }

        public virtual void Inicializar() { }
        public virtual void ActualizarSuministro() { }
        public virtual string VerificarDispositivo() => EstadoDispositivo;
        public virtual string VerificarSuministro() => EstadoSuministro;
        public virtual void EnviarInformacion() { }
    }
}
