using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Reservas.Shared.Data;

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

        public async Task<IEnumerable<SelectListItem>>GetListaEdificios()
        {
            var response = await _httpClient.GetAsync("api/Edificios");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var edificios = JsonConvert.DeserializeObject<IEnumerable<Edificio>>(content);

                return edificios!.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                }).ToList();
            }
            return new List<SelectListItem>();
        }
    }
}
