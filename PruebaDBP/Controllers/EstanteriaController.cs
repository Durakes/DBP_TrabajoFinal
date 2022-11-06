using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PruebaDBP.Controllers
{
    public class EstanteriaController : Controller
    {
        private readonly bdlumiereContext Context;
        public EstanteriaController(bdlumiereContext context)
        {
            Context = context;
        }
        public IActionResult Libreria()
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));
            var listEstanterias = (from est in Context.Estanteria
                                    where est.IdUsuario == usuario.IdUsuario
                                    select est).ToList();
            return View(listEstanterias);
        }
        public IActionResult AgregarLibreria(string nomLib)
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));
            Estanterium nuevLib = new Estanterium();
            nuevLib.IdUsuario = usuario.IdUsuario;
            nuevLib.NomEstanteria = nomLib;
            nuevLib.EsEditable = true;
            nuevLib.FechaCreacion = DateTime.Now.ToString("yyyy-MM-dd");

            Context.Estanteria.Add(nuevLib);
            Context.SaveChanges();
            return RedirectToAction("Libreria");
        }
    }
}
