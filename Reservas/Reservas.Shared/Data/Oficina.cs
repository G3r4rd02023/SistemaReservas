using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace Reservas.Shared.Data
{
    public class Oficina
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Nombre { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        [MaxLength(300, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Precio { get; set; }
        public int EdificioId { get; set; }
        public Edificio? Edificio { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Ubicacion { get; set; } = null!;

        

    }
}
