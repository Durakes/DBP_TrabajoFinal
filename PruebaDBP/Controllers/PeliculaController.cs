using Microsoft.AspNetCore.Mvc;

namespace PruebaDBP.Controllers
{
    public class PeliculaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Resultados()
        {
            return View();
        }
    }
}
