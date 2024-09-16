using Microsoft.AspNetCore.Mvc.Rendering;
using Reservas.Shared.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservas.Frontend.Models
{
    public class UsuarioViewModel : Usuario
    {
        [NotMapped]
        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}