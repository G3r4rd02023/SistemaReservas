using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservas.Shared.Data
{
    public class Reserva
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int OficinaId { get; set; }
        public Oficina? Oficina { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Final { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Estado { get; set; } = null!;

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }
    }
}