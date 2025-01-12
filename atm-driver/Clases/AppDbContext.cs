using atm_driver.Models;
using Microsoft.EntityFrameworkCore;

namespace atm_driver.Clases
{
    public class AppDbContext : DbContext
    {
        // DbSet para los modelos
        public DbSet<Sistemas_Comunicacion_Model> SistemasComunicacion { get; set; }
        public DbSet<Cajeros_Model> Cajeros { get; set; }

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
                optionsBuilder.UseSqlServer("Server=LAPTOP-BQF70VD3\\SQLEXPRESS;Database=atm-driver;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True\n");
            }
        }

        // Método estático para crear una instancia del contexto
        public static AppDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=LAPTOP-BQF70VD3\\SQLEXPRESS;Database=atm-driver;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True\n");
            return new AppDbContext(optionsBuilder.Options);
        } 
    }
}
