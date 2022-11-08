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
            if (objNew.Username == "admin" && objNew.Contraseña== "admin" )
            {
                    //Crear sesion de usuario, creo una variable de sesion string
                    HttpContext.Session.SetString("sadministrador", JsonConvert.SerializeObject(objNew));
                    return RedirectToAction("VentanaAdmi");
            }
            else
            {
                return View("LoginAdmi");
            }
        }

        [HttpGet]
        public IActionResult VentanaAdmi()
        {
            var ObjSesion = HttpContext.Session.GetString("sadministrador");
            if (ObjSesion != null)
            {
                var Obj = JsonConvert.DeserializeObject
                    <Usuario>(HttpContext.Session.GetString("sadministrador"));

                return View();
            }
            else
            {
                return RedirectToAction("LoginAdmi");
            }
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        //Manejo de Info relacionada a las peliculas
        [HttpGet]
        public IActionResult CrearPelicula()
        {
            var ObjSesion = HttpContext.Session.GetString("sadministrador");
            if (ObjSesion != null)
            {
                var Obj = JsonConvert.DeserializeObject
                    <Usuario>(HttpContext.Session.GetString("sadministrador"));

                return View();
            }
            else
            {
                return RedirectToAction("LoginAdmi");
            }
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
                ViewBag.Pelicula = Context.Peliculas;
                return View("CrearPelicula");
            }
        }

        [HttpGet]
        [Route("UsuarioAdmi/EditarPelicula/{Codigo}")]
        public IActionResult EditarPelicula(int codigo)
        {
            var ObjSesion = HttpContext.Session.GetString("sadministrador");
            if (ObjSesion != null)
            {
                var Obj = JsonConvert.DeserializeObject
                    <Usuario>(HttpContext.Session.GetString("sadministrador"));

                var obj = (from Tpeli in Context.Peliculas where Tpeli.IdPelicula == codigo select Tpeli).Single();
                return View(obj);
            }
            else
            {
                return RedirectToAction("LoginAdmi");
            }
        }

        [HttpGet]
        public IActionResult EditarPeli(Pelicula objNew)
        {
            if (ModelState.IsValid)
            {
                var objOld = (from Tpeli in Context.Peliculas where Tpeli.IdPelicula == objNew.IdPelicula select Tpeli).Single();

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

        [HttpGet]
        public IActionResult ListadoPeliculas()
        {
            var ObjSesion = HttpContext.Session.GetString("sadministrador");
            if (ObjSesion != null)
            {
                var Obj = JsonConvert.DeserializeObject
                    <Usuario>(HttpContext.Session.GetString("sadministrador"));

                var list = Context.Peliculas;
                return View(list);
            }
            else
            {
                return RedirectToAction("LoginAdmi");
            }
        }

        //Manejo de Info relacionada a los Directores
        [HttpGet]
        public IActionResult ListarDirectores()
        {
            var ObjSesion = HttpContext.Session.GetString("sadministrador");
            if (ObjSesion != null)
            {
                var Obj = JsonConvert.DeserializeObject
                    <Usuario>(HttpContext.Session.GetString("sadministrador"));

                var list = Context.Directors;
                return View(list);
            }
            else
            {
                return RedirectToAction("LoginAdmi");
            }
        }

        [Route("UsuarioAdmi/EliminarDirector/{Codigo}")]
        public IActionResult EliminarDirector(int codigo)
        {
            var obj = (from Tdirector in Context.Directors where Tdirector.IdDirector == codigo select Tdirector).Single();

            Context.Directors.Remove(obj);
            Context.SaveChanges();

            return RedirectToAction("ListarDirectores");
        }

        [HttpGet]
        public IActionResult CrearDirector()
        {
            var ObjSesion = HttpContext.Session.GetString("sadministrador");
            if (ObjSesion != null)
            {
                var Obj = JsonConvert.DeserializeObject
                    <Usuario>(HttpContext.Session.GetString("sadministrador"));

                return View();
            }
            else
            {
                return RedirectToAction("LoginAdmi");
            }
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
                ViewBag.Director = Context.Directors;
                return View("CrearDirector");
            }
        }

        [HttpGet]
        [Route("UsuarioAdmi/EditarDirector/{Codigo}")]
        public IActionResult EditarDirector(int codigo)
        {
            var ObjSesion = HttpContext.Session.GetString("sadministrador");
            if (ObjSesion != null)
            {
                var Obj = JsonConvert.DeserializeObject
                    <Usuario>(HttpContext.Session.GetString("sadministrador"));

                var obj = (from Tdirector in Context.Directors where Tdirector.IdDirector == codigo select Tdirector).Single();
                return View(obj);
            }
            else
            {
                return RedirectToAction("LoginAdmi");
            }
        }

        public IActionResult EdiatarDire(Director objNew)
        {
            if (ModelState.IsValid)
            {
                var objOld = (from Tdirector in Context.Directors where Tdirector.IdDirector == objNew.IdDirector select Tdirector).Single();


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
        [HttpGet]
        public IActionResult ListarUsuarios()
        {
            var ObjSesion = HttpContext.Session.GetString("sadministrador");
            if (ObjSesion != null)
            {
                var Obj = JsonConvert.DeserializeObject
                    <Usuario>(HttpContext.Session.GetString("sadministrador"));

                var list = Context.Usuarios;
                return View(list);
            }
            else
            {
                return RedirectToAction("LoginAdmi");
            }
        }

        [Route("UsuarioAdmi/EliminarUsuario/{Codigo}")]
        public IActionResult EliminarUsuario(int codigo)
        {
            var obj = (from Tusuarios in Context.Usuarios where Tusuarios.IdUsuario == codigo select Tusuarios).Single();

            Context.Usuarios.Remove(obj);
            Context.SaveChanges();

            return RedirectToAction("ListarUsuarios");
        }

        [HttpGet]
        public IActionResult CrearUsuario()
        {
            var ObjSesion = HttpContext.Session.GetString("sadministrador");
            if (ObjSesion != null)
            {
                var Obj = JsonConvert.DeserializeObject
                    <Usuario>(HttpContext.Session.GetString("sadministrador"));

                return View();
            }
            else
            {
                return RedirectToAction("LoginAdmi");
            }
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
