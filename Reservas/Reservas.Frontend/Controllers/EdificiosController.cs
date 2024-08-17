using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservas.Frontend.Models;
using Reservas.Shared.Data;
using System.Text;

namespace Reservas.Frontend.Controllers
{
    [Authorize]
    public class EdificiosController : Controller
    {
        private readonly HttpClient _httpClient;

        public EdificiosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");

        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Edificios");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var edificios = JsonConvert.DeserializeObject<IEnumerable<Edificio>>(content);
                return View("Index", edificios);
            }
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
                var json = JsonConvert.SerializeObject(edificio);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/Edificios", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Edificio creado Exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
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
                var json = JsonConvert.SerializeObject(edificio);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Edificios/{id}", content);
                if (response.IsSuccessStatusCode)
                {
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
