﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PruebaDBP.Controllers
{
    public class EstanteriaController : Controller
    {
        private readonly bdlumiereContext Context;
        private Paginador<PeliculaLib> _PaginadorLibPel;
        private readonly int _RegistrosPorPagina = 10;
        public EstanteriaController(bdlumiereContext context)
        {
            Context = context;
        }
        [Route("Estanterias")]
        public IActionResult Libreria(int idLib, int pagina=1)
        {
            Console.Write("HOLAAA" +idLib+ "\n");
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));
            var listEstanterias = (from est in Context.Estanteria
                                    where est.IdUsuario == usuario.IdUsuario
                                    select est).ToList();
            var idLibreria = 0;
            //Si se redirecciona desde otra pagina:
            if(idLib == 0)
            {
                //Recuperar id de estantería "Vistas" del usuario
                var vistasId = (from lib in listEstanterias
                            where lib.NomEstanteria == "Vistas"
                            select lib).Single();
                idLibreria = vistasId.IdEstanteria;
                ViewData["nomLibreria"] = "Vistas";
            }
            //Si se viene luego de hacer click en una libreria
            else 
            {
                idLibreria = idLib;
                var lib = (from obj in Context.Estanteria where obj.IdEstanteria == idLib select obj).Single();
                ViewData["nomLibreria"] = lib.NomEstanteria;
            }
            //Paginador y lista peliculas
            List<PeliculaLib> listaPels = new List<PeliculaLib>();
            List<PeliculaLib> lista = listPeliculas(idLibreria);
                listaPels = lista.Skip((pagina - 1) * _RegistrosPorPagina).Take(_RegistrosPorPagina).ToList();
                // Número total de páginas de la tabla Pelicula
                var _TotalPaginas = (int)Math.Ceiling((double) lista.Count()/ _RegistrosPorPagina);
                // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
                _PaginadorLibPel = new Paginador<PeliculaLib>()
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
            libPel.idLibAct = idLibreria;
            
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
        public IActionResult EliminarPelicula(int idLibreria, int idPel)
        {
            var obj = (from reg in Context.PeliculaEstanteria where reg.IdEstanteria == idLibreria && reg.IdPelicula == idPel select reg).Single();
            Context.PeliculaEstanteria.Remove(obj);
            Context.SaveChanges();

            return RedirectToAction("Libreria", new { idLib = idLibreria });
        }
        
        //Recuperar la lista de peliculas de cada estantería
        public List<PeliculaLib> listPeliculas(int idLib)
        {
            List<PeliculaLib> lista = new List<PeliculaLib>();
            var listRegistros = (from obj in Context.PeliculaEstanteria
                                where obj.IdEstanteria == idLib
                                select obj).ToList();
                                
            foreach(var item in listRegistros)
            {
                var obj = (from pel in Context.Peliculas where pel.IdPelicula == item.IdPelicula select pel).Single();
                var directorPel = (from dir in Context.PeliculaDirectors where obj.IdPelicula == dir.IdPelicula select dir).FirstOrDefault();
                var directorReg = (from director in Context.Directors where director.IdDirector == directorPel.IdDirector select director).Single();

                PeliculaLib registro = new PeliculaLib();
                registro.IdPelicula = obj.IdPelicula;
                registro.FechaAgregacion = item.FechaAgregacion;
                registro.NomPelicula = obj.NomPelicula;
                registro.NomDirector = directorReg.NomDirector;
                registro.UrlFoto = obj.UrlFoto;
                lista.Add(registro);
            }

            return lista;
        }
    }
}
