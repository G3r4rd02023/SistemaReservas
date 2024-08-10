using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.Backend.Models;
using Reservas.Shared.Data;
using Reservas.Shared.Models;

namespace Reservas.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("IniciarSesion")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _context.Usuarios
                .SingleOrDefaultAsync(u => u.Email == login.Email);

            if (usuario != null)
            {
                if (BCrypt.Net.BCrypt.Verify(login.Contraseña, usuario.Contrasena))
                {
                    return Ok(new { Message = "Inicio de sesión exitoso." });
                }
            }

            return Unauthorized(new { Message = "Inicio de sesión fallido. Usuario o contraseña incorrectos." });
        }

        [HttpPost("Registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.Contrasena = BCrypt.Net.BCrypt.HashPassword(model.Contrasena);

            var usuario = new Usuario()
            {
                PrimerNombre = model.PrimerNombre,
                SegundoNombre = model.SegundoNombre,
                PrimerApellido = model.PrimerApellido,
                SegundoApellido = model.SegundoApellido,
                Email = model.Email,
                Contrasena = model.Contrasena,
                RolId = model.RolId
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var datos = new DatoPersonal()
            {
                Direccion = model.Direccion,
                EmailPersonal = model.EmailPersonal,
                FechaRegistro = DateTime.Now,
                Telefono = model.Telefono,
                UsuarioId = usuario.Id
            };

            _context.DatosPersonales.Add(datos);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Usuario registrado exitosamente." });
        }
    }
}
