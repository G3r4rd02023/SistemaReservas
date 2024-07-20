using Microsoft.EntityFrameworkCore;
using Reservas.Shared.Data;

namespace Reservas.Backend.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<DatosPersonales> DatosPersonales { get; set; }
        public DbSet<Edificio> Edificios { get; set; }
        public DbSet<Oficina> Oficinas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get;set; }
    }
}
