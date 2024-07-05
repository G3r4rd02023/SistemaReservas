using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.Backend.Models;
using Reservas.Shared.Data;

namespace Reservas.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OficinasController : ControllerBase
    {
        private readonly DataContext _context;

        public OficinasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Oficinas.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Oficina oficina)
        {
            var edificio = _context.Edificios.FirstOrDefault(e => e.Id == oficina.EdificioId);
            if (edificio != null)
            {
                oficina.Edificio = edificio;
                _context.Add(oficina);
                await _context.SaveChangesAsync();
                return Ok();
            }

            throw new Exception("El edificio con ID especificado no existe.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var oficina = await _context.Oficinas
                .SingleOrDefaultAsync(p => p.Id == id);
            if (oficina == null)
            {
                return NotFound();
            }
            return Ok(oficina);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, Oficina oficina)
        {
            var oficinaExistente = _context.Oficinas.FirstOrDefault(o => o.Id == id);

            if (oficinaExistente == null)
            {
                return NotFound();
            }

            oficinaExistente.Nombre = oficina.Nombre;
            oficinaExistente.Descripcion = oficina.Descripcion;
            oficinaExistente.Precio = oficina.Precio;
            oficinaExistente.EdificioId = oficina.EdificioId;
            oficinaExistente.Ubicacion = oficina.Ubicacion;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var oficina = await _context.Oficinas.FindAsync(id);
            if (oficina == null)
            {
                return NotFound();
            }
            _context.Remove(oficina);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
