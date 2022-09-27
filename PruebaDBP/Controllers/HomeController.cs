using Microsoft.AspNetCore.Mvc;
using PruebaDBP.Models;
using System.Diagnostics;

namespace PruebaDBP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult EditProfile()
        {
            return View();
        }

        public IActionResult LandingPage()
        {
            return View();
        }

        public IActionResult LandingPageUser()
        {
            return View();
        }

        public IActionResult Library()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Movie()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Result()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}