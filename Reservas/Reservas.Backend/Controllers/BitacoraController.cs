using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.Backend.Models;
using Reservas.Shared.Data;

namespace Reservas.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraController : ControllerBase
    {
        private readonly DataContext _context;

        public BitacoraController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bitacora>>> VerBitacora()
        {
            if (_context.Bitacora == null)
            {
                return NotFound();
            }

            var bitacora = await _context.Bitacora.ToListAsync();

            return Ok(bitacora);
        }

        [HttpPost]
        public async Task<ActionResult<Bitacora>> AgregarRegistro(Bitacora bitacora)
        {
            await _context.Bitacora.AddAsync(bitacora);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}