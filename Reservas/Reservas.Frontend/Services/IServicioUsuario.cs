using Reservas.Shared.Data;

namespace Reservas.Frontend.Services
{
    public interface IServicioUsuario
    {
        Task<Usuario> GetUsuarioByEmail(string email);

        Task<string> GetRolById(int rolId);
    }
}