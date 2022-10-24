using Microsoft.AspNetCore.Mvc;

namespace PruebaDBP.Controllers
{
    public class UsuarioController : Controller
    {
      
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Perfil()
        {
            return View();
        }

        public IActionResult EditarPerfil()
        {
            return View();
        }
    }
}
