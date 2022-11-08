using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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


        //Manejo de los Procesos del Administrador
        public IActionResult LoginAdmi()
        {
            return View();
        }
        public IActionResult Validar(Usuario objNew)
        {
            if (ModelState.IsValid)
            {
                var Obj = (from Tusuario in Context.Usuarios
                           where Tusuario.IdUsuario == objNew.IdUsuario &&
                           Tusuario.Contraseña == objNew.Contraseña
                           select Tusuario).FirstOrDefault();
                if (Obj == null)
                {
                    return RedirectToAction("LoginAdmi");
                }
                else
                {
                    //Crear sesion de usuario, creo una variable de sesion string
                    HttpContext.Session.SetString("sadministrador", JsonConvert.SerializeObject(Obj));
                    return RedirectToAction("VentanaAdmi");
                }

            }
            else
            {
                return View("LoginAdmi");
            }
        }


        public IActionResult VentanaAdmi()
        {
                return View();
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
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


        //Manejo de la info relacionada a Usuarios
        public IActionResult ListarUsuarios()
        {
            var list = Context.Usuarios;
            return View(list);
        }

        [Route("UsuarioAdmi/EliminarUsuario/{Codigo}")]
        public IActionResult EliminarUsuario(int codigo)
        {
            var obj = (from Tusuarios in Context.Usuarios where Tusuarios.IdUsuario == codigo select Tusuarios).Single();

            Context.Usuarios.Remove(obj);
            Context.SaveChanges();

            return RedirectToAction("ListarUsuarios");
        }

        public IActionResult CrearUsuario()
        {
            return View();
        }

        public IActionResult CrearUsu(Usuario obj)
        {
            if (ModelState.IsValid)
            {
                Context.Usuarios.Add(obj);
                Context.SaveChanges();
                return RedirectToAction("ListarUsuarios");
            }
            else
            {
                ViewBag.Usuario = Context.Usuarios;
                return View("CrearUsuario");
            }
        }
    }
}
