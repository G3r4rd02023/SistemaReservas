namespace Reservas.Shared.Data
{
    public class Transaccion
    {
        public int Id { get; set; }

        public int ServicioId { get; set; }
        public Servicio? Servicio { get; set; }

        public int ReservaId { get; set; }
        public Reserva? Reserva { get; set; }
    }
}