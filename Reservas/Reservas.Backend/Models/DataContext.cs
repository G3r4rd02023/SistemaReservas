using Microsoft.EntityFrameworkCore;
using Reservas.Shared.Data;

namespace Reservas.Backend.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Edificio> Edificios { get; set; }

        public DbSet<Oficina> Oficinas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<DatoPersonal> DatosPersonales { get; set; }

        public DbSet<Rol> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().HasIndex(c => c.Email).IsUnique();
        }
    }
}
