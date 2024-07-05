using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.Backend.Models;
using Reservas.Shared.Data;

namespace Reservas.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdificiosController : ControllerBase
    {
        private readonly DataContext _context;

        public EdificiosController(DataContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Edificios.ToListAsync());
        }

        [HttpPost]

        public async Task<IActionResult> PostAsync(Edificio edificio)
        {
            _context.Add(edificio);
            await _context.SaveChangesAsync();
            return Ok();
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var edificio = await _context.Edificios
                .SingleOrDefaultAsync(p => p.Id == id);
            if (edificio == null)
            {
                return NotFound();
            }
            return Ok(edificio);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Edificio edificio)
        {
            if (id != edificio.Id)
            {
                return BadRequest();
            }

            _context.Update(edificio);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var edificio = await _context.Edificios.FindAsync(id);
            if (edificio == null)
            {
                return NotFound();
            }
            _context.Remove(edificio);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
