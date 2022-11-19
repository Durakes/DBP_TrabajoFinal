using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaDBP.Models;
using System.Dynamic;
using System.Linq;

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
        public IActionResult Login2()
        {
            return View();
        }
        public IActionResult Validar2(UsuarioLogin2 usuario)
        {
            if (ModelState.IsValid)
            {
                var obj = (from userT in Context.Usuarios
                           where userT.Username == usuario.Username && userT.FechaNacimiento == (Convert.ToDateTime(usuario.FechaNacimiento)).ToString("yyyy-MM-dd")
                           select userT).FirstOrDefault();
                Console.WriteLine(usuario.FechaNacimiento);
                Console.WriteLine(usuario.Username);
                Console.WriteLine("Probando");

                if (obj != null)
                {
                    HttpContext.Session.SetString("sUsuario", JsonConvert.SerializeObject(obj));
                    return RedirectToAction("RenovarContraseña");
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
        public IActionResult RenovarContraseña()
        {
            var obj = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));
            

            return View();
        }
        public IActionResult ModificarContraseña(Cambio obj)
        {
            var obj1 = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));
            Console.WriteLine(obj.ContraseñaNueva1);
            Console.WriteLine(obj.ContraseñaNueva2);
            Console.WriteLine(obj1.IdUsuario);
            if (ModelState.IsValid)
            {
                var objAnterior = (from userT in Context.Usuarios
                                   where userT.IdUsuario == obj1.IdUsuario
                                   select userT).Single();
                Console.WriteLine(objAnterior);
                Console.WriteLine(obj.ContraseñaNueva1);
                Console.WriteLine(obj.ContraseñaNueva2);


                if (obj.ContraseñaNueva1 != obj.ContraseñaNueva2) //Si lo ingresado en contraseña nueva 1 y contraseña nueva 2 no son iguales
                {
                    return NoContent();
                }
                else
                {
                    objAnterior.Contraseña = obj.ContraseñaNueva2;
                    Context.SaveChanges();

                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("sUsuario", JsonConvert.SerializeObject(objAnterior));
                    return RedirectToAction("Index", "Usuario");
                }
            }
            else
            {
                return RedirectToAction("RenovarContraseña");
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

                    //Crear Estanterías default
                    var user = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));
                    Estanterium vistas = new Estanterium();
                    vistas.IdUsuario = user.IdUsuario;
                    vistas.NomEstanteria = "Vistas";
                    vistas.EsEditable = true;
                    vistas.FechaCreacion = DateTime.Now.ToString("yyyy-MM-dd");

                    Context.Estanteria.Add(vistas);

                    Estanterium porVer = new Estanterium();
                    porVer.IdUsuario = user.IdUsuario;
                    porVer.NomEstanteria = "Por Ver";
                    porVer.EsEditable = true;
                    porVer.FechaCreacion = DateTime.Now.ToString("yyyy-MM-dd");

                    Context.Estanteria.Add(porVer);

                    Context.SaveChanges();
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

        //Peliculas recomendadas en carrusel
        public IActionResult Index()
        {
            //Recuperar ult peli agregada
            List<PeliAgre> listaAgregadas = new List<PeliAgre>();
            listaAgregadas = PeliculasList();

            if(!listaAgregadas.Any()) //Si la vista está vacía
            {
                Random rnd = new Random();

                //Obtener tres indices random entre 0 y 1000
                int index1 = rnd.Next(0,1000);
                int index2 = rnd.Next(0,1000);
                int index3 = rnd.Next(0,1000);

                //Obtener los las peliculas correspondientes a esos Index
                var peli1 = (from pel in Context.Peliculas where pel.IdPelicula == index1 select pel).Single();
                var peli2 = (from pel in Context.Peliculas where pel.IdPelicula == index2 select pel).Single();
                var peli3 = (from pel in Context.Peliculas where pel.IdPelicula == index3 select pel).Single();

                Recomendaciones objRecom = new Recomendaciones();
                objRecom.peli1 = peli1;
                objRecom.peli2 = peli2;
                objRecom.peli3 = peli3;
                return View(objRecom);
            }
            else
            {
                var ultPel = listaAgregadas.Last();

                //Recuperar un tag de la pelicula
                var catPel = (from cat in Context.PeliculaCategoria where cat.IdPelicula == ultPel.IdPelicula select cat).FirstOrDefault();

                //Hacer una lista de las peliculas con esa categoria
                var listPelisCateg = (from pel in Context.PeliculaCategoria where pel.IdCategoria == catPel.IdCategoria select pel).ToList();

                //Obtener numero de elementos en la lista listPelisCateg
                var total = listPelisCateg.Count();

                //Obtener tres indices random entre 0 y total
                Random rnd = new Random();

                int index1 = rnd.Next(0,total);
                int index2 = rnd.Next(0,total);
                int index3 = rnd.Next(0,total);

                //Obtener los las peliculas correspondientes a esos Index
                var peli1 = (from pel in Context.Peliculas where pel.IdPelicula == listPelisCateg[index1].IdPelicula select pel).Single();
                var peli2 = (from pel in Context.Peliculas where pel.IdPelicula == listPelisCateg[index2].IdPelicula select pel).Single();
                var peli3 = (from pel in Context.Peliculas where pel.IdPelicula == listPelisCateg[index3].IdPelicula select pel).Single();

                Recomendaciones objRecom = new Recomendaciones();
                objRecom.peli1 = peli1;
                objRecom.peli2 = peli2;
                objRecom.peli3 = peli3;

                return View(objRecom);
            }
            
        }

        public IActionResult Perfil()
        {
            PerfilPelis obj = Generar();

            return View(obj);
        }
        public PerfilPelis Generar()
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));

            PerfilPelis obj = new PerfilPelis();
            obj.Agregadas = PeliculasList();
            obj.Descripcion = usuario.Descripcion;
            obj.UrlFoto = usuario.UrlFoto;
            obj.ApeUsuario = usuario.ApeUsuario;
            obj.IdUsuario = usuario.IdUsuario;
            obj.Contraseña = usuario.Contraseña;
            obj.NomUsuario = usuario.NomUsuario;
            obj.Username = usuario.Username;
            obj.FechaCreacion = usuario.FechaCreacion;
            Console.WriteLine(obj.Agregadas);

            return obj;
        }
        public List<PeliAgre> PeliculasList()
        { var sUser = HttpContext.Session.GetString("sUsuario");
            var usuario = JsonConvert.DeserializeObject<Usuario>(sUser);
            List<PeliAgre> lista = new List<PeliAgre>();
            var listRegistros = (from obj in Context.Estanteria
                                 where obj.IdUsuario == usuario.IdUsuario && obj.NomEstanteria=="Vistas"
                                 select obj).ToList();

            foreach (var item in listRegistros)
            {
                var obj2 = (from pel in Context.PeliculaEstanteria  where pel.IdEstanteria == item.IdEstanteria select pel).ToList();
                
                foreach (var item2 in obj2)
                {
                    var obj = (from pel in Context.Peliculas where pel.IdPelicula == item2.IdPelicula select pel).Single();
                    var directorPel = (from dir in Context.PeliculaDirectors where obj.IdPelicula == dir.IdPelicula select dir).ToList();
                    List<Director> listaDirectores = new List<Director>();
                    foreach (var dir in directorPel)
                    {
                        Console.WriteLine("************************+" + dir.IdDirector);
                        var directorReg = (from director in Context.Directors where director.IdDirector == dir.IdDirector select director).Single();
                        listaDirectores.Add(directorReg);
                    }
                    PeliAgre registro = new PeliAgre();
                    registro.IdPelicula = obj.IdPelicula;
                    registro.IdTmdb=obj.IdTmdb;
                    registro.NomPelicula = obj.NomPelicula;
                    registro.directoresLista = listaDirectores;
                    registro.Sumilla = obj.Sumilla;
                    registro.FechaEstreno = obj.FechaEstreno;
                    registro.UrlFoto = obj.UrlFoto;
                    registro.estanteria = item.NomEstanteria;
                    registro.IdEstanteria = item.IdEstanteria;
                    registro.FechaAgregar = item2.FechaAgregacion;
                    if (sUser != null)
                    {
                        registro.valoracion = (from valUser in Context.ValoracionUsuarios where valUser.IdPelicula == obj.IdPelicula && valUser.IdUsuario == usuario.IdUsuario select valUser).FirstOrDefault();
                        

                    }
                    else
                    {
                        registro.valoracion = null;
                    }
                    lista.Add(registro);
                }
            
            }
            

            return lista.OrderByDescending(p=>p.FechaAgregar).Take(5).ToList();
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

        public IActionResult ModificarFoto(UsuarioModif obj, string foto)
        {
            
            if (ModelState.IsValid)
            {
                var objAnterior = (from userT in Context.Usuarios
                                   where userT.IdUsuario == obj.IdUsuario
                                   select userT).Single();
                Console.WriteLine(foto);
                Console.WriteLine(objAnterior.UrlFoto);
                objAnterior.UrlFoto = foto;
                Console.WriteLine(objAnterior.UrlFoto);
                Context.SaveChanges();

                HttpContext.Session.Clear();
                HttpContext.Session.SetString("sUsuario", JsonConvert.SerializeObject(objAnterior));

                return RedirectToAction("EditarPerfil");


            }
            else
            {
                return RedirectToAction("EditarPerfil");
            }
        }

        
    }
}
