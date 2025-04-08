using atm_driver.Models;
using Microsoft.EntityFrameworkCore;

namespace atm_driver.Clases
{
    public class AppDbContext : DbContext
    {
        // DbSet para los modelos
        public DbSet<Sistemas_Comunicacion_Model> SistemasComunicacion { get; set; }
        public DbSet<Cajeros_Model> Cajeros { get; set; }
        public DbSet<Tipo_Mensaje_Model> Tipo_Mensajes { get; set; }
        public DbSet<Servicio_Model> Servicios { get; set; }
        public DbSet<Mensaje_Model> Mensajes { get; set; }
        public DbSet<Control_Model> Control { get; set; }
        public DbSet<Denominaciones_Monedas_Model> Denominaciones_Monedas { get; set; }
        public DbSet<Dispositivos_Model> Dispositivos { get; set; }
        public DbSet<Download_Model> Download { get; set; }
        public DbSet<Eventos_Model> Eventos { get; set; }
        public DbSet<Formato_Cajero_Model> Formato_Cajero { get; set; }
        public DbSet<Transacciones_Model> Transacciones {get; set; }
        public DbSet<Rol_Model> Roles { get; set; }
        public DbSet<Usuarios_Model> Usuarios { get; set; }
        public DbSet<Keys_Model> Keys { get; set; }
        public DbSet<Tipo_Evento_Model> Tipo_Eventos { get; set; }
        public DbSet<Codigos_Evento_Model> Codigos_Eventos { get; set; }
        public DbSet<Cajetin_Model> Cajetines { get; set; }
        public DbSet<Cajeros_Dispositivos_Model> Cajeros_Dispositivos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cajeros_Model>()
                .HasOne(c => c.Download)
                .WithMany()
                .HasForeignKey(c => c.download_id);

            modelBuilder.Entity<Cajeros_Model>()
                .HasOne(c => c.Key)
                .WithMany()
                .HasForeignKey(c => c.key_id);

            modelBuilder.Entity<Cajetin_Model>()
                .HasOne(c => c.Cajeros_Dispositivos)
                .WithMany()
                .HasForeignKey(c => c.cajero_dispositivo_id);

            modelBuilder.Entity<Cajeros_Dispositivos_Model>()
                .HasOne(d => d.Cajero)
                .WithMany()
                .HasForeignKey(d => d.cajero_id);
            modelBuilder.Entity<Cajeros_Dispositivos_Model>()
               .HasOne(d => d.Dispositivo)
               .WithMany()
               .HasForeignKey(d => d.dispositivo_id);

            modelBuilder.Entity<Cajetin_Model>()
                .HasOne(d => d.Denominaciones_Monedas)
                .WithMany()
                .HasForeignKey(d => d.denominacion_moneda_id);

            modelBuilder.Entity<Transacciones_Model>()
                .HasOne(t => t.Cajero)
                .WithMany()
                .HasForeignKey(t => t.cajero_id);

            modelBuilder.Entity<Transacciones_Model>()
                .HasOne(t => t.Denominaciones_Monedas)
                .WithMany()
                .HasForeignKey(t => t.denominacion_moneda_id);

            modelBuilder.Entity<Codigos_Evento_Model>()
                .HasOne(c => c.Tipo_Evento)
                .WithMany()
                .HasForeignKey(c => c.tipo_evento_id);


        }

        public AppDbContext() { } // Constructor vacío
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) // Llama al constructor base
        {
        }

        // Este método es opcional si usas Dependency Injection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "Server=LUISGUTIERREZ-P\\SQLEXPRESS2;Database=atm-driver;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

       

        // Método estático para crear una instancia del contexto
        public static AppDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            /*string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");*/
            string connectionString = "Server=LUISGUTIERREZ-P\\SQLEXPRESS2;Database=atm-driver;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";
            /*Console.Write(connectionString);*/
            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
            else
            {
                throw new InvalidOperationException($"La variable de entorno CONNECTION_STRING no está configurada. Valor actual: {connectionString}");
            }
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}