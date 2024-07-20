using Microsoft.AspNetCore.Mvc;

namespace Reservas.Backend.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

    }
}
