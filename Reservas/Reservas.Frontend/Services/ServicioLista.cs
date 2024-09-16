using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Reservas.Shared.Data;
using System.Data;

namespace Reservas.Frontend.Services
{
    public class ServicioLista : IServicioLista
    {
        private readonly HttpClient _httpClient;

        public ServicioLista(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
        }

        public async Task<IEnumerable<SelectListItem>> GetListaEdificios()
        {
            var response = await _httpClient.GetAsync("api/Edificios");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var edificios = JsonConvert.DeserializeObject<IEnumerable<Edificio>>(content);

                var listaEdificios = edificios!.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Nombre
                }).ToList();

                listaEdificios.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Seleccione un Edificio"
                });
                return listaEdificios;
            }
            return [];
        }

        public async Task<IEnumerable<SelectListItem>> GetListaRoles()
        {
            var response = await _httpClient.GetAsync("/api/Roles");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<IEnumerable<Rol>>(content);
                var listaRoles = roles!.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Descripcion
                }).ToList();

                listaRoles.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Seleccione un Rol"
                });
                return listaRoles;
            }

            return [];
        }
    }
}