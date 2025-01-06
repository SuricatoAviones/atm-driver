using atm_driver.Models;
using Microsoft.EntityFrameworkCore;

namespace atm_driver
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Sistemas_Comunicacion_Model> SistemasComunicacion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=atm-driver;User ID=sa;Password=151199");
            }
        }
    }
}
