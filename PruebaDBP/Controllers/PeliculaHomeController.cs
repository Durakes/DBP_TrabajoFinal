using Microsoft.AspNetCore.Mvc;

namespace PruebaDBP.Controllers
{
    public class PeliculaHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
