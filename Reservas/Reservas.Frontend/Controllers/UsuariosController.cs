using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservas.Frontend.Models;
using Reservas.Frontend.Services;
using Reservas.Shared.Data;
using Reservas.Shared.Models;
using System.Text;

namespace Reservas.Frontend.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IServicioLista _lista;

        public UsuariosController(IHttpClientFactory httpClientFactory, IServicioLista lista)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
            _lista = lista;
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

        public async Task<IActionResult> ModificarRol(int id)
        {
            var response = await _httpClient.GetAsync($"api/Usuarios/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UsuarioViewModel>(json);
            if (user == null)
            {
                return NotFound();
            }

            user.Roles = await _lista.GetListaRoles();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ModificarRol(UsuarioViewModel user)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/api/Usuarios/{user.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Usuario actualizado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar usuario!!";
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Usuarios/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Usuario eliminado exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el usuario.";
                return RedirectToAction("Index");
            }
        }
    }
}