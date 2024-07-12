using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservas.Shared.Data;
using Reservas.Frontend.Models;
using Reservas.Frontend.Services;

namespace Reservas.Frontend.Controllers
{
    public class OficinasController : Controller
    {
        
        private readonly HttpClient _httpClient;
        private readonly IServicioLista? _lista;

        public OficinasController(IHttpClientFactory httpClientFactory, IServicioLista lista)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
            lista = _lista!;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Oficinas");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var oficinas = JsonConvert.DeserializeObject<IEnumerable<Oficina>>(content);
                return View("Index", oficinas);
            }
            return View(new List<Oficina>());
        }

        public async Task<IActionResult> Create()
        {
            var model = new EdificioViewModel();
            model.Edificios = await _lista!.GetListaEdificios();

            return View(model);
        }
    }
}
