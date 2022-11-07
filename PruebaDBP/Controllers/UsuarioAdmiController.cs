using Microsoft.AspNetCore.Mvc;
using PruebaDBP.Models;

namespace PruebaDBP.Controllers
{
    public class UsuarioAdmiController : Controller
    {
        private readonly bdlumiereContext Context;
        public UsuarioAdmiController(bdlumiereContext context)
        {
            Context = context;
        }


        public IActionResult LoginAdmi()
        {
            return View();
        }
        public IActionResult VentanaAdmi()
        {
            return View();
        }

        //Manejo de Info relacionada a las peliculas
        public IActionResult CrearPelicula()
        {
            return View();
        }

        public IActionResult CrearPeli(Pelicula obj)
        {
            if (ModelState.IsValid)
            {
                Context.Peliculas.Add(obj);
                Context.SaveChanges();
                return RedirectToAction("ListadoPeliculas");
            }
            else
            {
                ViewBag.Civil = Context.Peliculas;
                return View("CrearPelicula");
            }
        }


        [Route("UsuarioAdmi/EditarPelicula/{Codigo}")]
        public IActionResult EditarPelicula(int codigo)
        {
            var obj = (from Tpeli in Context.Peliculas where Tpeli.IdPelicula == codigo select Tpeli).Single();
            return View(obj);
        }

        public IActionResult EditarPeli(Pelicula objNew)
        {
            if (ModelState.IsValid)
            {
                var objOld = (from Tpeli in Context.Peliculas where Tpeli.IdPelicula == objNew.IdPelicula select Tpeli).Single();

                objOld.IdPelicula = objNew.IdPelicula;
                objOld.IdTmdb = objNew.IdTmdb;
                objOld.IdIdioma = objNew.IdIdioma;
                objOld.NomPelicula = objNew.NomPelicula;
                objOld.FechaEstreno = objNew.FechaEstreno;
                objOld.DuracionMin = objNew.DuracionMin;
                objOld.Sumilla = objNew.Sumilla;
                objOld.UrlFoto = objNew.UrlFoto;



                Context.SaveChanges();
                return RedirectToAction("ListadoPeliculas");
            }
            else
            {
                return View("EditrPelicula");
            }
        }

        [Route("UsuarioAdmi/EliminarPelicula/{Codigo}")]
        public IActionResult EliminarPelicula(int codigo)
        {
            var obj = (from Tpeli in Context.Peliculas where Tpeli.IdPelicula == codigo select Tpeli).Single();

            Context.Peliculas.Remove(obj);
            Context.SaveChanges();

            return RedirectToAction("ListadoPeliculas");
        }

        public IActionResult ListadoPeliculas()
        {
            var list = Context.Peliculas;
            return View(list);
        }

        //Manejo de Info relacionada a los Directores
        public IActionResult ListarDirectores()
        {
            var list = Context.Directors;
            return View(list);
        }

        [Route("UsuarioAdmi/EliminarDirector/{Codigo}")]
        public IActionResult EliminarDirector(int codigo)
        {
            var obj = (from Tdirector in Context.Directors where Tdirector.IdDirector == codigo select Tdirector).Single();

            Context.Directors.Remove(obj);
            Context.SaveChanges();

            return RedirectToAction("ListarDirectores");
        }

        public IActionResult CrearDirector()
        {
            return View();
        }

        public IActionResult CrearDire(Director obj)
        {
            if (ModelState.IsValid)
            {
                Context.Directors.Add(obj);
                Context.SaveChanges();
                return RedirectToAction("ListarDirectores");
            }
            else
            {
                ViewBag.Civil = Context.Directors;
                return View("CrearDirector");
            }
        }

        [Route("UsuarioAdmi/EditarDirector/{Codigo}")]
        public IActionResult EditarDirector(int codigo)
        {
            var obj = (from Tdirector in Context.Directors where Tdirector.IdDirector == codigo select Tdirector).Single();
            return View(obj);
        }

        public IActionResult EdiatarDire(Director objNew)
        {
            if (ModelState.IsValid)
            {
                var objOld = (from Tdirector in Context.Directors where Tdirector.IdDirector == objNew.IdDirector select Tdirector).Single();

                objOld.IdDirector = objNew.IdDirector;
                objOld.IdDirTmdb = objNew.IdDirTmdb;
                objOld.NomDirector = objNew.NomDirector;
                objOld.BioDirector = objNew.BioDirector;
                objOld.UrlFoto = objNew.UrlFoto;



                Context.SaveChanges();
                return RedirectToAction("ListarDirectores");
            }
            else
            {
                return View("EditarDirector");
            }
        }
    }
}
