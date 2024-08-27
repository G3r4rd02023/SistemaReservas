using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.Backend.Models;
using Reservas.Shared.Data;

namespace Reservas.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DataContext _context;

        public RolesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Roles.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Rol rol)
        {
            _context.Add(rol);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var rol = await _context.Roles
                .SingleOrDefaultAsync(c => c.Id == id);
            if (rol == null)
            {
                return NotFound();
            }
            return Ok(rol);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Rol rol)
        {
            if (id != rol.Id)
            {
                return BadRequest();
            }

            _context.Update(rol);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            _context.Remove(rol);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}