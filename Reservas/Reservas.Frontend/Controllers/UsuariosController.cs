using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservas.Shared.Data;
using Reservas.Shared.Models;
using System.Text;

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

        public IActionResult Create()
        {
            var model = new RegistroViewModel()
            {
                RolId = 1,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.RolId = 1;
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Login/Registro", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Usuario registrado exitosamente!!!";
                    return RedirectToAction("IniciarSesion", "Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error, no se pudo crear el usuario");
                }
            }
            return View(model);
        }
    }
}