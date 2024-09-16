using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservas.Frontend.Services;
using Reservas.Shared.Data;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Reservas.Frontend.Controllers
{
    public class EdificiosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IServicioUsuario _usuario;
        private readonly IServicioBitacora _bitacora;

        public EdificiosController(IHttpClientFactory httpClientFactory, IServicioUsuario usuario, IServicioBitacora bitacora)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
            _usuario = usuario;
            _bitacora = bitacora;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _usuario.GetUsuarioByEmail(User.Identity!.Name!);
            var apiService = new ApiService();
            var token = await apiService.Autenticar(user);
            // Realiza la solicitud HTTP al endpoint protegido
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/Edificios");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var edificios = JsonConvert.DeserializeObject<IEnumerable<Edificio>>(content);
                var bitacora = new Bitacora()
                {
                    UsuarioId = user!.Id,
                    TipoAccion = "SELECT",
                    Tabla = "Edificios",
                    Fecha = DateTime.Now
                };
                await _bitacora.AgregarRegistro(bitacora);
                return View("Index", edificios);
            }

            // Si la solicitud falla (por ejemplo, 401 Unauthorized), maneja el error
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Podrías redirigir al usuario a la página de login o mostrar un mensaje de error
                return RedirectToAction("IniciarSesion", "Login");
            }

            // En caso de otros errores, podrías manejar la excepción o retornar una vista vacía
            return View(new List<Edificio>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Edificio edificio)
        {
            if (ModelState.IsValid)
            {
                var user = await _usuario.GetUsuarioByEmail(User.Identity!.Name!);
                var apiService = new ApiService();
                var token = await apiService.Autenticar(user);
                // Realiza la solicitud HTTP al endpoint protegido
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(edificio);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/Edificios", content);
                if (response.IsSuccessStatusCode)
                {
                    var bitacora = new Bitacora()
                    {
                        UsuarioId = user!.Id,
                        TipoAccion = "INSERT",
                        Tabla = "Edificios",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Edificio creado Exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    if (response.StatusCode.ToString() == "401")
                    {
                        TempData["ErrorMessage"] = "No estás autorizado para realizar esta acción. Por favor, inicia sesión.";
                    }

                    TempData["ErrorMessage"] = "Ocurrio un error al crear el edificio";
                }
            }

            return View(edificio);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Edificios/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener edificio";
                return RedirectToAction("Index");
            }
            var jsonString = await response.Content.ReadAsStringAsync();
            var edificio = JsonConvert.DeserializeObject<Edificio>(jsonString);

            return View(edificio);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Edificio edificio)
        {
            if (ModelState.IsValid)
            {
                var user = await _usuario.GetUsuarioByEmail(User.Identity!.Name!);
                var json = JsonConvert.SerializeObject(edificio);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Edificios/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    var bitacora = new Bitacora()
                    {
                        UsuarioId = user!.Id,
                        TipoAccion = "EDIT",
                        Tabla = "Edificios",
                        Fecha = DateTime.Now
                    };
                    await _bitacora.AgregarRegistro(bitacora);
                    TempData["AlertMessage"] = "Edificio actualizada Exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrio un error al actualizar el edificio";
                    return RedirectToAction("Index");
                }
            }

            return View(edificio);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Edificios/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Edificio eliminado Exitosamente";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el edificio";
                return RedirectToAction("Index");
            }
        }
    }
}