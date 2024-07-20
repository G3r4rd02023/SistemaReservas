using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservas.Frontend.Services;
using Reservas.Shared.Data;
using Reservas.Shared.Models;
using System.Security.Claims;
using System.Text;

namespace Reservas.Frontend.Controllers
{
    public class LoginController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IServicioUsuario _servicioUsuario;

        public LoginController(IHttpClientFactory httpClientFactory, IServicioUsuario servicioUsuario)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
            _servicioUsuario = servicioUsuario;
        }
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                //var email = Uri.EscapeDataString(model.Email);
                //var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");
                //var usuarioJson = await userResponse.Content.ReadAsStringAsync();
                //var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);


                var user = await _servicioUsuario.ObtenerUsuarioPorEmailAsync(model.Email);
                var response = await _httpClient.PostAsync("/api/Login/IniciarSesion", content);
                if (response.IsSuccessStatusCode)
                {
                    //var result = await _httpClient.GetAsync($"/api/Roles/{user!.RolId}");
                    //var rolJson = await result.Content.ReadAsStringAsync();
                    //var rol = JsonConvert.DeserializeObject<Rol>(rolJson);
                    //var descripcion = rol!.Descripcion;

                    var descripcion = await _servicioUsuario.ObtenerNombreRolAsync(user);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.Role, descripcion),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["AlertMessage"] = "Usuario o clave incorrectos!!!";
                }
            }
            return View(model);
        }


        public IActionResult Registro()
        {

            var model = new RegistroViewModel()
            {
                RolId = 2,               
            };
                     
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.RolId = 2;                
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
