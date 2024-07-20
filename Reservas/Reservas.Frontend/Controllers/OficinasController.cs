using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservas.Shared.Data;
using Reservas.Frontend.Models;
using Reservas.Frontend.Services;
using System.Text;

namespace Reservas.Frontend.Controllers
{
    public class OficinasController : Controller
    {
        
        private readonly HttpClient _httpClient;
        private readonly IServicioLista _lista;

        public OficinasController(IHttpClientFactory httpClientFactory, IServicioLista lista)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
            _lista = lista;
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
            var edificios = await _lista!.GetListaEdificios();
            if(edificios == null)
            {
                ModelState.AddModelError(string.Empty, "Edificio no encontrado");
            }

            OficinaViewModel oficina = new()
            {
                Edificios = edificios,
            };

            return View(oficina);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OficinaViewModel oficina)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(oficina);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/Oficinas", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Oficina creada Exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrio un error al crear la oficina";
                }
            }
            oficina.Edificios = await _lista.GetListaEdificios();
            return View(oficina);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Oficinas/{id}");
            if (!response.IsSuccessStatusCode) 
            {
                TempData["ErrorMessage"] = "Error al obtener oficina";
                return RedirectToAction("Index");
            }
            var jsonString = await response.Content.ReadAsStringAsync();
            var oficina = JsonConvert.DeserializeObject<OficinaViewModel>(jsonString);
            oficina!.Edificios = await _lista.GetListaEdificios();
            return View(oficina);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, OficinaViewModel oficina)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(oficina);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Oficinas/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Oficina actualizada Exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrio un error al actualizar la oficina";
                    return RedirectToAction("Index");
                }
            }
            oficina.Edificios = await _lista.GetListaEdificios();
            return View(oficina);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Oficinas/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Oficina eliminada Exitosamente";
                return RedirectToAction("Index");
            }
            else 
            {
                TempData["ErrorMessage"] = "Error al eliminar la oficina";
                return RedirectToAction("Index");
            }           
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Oficinas/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener oficina";
                return RedirectToAction("Index");
            }
            var jsonString = await response.Content.ReadAsStringAsync();
            var oficina = JsonConvert.DeserializeObject<OficinaViewModel>(jsonString);
           
            return View(oficina);
        }

    }
}
