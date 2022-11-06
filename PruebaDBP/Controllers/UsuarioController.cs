using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Dynamic;
namespace PruebaDBP.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly bdlumiereContext Context;
        public UsuarioController(bdlumiereContext context)
        {
            Context = context;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Validar(UsuarioLogin usuario)
        {
            if(ModelState.IsValid)
            {
                var obj = (from userT in Context.Usuarios 
                            where userT.Username == usuario.Username && userT.Contraseña == usuario.Contraseña 
                            select userT).FirstOrDefault();
                
                if(obj != null)
                {
                    HttpContext.Session.SetString("sUsuario",JsonConvert.SerializeObject(obj));
                    return RedirectToAction("Index","Usuario");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return View("Login");
            }
            
        }
        public IActionResult Salir()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }

        public IActionResult Registro()
        {
            return View();
        }
        public IActionResult RegistrarUser(Usuario usuario)
        {
            if(ModelState.IsValid)
            {
                var obj = (from userT in Context.Usuarios 
                            where userT.Username == usuario.Username 
                            select userT).FirstOrDefault();
                
                if(obj == null)
                {
                    Usuario nuevoUsuario = new Usuario();
                    nuevoUsuario.NomUsuario = usuario.NomUsuario;
                    nuevoUsuario.ApeUsuario = usuario.ApeUsuario;
                    nuevoUsuario.Username = usuario.Username;
                    nuevoUsuario.Contraseña = usuario.Contraseña;
                    nuevoUsuario.FechaNacimiento = (Convert.ToDateTime(usuario.FechaNacimiento)).ToString("yyyy-MM-dd");
                    var fechaCreacion = DateTime.Now.ToString("yyyy-MM-dd");
                    nuevoUsuario.FechaCreacion = fechaCreacion;
                
                    Context.Usuarios.Add(nuevoUsuario);
                    Context.SaveChanges();

                    HttpContext.Session.SetString("sUsuario",JsonConvert.SerializeObject(nuevoUsuario));
                

                    return RedirectToAction("Index");
                }
                else
                {
                    
                    return View("Registro");
                }

            }
            else
            {
                return View("Registro");
            }

            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Perfil()
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));
            return View(usuario);
        }
        [Route("Editar")]
        public IActionResult EditarPerfil()
        {
            var obj = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));
            UsuarioModif userMod = new UsuarioModif();
            userMod.IdUsuario = obj.IdUsuario;
            userMod.NomUsuario = obj.NomUsuario;
            userMod.ApeUsuario = obj.ApeUsuario;
            userMod.Username = obj.Username;
            userMod.Descripcion = obj.Descripcion;
            userMod.Contraseña = obj.Contraseña;
            userMod.UrlFoto = obj.UrlFoto;

            return View(userMod);
        }
        public IActionResult ModificarPerfil(UsuarioModif obj)
        {
            if(ModelState.IsValid)
            {  
                var objAnterior = (from userT in Context.Usuarios 
                                where userT.IdUsuario == obj.IdUsuario
                                select userT).Single();

                var objUsuario = (from userT in Context.Usuarios 
                            where userT.Username == obj.Username 
                            select userT).FirstOrDefault();
                if(objUsuario == null ) //Si el username no está en la bd
                {
                    objAnterior.NomUsuario = obj.NomUsuario;
                    objAnterior.ApeUsuario = obj.ApeUsuario;
                    objAnterior.Username = obj.Username;
                    objAnterior.Descripcion = obj.Descripcion;
                    objAnterior.UrlFoto = obj.UrlFoto;

                    Context.SaveChanges();

                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("sUsuario",JsonConvert.SerializeObject(objAnterior));
                    return RedirectToAction("Perfil");
                }
                else if(objUsuario != null && objUsuario.IdUsuario == obj.IdUsuario) //Si el username está en la bd pero corresponde al mismo usuario
                {
                    objAnterior.NomUsuario = obj.NomUsuario;
                    objAnterior.ApeUsuario = obj.ApeUsuario;
                    objAnterior.Username = obj.Username;
                    objAnterior.Descripcion = obj.Descripcion;
                    objAnterior.UrlFoto = obj.UrlFoto;

                    Context.SaveChanges();

                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("sUsuario",JsonConvert.SerializeObject(objAnterior));
                    return RedirectToAction("Perfil");

                }
                else //Si el username le corresponde a otro usuario
                {
                    return NoContent();
                }
                
            }
            else
            {
                return RedirectToAction("EditarPerfil");
            }
            
        }
        public IActionResult CambiarContra(UsuarioModif obj)
        {
            if(ModelState.IsValid)
            {  
                var objAnterior = (from userT in Context.Usuarios 
                                where userT.IdUsuario == obj.IdUsuario
                                select userT).Single();
            
                if(obj.ContraseñaActual != objAnterior.Contraseña) //Si lo ingresado en contra actual no es igual a la ya existente
                { 
                    return NoContent();
                }
                else if(obj.ContraseñaNueva1 != obj.ContraseñaNueva2) //Si lo ingresado en contraseña nueva 1 y contraseña nueva 2 no son iguales
                {
                    return NoContent();
                }
                else
                {
                    objAnterior.Contraseña = obj.ContraseñaNueva2;
                    Context.SaveChanges();

                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("sUsuario",JsonConvert.SerializeObject(objAnterior));
                    return RedirectToAction("Perfil");
                }

                
            }
            else
            {
                return RedirectToAction("EditarPerfil");
            }
            
        }

    }
}
