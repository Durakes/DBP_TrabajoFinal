using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PruebaDBP.Controllers
{
    public class EstanteriaController : Controller
    {
        private readonly bdlumiereContext Context;
        private Paginador<PeliLib> _PaginadorLibPel;
        private readonly int _RegistrosPorPagina = 10;
        public EstanteriaController(bdlumiereContext context)
        {
            Context = context;
        }
        [Route("Estanterias")]
        public IActionResult Libreria(int idLib, int pagina=1)
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));
            var listEstanterias = (from est in Context.Estanteria
                                    where est.IdUsuario == usuario.IdUsuario
                                    select est).ToList();
            var idLibreria = 0;
            Estanterium libAct = new Estanterium();
            //Si se redirecciona desde otra pagina:
            if(idLib == 0)
            {
                //Recuperar id de estantería "Vistas" del usuario
                libAct = (from libreria in listEstanterias
                            where libreria.NomEstanteria == "Vistas"
                            select libreria).Single();
                
                
            }
            //Si se viene luego de hacer click en una libreria
            else 
            {
                libAct = (from obj in Context.Estanteria where obj.IdEstanteria == idLib select obj).Single();
                
            }
            //Paginador y lista peliculas
            List<PeliLib> listaPels = new List<PeliLib>();
            List<PeliLib> lista = listPeliculas(libAct.IdEstanteria);
                listaPels = lista.Skip((pagina - 1) * _RegistrosPorPagina).Take(_RegistrosPorPagina).ToList();
                // Número total de páginas de la tabla Pelicula
                var _TotalPaginas = (int)Math.Ceiling((double) lista.Count()/ _RegistrosPorPagina);
                // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
                _PaginadorLibPel = new Paginador<PeliLib>()
                {
                    RegistrosPorPagina = _RegistrosPorPagina,
                    TotalRegistros = lista.Count(),
                    TotalPaginas = _TotalPaginas,
                    PaginaActual = pagina,
                    Resultado = listaPels,
                    nombreABuscar = ""
                };
            
            //Crear nuevo modelo de librerias y peliculas
            LibreriaPelis libPel= new LibreriaPelis();
            libPel.listLib = listEstanterias;
            libPel.listPelis = _PaginadorLibPel;
            libPel.LibAct = libAct;
            
            return View(libPel);
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
        public IActionResult ModificarLibreria(LibreriaPelis libModif)
        {
            var libreria = (from obj in Context.Estanteria where obj.IdEstanteria == libModif.LibAct.IdEstanteria select obj).Single(); 
            libreria.NomEstanteria = libModif.LibAct.NomEstanteria;
            Context.SaveChanges();
            return RedirectToAction("Libreria",new { idLib = libModif.LibAct.IdEstanteria });
        }
        public IActionResult EliminarLibreria(int idLib)
        {
            var libreria = (from obj in Context.Estanteria where obj.IdEstanteria == idLib select obj).Single(); 
            Context.Remove(libreria);
            Context.SaveChanges();
            return RedirectToAction("Libreria");

        }
        public IActionResult EliminarPelicula(int idLibreria, int idPel)
        {
            var obj = (from reg in Context.PeliculaEstanteria where reg.IdEstanteria == idLibreria && reg.IdPelicula == idPel select reg).Single();
            Context.PeliculaEstanteria.Remove(obj);
            Context.SaveChanges();

            return RedirectToAction("Libreria", new { idLib = idLibreria });
        }
        
        //Recuperar la lista de peliculas de cada estantería
        public List<PeliLib> listPeliculas(int idLib)
        {
            List<PeliLib> lista = new List<PeliLib>();
           var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));

            var listRegistros = (from obj in Context.PeliculaEstanteria
                                where obj.IdEstanteria == idLib
                                select obj).ToList();
            foreach(var item in listRegistros.OrderByDescending(x => x.FechaAgregacion))
            {
                //selecciono la pelicula
                var objPelicula = (from pel in Context.Peliculas where pel.IdPelicula == item.IdPelicula select new PeliLib{
                                IdPelicula = pel.IdPelicula,
                                IdTmdb = pel.IdTmdb,
                                Valoracion = pel.Valoracion,
                                FechaAgregacion = item.FechaAgregacion,
                                NomPelicula = pel.NomPelicula,
                                UrlFoto = pel.UrlFoto,
                                }).Single();
                                
                //Busco los ID de los directores de esa pelicula
                var directorPel = (from dir in Context.PeliculaDirectors where objPelicula.IdPelicula == dir.IdPelicula select dir).ToList();

                //Recuperar los nombres de los directores
                List<Director> listaDirectores = new List<Director>();
                foreach(var dir in directorPel)
                {
                    var directorReg = (from director in Context.Directors where director.IdDirector == dir.IdDirector select director).Single();
                    listaDirectores.Add(directorReg);
                }
                objPelicula.directores = listaDirectores;

                //Recuperar la valoración
                objPelicula.valoracion = (from valUser in Context.ValoracionUsuarios where valUser.IdPelicula == objPelicula.IdPelicula && valUser.IdUsuario == usuario.IdUsuario select valUser).FirstOrDefault();
                
                lista.Add(objPelicula);
            }            

            return lista;
        }
    }
}
