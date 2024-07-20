using Reservas.Shared.Data;

namespace Reservas.Frontend.Services
{
    public interface IServicioUsuario
    {
        Task<Usuario> ObtenerUsuarioPorEmailAsync(string email);

        Task<string> ObtenerNombreRolAsync(Usuario usuario);
    }
}
