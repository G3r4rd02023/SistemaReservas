using System.ComponentModel.DataAnnotations;

namespace Reservas.Shared.Models
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Contraseña { get; set; } = null!;
    }
}
