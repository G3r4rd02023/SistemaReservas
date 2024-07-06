using Microsoft.AspNetCore.Mvc.Rendering;

namespace Reservas.Frontend.Models
{
    public class EdificiosViewModel
    {
        public int EdificioId { get; set; }
        public IEnumerable<SelectListItem>? Edificios { get; set; }
    }
}
