using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Reservas.Shared.Data;

namespace Reservas.Backend.Models
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await ValidarRolesAsync("Administrador");
            await ValidarRolesAsync("Usuario");
            var rol = _context.Roles.FirstOrDefault();
            await ValidarUsuariosAsync("Super", "Usuario", "Admin", "Sistema", "superadmin@gmail.com", "123456", rol!);
        }

        private async Task<Rol> ValidarRolesAsync(string nombreRol)
        {
            var rolExistente = await _context.Roles.FirstOrDefaultAsync(r => r.Descripcion == nombreRol);
            if (rolExistente != null)
            {
                return rolExistente;
            }

            Rol rol = new()
            {
                Descripcion = nombreRol,
            };

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        private async Task<Usuario> ValidarUsuariosAsync(string primerNombre, string segundoNombre, string primerApellido, string segundoApellido, string email, string contrasena, Rol rolUsuario)
        {
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuarioExistente != null)
            {
                return usuarioExistente;
            }

            Usuario usuario = new()
            {
                PrimerNombre = primerNombre,
                SegundoNombre = segundoNombre,
                Email = email,
                Contrasena = contrasena,
                PrimerApellido = primerApellido,
                Rol = rolUsuario,
                SegundoApellido = segundoApellido,
            };

            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }


    }
}
