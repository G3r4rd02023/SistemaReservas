using Microsoft.AspNetCore.Mvc.Rendering;

namespace Reservas.Frontend.Models
{
    public class EdificioViewModel
    {
        public int EdificioId { get; set; }
        public IEnumerable<SelectListItem>? Edificios { get; set; }
    }
}
