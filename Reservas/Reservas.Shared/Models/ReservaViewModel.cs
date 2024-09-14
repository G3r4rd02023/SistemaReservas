using Reservas.Shared.Data;

namespace Reservas.Shared.Models
{
    public class ReservaViewModel : Reserva
    {
        public int ServicioId { get; set; }
        public Servicio? Servicio { get; set; }
    }
}