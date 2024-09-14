using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Reservas.Frontend.Services;
using Reservas.Shared.Data;
using System.Text;

namespace Reservas.Frontend.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IServicioUsuario _usuario;

        public ServiciosController(IHttpClientFactory httpClientFactory, IServicioUsuario usuario)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
            _usuario = usuario;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Servicios");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var servicios = JsonConvert.DeserializeObject<IEnumerable<Servicio>>(content);
                return View("Index", servicios);
            }

            return View(new List<Servicio>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(servicio);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("/api/Servicios", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["AlertMessage"] = "Servicio creado Exitosamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Ocurrio un error al crear el servicio";
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException!.InnerException!.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un servicio con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(servicio);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Servicios/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener servicio";
                return RedirectToAction("Index");
            }
            var jsonString = await response.Content.ReadAsStringAsync();
            var servicio = JsonConvert.DeserializeObject<Servicio>(jsonString);

            return View(servicio);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(servicio);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Servicios/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Servicio actualizado Exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrio un error al actualizar el servicio";
                    return RedirectToAction("Index");
                }
            }

            return View(servicio);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Servicios/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Servicio eliminado Exitosamente";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el servicio";
                return RedirectToAction("Index");
            }
        }
    }
}