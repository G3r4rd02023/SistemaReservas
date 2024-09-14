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

        public DbSet<Reserva> Reservas { get; set; }

        public DbSet<Servicio> Servicios { get; set; }

        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().HasIndex(c => c.Email).IsUnique();
            modelBuilder.Entity<Servicio>().HasIndex(r => r.Nombre).IsUnique();
        }
    }
}