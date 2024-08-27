using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Reservas.Backend.Models;
using Reservas.Shared.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Reservas.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly string secretKey;
        private readonly DataContext _context;

        public AutenticacionController(IConfiguration config, DataContext context)
        {
            secretKey = config.GetSection("Jwt").GetValue<string>("key")!;
            _context = context;
        }

        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar([FromBody] Usuario request)
        {
            var usuario = _context.Usuarios.SingleOrDefault(u => u.Email == request.Email);

            if (usuario != null && usuario.Contrasena == request.Contrasena)
            {
                var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Email));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokencreado = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = tokencreado });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }
        }
    }
}