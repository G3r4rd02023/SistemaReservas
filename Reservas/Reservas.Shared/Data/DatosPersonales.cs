using System.ComponentModel.DataAnnotations;

namespace Reservas.Shared.Data
{
    public class DatosPersonales
    {
        public int Id { get; set; }

        [MaxLength(255, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string EmailPersonal { get; set; } = null!;

        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Telefono{ get; set; } = null!;

        [MaxLength(255, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Direccion { get; set; } = null!;

        public DateTime FechaRegistro { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
    }
}
