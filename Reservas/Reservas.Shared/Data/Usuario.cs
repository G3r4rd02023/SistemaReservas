using System.ComponentModel.DataAnnotations;

namespace Reservas.Shared.Data
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string PrimerNombre { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string SegundoNombre { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string PrimerApellido { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string SegundoApellido { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Contrasena { get; set; } = null!;
        public string NombreCompleto => $"{PrimerNombre} {PrimerApellido}";
        public int RolId { get; set; }
        public Rol? Rol { get; set; }

    }
}
