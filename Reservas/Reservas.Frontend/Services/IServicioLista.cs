using Microsoft.AspNetCore.Mvc.Rendering;

namespace Reservas.Frontend.Services
{
    public interface IServicioLista
    {
        Task<IEnumerable<SelectListItem>> GetListaEdificios();

        Task<IEnumerable<SelectListItem>> GetListaRoles();
    }
}