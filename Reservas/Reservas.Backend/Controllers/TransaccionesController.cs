using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.Backend.Models;
using Reservas.Shared.Data;
using Reservas.Shared.Models;

namespace Reservas.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        private readonly DataContext _context;

        public TransaccionesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Transacciones
                .Include(t => t.Reserva)
                .Include(t => t.Servicio)
                .ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(ReservaViewModel model)
        {
            var reserva = new Reserva()
            {
                Usuario = model.Usuario,
                Oficina = model.Oficina,
                UsuarioId = model.UsuarioId,
                OficinaId = model.OficinaId,
                Inicio = model.Inicio,
                Final = model.Final,
                Estado = model.Estado,
                Total = model.Total
            };
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            var transaccion = new Transaccion()
            {
                ReservaId = reserva.Id,
                ServicioId = model.ServicioId,
                Reserva = reserva,
                Servicio = model.Servicio
            };
            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}