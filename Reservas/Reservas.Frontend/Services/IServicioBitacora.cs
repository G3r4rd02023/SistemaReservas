using Reservas.Shared.Data;

namespace Reservas.Frontend.Services
{
    public interface IServicioBitacora
    {
        Task<Bitacora> AgregarRegistro(Bitacora bitacora);
    }
}