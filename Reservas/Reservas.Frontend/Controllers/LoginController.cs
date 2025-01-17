﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservas.Frontend.Services;
using Reservas.Shared.Models;
using System.Security.Claims;
using System.Text;

namespace Reservas.Frontend.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IServicioUsuario _usuario;

        public LoginController(IHttpClientFactory httpClientFactory, IServicioUsuario usuario)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
            _usuario = usuario;
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
                var response = await _httpClient.PostAsync("/api/Login/IniciarSesion", content);
                var user = await _usuario.GetUsuarioByEmail(model.Email);
                var rol = await _usuario.GetRolById(user.RolId);

                if (response.IsSuccessStatusCode)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.Role, rol)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    var apiService = new ApiService();
                    await apiService.Autenticar(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["AlertMessage"] = "Usuario o clave incorrectos!!! numero de intentos: ";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("IniciarSesion", "Login");
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