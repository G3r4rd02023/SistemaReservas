using Microsoft.AspNetCore.Mvc.Rendering;
using Reservas.Shared.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservas.Frontend.Models
{
    public class OficinaViewModel: Oficina
    {
        [NotMapped]
        public IEnumerable<SelectListItem>? Edificios { get; set; }
    }
}
