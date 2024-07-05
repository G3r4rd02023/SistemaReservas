using System.ComponentModel.DataAnnotations;

namespace Reservas.Shared.Data
{
    public class Edificio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Ubicacion { get; set; } = null!;

    }
}
