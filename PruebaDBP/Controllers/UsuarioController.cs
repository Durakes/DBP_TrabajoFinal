using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                return RedirectToAction("Login");
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
                return RedirectToAction("Registro");
            }

            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Perfil()
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));
            Console.WriteLine(usuario.Username);
            return View(usuario);
        }

        public IActionResult EditarPerfil()
        {
            return View();
        }
    }
}
