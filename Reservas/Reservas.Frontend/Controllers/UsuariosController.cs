using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservas.Shared.Data;

namespace Reservas.Frontend.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsuariosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Usuarios");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var usuarios = JsonConvert.DeserializeObject<IEnumerable<DatoPersonal>>(content);
                return View("Index", usuarios);
            }
            return View(new List<DatoPersonal>());
        }
    }
}
