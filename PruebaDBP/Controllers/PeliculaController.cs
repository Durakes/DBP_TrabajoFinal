using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PruebaDBP.Models;
using System;
using System.IO;
using System.Net.Http;

namespace PruebaDBP.Controllers
{
    public class PeliculaController : Controller
    {
        private readonly bdlumiereContext Context;
        public PeliculaController(bdlumiereContext context)
        {
            Context = context;
        }
        public async Task<IActionResult> Index(int id)
        {
            var objPelicula = (from peliculaT in Context.Peliculas
                               where peliculaT.IdTmdb == id
                               select peliculaT).FirstOrDefault();

            int idPelicula;
            string sumilla;
            string posterUrl;
            string titulo;
            string fecha;
            int? minutos;
            string lenguaje;
            List<String> generos = new List<string>();
            List<int?> directores = new List<int?>();
            List<Director> listDirectores = new List<Director>(); //Lista que se pasa a vista
            int? idDirectorTmdb;
            string nombreDirector;
            string directorUrl;
            string bioDirector;
            Pelicula objPeliculaRegistro;
            List<ComentarioIndex> listaComentario = new List<ComentarioIndex>();

            

            var baseAddress = new Uri("http://api.themoviedb.org/3/");

            

            if (objPelicula == null)
            {
                Pelicula lastPelicula2;
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    using (var detalle = await httpClient.GetAsync("movie/" + id.ToString() + "?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es"))
                    {
                        string detalleData = await detalle.Content.ReadAsStringAsync();
                        dynamic detallePelicula = JsonConvert.DeserializeObject(detalleData);
                        sumilla = detallePelicula.overview;
                        posterUrl = "https://image.tmdb.org/t/p/w500" + detallePelicula.poster_path;
                        titulo = detallePelicula.title;
                        fecha = detallePelicula.release_date;
                        minutos = detallePelicula.vote_average;
                        lenguaje = detallePelicula.original_language;
                        generos = new List<string>();
                        /*Probar esta parte luego*/
                        //dynamic generosv2 = detallePelicula.genres;
                        foreach (var genero in detallePelicula.genres)
                        {
                            string nombreGenero = genero.name;
                            generos.Add(nombreGenero);
                        }

                        var ObjIdioma = (from Tleng in Context.Idiomas where Tleng.Abreviacion == lenguaje select Tleng).Single();

                        objPeliculaRegistro = new Pelicula(id, ObjIdioma.IdIdioma, titulo, fecha, minutos, sumilla, posterUrl);
                        if (ModelState.IsValid)
                        {
                            Context.Peliculas.Add(objPeliculaRegistro);
                            Context.SaveChanges();

                            Pelicula lastPelicula = Context.Peliculas.OrderByDescending(p => p.IdPelicula).FirstOrDefault();
                            foreach (String genero in generos)
                            {
                                var ObjCategoria = (from TCat in Context.Categoria where TCat.NomCategoria == genero select TCat).Single();
                                PeliculaCategorium registroCategoria = new PeliculaCategorium(lastPelicula.IdPelicula, ObjCategoria.IdCategoria);
                                Context.PeliculaCategoria.Add(registroCategoria);
                                Context.SaveChanges();
                            }
                        }
                    }

                    using (var creditos = await httpClient.GetAsync("movie/" + id.ToString() + "/credits?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es"))
                    {
                        string responseCredits = await creditos.Content.ReadAsStringAsync();

                        dynamic personasResult = JsonConvert.DeserializeObject(responseCredits);

                        foreach (var objPersona in personasResult.crew)
                        {
                            if (objPersona.job == "Director")
                            {
                                int idTemp = objPersona.id;
                                directores.Add(idTemp);
                            }
                        }
                    }

                    lastPelicula2 = Context.Peliculas.OrderByDescending(p => p.IdPelicula).FirstOrDefault();
                    foreach (int? i in directores)
                    {
                        using (var peopledetalle = await httpClient.GetAsync("person/" + i.ToString() + "?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es"))
                        {
                            string responseDetailPeople = await peopledetalle.Content.ReadAsStringAsync();

                            dynamic personaResult = JsonConvert.DeserializeObject(responseDetailPeople);

                            nombreDirector = personaResult.name;
                            //idDirectorTmdb = personaResult.id;
                            directorUrl = "https://image.tmdb.org/t/p/w500" + personaResult.profile_path;
                            bioDirector = personaResult.biography;

                            Director ObjDirector = new Director();

                            if (Context.Directors.Any(x => x.IdDirTmdb == i) == true)
                            {
                                ObjDirector = (from TDir in Context.Directors where TDir.IdDirTmdb == i select TDir).Single();
                            }
                            else
                            {
                                Director director = new Director(i, nombreDirector, bioDirector, directorUrl);
                                if (ModelState.IsValid)
                                {
                                    Context.Directors.Add(director);
                                    Context.SaveChanges();
                                }

                                ObjDirector = Context.Directors.OrderByDescending(p => p.IdDirector).FirstOrDefault();
                            }
                            listDirectores.Add(ObjDirector);
                            PeliculaDirector registroDirector = new PeliculaDirector(lastPelicula2.IdPelicula, ObjDirector.IdDirector);
                            if (ModelState.IsValid)
                            {
                                Context.PeliculaDirectors.Add(registroDirector);
                                Context.SaveChanges();
                            }
                        }
                    }
                }

                //Objeto que se devolverá a la vista
                IndexPelicula objVista = new IndexPelicula();
                objVista.objPelicula = lastPelicula2;
                objVista.listCategoria = generos;
                objVista.listDirectores = listDirectores;
                ComentarioIndex objComentariosIndex = new ComentarioIndex();
                listaComentario.Add(objComentariosIndex);
                objVista.listComentario = listaComentario;


                var sesUsuario = (HttpContext.Session.GetString("sUsuario"));
                if(sesUsuario != null)
                {
                    objVista.usuario = JsonConvert.DeserializeObject<Usuario>(sesUsuario);
                    objVista.valoracion = (from valUser in Context.ValoracionUsuarios where valUser.IdPelicula == objVista.objPelicula.IdPelicula && valUser.IdUsuario == objVista.usuario.IdUsuario select valUser).FirstOrDefault();
                    objVista.listEstanterias = (from est in Context.Estanteria
                                    where est.IdUsuario == objVista.usuario.IdUsuario
                                    select est).ToList();
                    
                }
                else
                {
                    objVista.usuario = null;
                    objVista.valoracion = null;
                    objVista.listEstanterias = null;
                }

                return View(objVista);
            }
            else
            {
                IndexPelicula objVista = new IndexPelicula();
                objVista.objPelicula = objPelicula;

                //Obtener categorias
                var catPel = (from cat in Context.PeliculaCategoria where cat.IdPelicula == objPelicula.IdPelicula select cat).ToList();
                List<String> listaCateg = new List<String>();
                foreach(var cat in catPel)
                {
                    var categoria = (from categ in Context.Categoria where categ.IdCategoria == cat.IdCategoria select categ).Single();
                    listaCateg.Add(categoria.NomCategoria);
                }
                objVista.listCategoria = listaCateg.Take(3).ToList();

                //Obtener directores
                
                var directorPel = (from dir in Context.PeliculaDirectors where dir.IdPelicula == objPelicula.IdPelicula select dir).ToList();
                List<Director> listaDirectores = new List<Director>();
                foreach(var dir in directorPel)
                {
                    Console.WriteLine("************************+"+dir.IdDirector);
                    var directorReg = (from director in Context.Directors where director.IdDirector == dir.IdDirector select director).Single();
                    listaDirectores.Add(directorReg);
                }
                    string NombreNull = "No disponible";
                    Director objDirector = new Director();
                    objDirector.NomDirector = NombreNull;
                    listaDirectores.Add(objDirector);
                    objVista.listDirectores = listaDirectores;
                

                //Obtener info de usuario + estanterías
                var sesUsuario = (HttpContext.Session.GetString("sUsuario"));
                
                if(sesUsuario != null)
                {
                    objVista.usuario = JsonConvert.DeserializeObject<Usuario>(sesUsuario);
                    objVista.valoracion = (from valUser in Context.ValoracionUsuarios where valUser.IdPelicula == objVista.objPelicula.IdPelicula && valUser.IdUsuario == objVista.usuario.IdUsuario select valUser).FirstOrDefault();
                    objVista.listEstanterias = (from est in Context.Estanteria
                                    where est.IdUsuario == objVista.usuario.IdUsuario
                                    select est).ToList();
                    
                }
                else
                {
                    objVista.usuario = null;
                    objVista.valoracion = null;
                    objVista.listEstanterias = null;
                }

                var comentariosPel = (from pelComent in Context.Comentarios where pelComent.IdPelicula == objPelicula.IdPelicula select pelComent).ToList();
                foreach(var item in comentariosPel)
                {
                    var userVal = (from pelUser in Context.Usuarios where pelUser.IdUsuario == item.IdUsuario select pelUser).FirstOrDefault();
                    var ValoracionPel = (from pelVal in Context.ValoracionUsuarios where pelVal.IdUsuario == item.IdUsuario && pelVal.IdPelicula==item.IdPelicula select pelVal).FirstOrDefault();
                    ComentarioIndex objComentariosIndex = new ComentarioIndex();
                    Console.WriteLine(ValoracionPel.Valoracion);
                    objComentariosIndex.UsuarioValor = ValoracionPel;
                    objComentariosIndex.CometariosUsers = item;
                    objComentariosIndex.User = userVal;
                    listaComentario.Add(objComentariosIndex);
                }
                objVista.listComentario = listaComentario;
                return View(objVista);
            }
        }

        public IActionResult aparecer(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        //private List<Pelicula> _pelicula;
        //List<Pelicula> peliculasEncontradasAPI = new List<Pelicula>();
        private readonly int _RegistrosPorPagina = 10;
        private Paginador<PeliculaTop> _PaginadorPelicula;
        private Paginador<PeliculaTop> _PaginadorPeliculaTop;


        public async Task<IActionResult> Resultados(string nombreBuscar, int pagina = 1)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            List<PeliculaTop> peliculasEncontradasAPI = new List<PeliculaTop>();

            var baseAddress = new Uri("http://api.themoviedb.org/3/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                int? page = 1;
                int? totalpages = 1;
                int totalResult = 0;
                int idPeliculaTMDB;
                string lenguaje;
                string titulo;
                string fecha;
                int valoracion = 0;
                string sumilla;
                string posterURL;
                List<String> generos = new List<string>();

                while (page <= totalpages)
                {
                    using (var busqueda = await httpClient.GetAsync("search/movie?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es&query=" + nombreBuscar + "&page=" + page.ToString() + "&include_adult=false"))
                    {
                        string resultadosBusqueda = await busqueda.Content.ReadAsStringAsync();

                        dynamic busquedaDatos = JsonConvert.DeserializeObject(resultadosBusqueda);
                        totalpages = busquedaDatos.total_pages;
                        page = busquedaDatos.page;
                        totalResult = busquedaDatos.total_results;

                        foreach (var objPelicula in busquedaDatos.results)
                        {
                            idPeliculaTMDB = objPelicula.id;
                            //
                            
                            using (var detalle = await httpClient.GetAsync("movie/" + idPeliculaTMDB.ToString() + "?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es"))
                            {
                                string detalleData = await detalle.Content.ReadAsStringAsync();
                                dynamic detallePelicula = JsonConvert.DeserializeObject(detalleData);
                                sumilla = detallePelicula.overview;
                                if (objPelicula.poster_path != null)
                                    posterURL = "https://image.tmdb.org/t/p/w500" + objPelicula.poster_path;
                                else
                                    posterURL = "";
                                titulo = detallePelicula.title;
                                fecha = detallePelicula.release_date;
                                lenguaje = detallePelicula.original_language;
                                generos = new List<string>();
                                valoracion = detallePelicula.vote_average;
                                /*Probar esta parte luego*/
                                //dynamic generosv2 = detallePelicula.genres;
                                foreach (var genero in detallePelicula.genres)
                                {
                                    string nombreGenero = genero.name;
                                    generos.Add(nombreGenero);
                                }

                                var ObjIdioma = (from Tleng in Context.Idiomas where Tleng.Abreviacion == lenguaje select Tleng).Single();
                                Pelicula objPeliculaEncontrado = new Pelicula(idPeliculaTMDB, ObjIdioma.IdIdioma, titulo, fecha, valoracion, sumilla, posterURL);

                                //

                                PeliculaTop PeliculasMostrar = new PeliculaTop();
                                PeliculasMostrar.PeliTop = objPeliculaEncontrado;
                                PeliculasMostrar.listaCategorias = generos;
                                peliculasEncontradasAPI.Add(PeliculasMostrar);
                            }
                        }
                    }
                    page++;
                }
                List<PeliculaTop> pruebita2 = new List<PeliculaTop>();
                pruebita2 = peliculasEncontradasAPI.Skip((pagina - 1) * _RegistrosPorPagina).Take(_RegistrosPorPagina).ToList();
                // Número total de páginas de la tabla Pelicula
                var _TotalPaginas = (int)Math.Ceiling((double)totalResult / _RegistrosPorPagina);
                // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
                _PaginadorPelicula = new Paginador<PeliculaTop>()
                {
                    RegistrosPorPagina = _RegistrosPorPagina,
                    TotalRegistros = totalResult,
                    TotalPaginas = _TotalPaginas,
                    PaginaActual = pagina,
                    Resultado = pruebita2,
                    nombreABuscar = nombreBuscar
                };
                // Enviamos a la Vista la 'Clase de paginación'

                return View(_PaginadorPelicula);
            }
        }

        //Agregar la pelicula a una estantería
        public IActionResult AgregarPelicula(int idPel, int idEst)
        {
            var listRegistros = (from obj in Context.PeliculaEstanteria
                                where obj.IdEstanteria == idEst
                                select obj.IdPelicula).ToList();
            
            if(listRegistros.Contains(idPel))
            {
                return NoContent();
            }
            else
            {
                PeliculaEstanterium obj = new PeliculaEstanterium();
                obj.IdEstanteria = idEst;
                obj.IdPelicula = idPel;
                obj.FechaAgregacion = DateTime.Now.ToString("yyyy-MM-dd");

                Context.PeliculaEstanteria.Add(obj);
                Context.SaveChanges();
                return NoContent();
            }
            
        }

        //Marcar pelicula como vista
        public IActionResult PeliculaVista(int idPel, int idUser)
        {
            var valoracion = (from valUser in Context.ValoracionUsuarios where valUser.IdPelicula == idPel && valUser.IdUsuario == idUser select valUser).FirstOrDefault();
            var pelicula = (from pel in Context.Peliculas where pel.IdPelicula == idPel select pel).Single();
            var libVistas = (from lib in Context.Estanteria where lib.IdUsuario == idUser && lib.NomEstanteria == "Vistas" select lib).Single();
            var libPorVer = (from lib in Context.Estanteria where lib.IdUsuario == idUser && lib.NomEstanteria == "Por Ver" select lib).Single();

            if(valoracion == null)
            {
                ValoracionUsuario objVal = new ValoracionUsuario();
                objVal.EstaVisto = 1;
                objVal.IdPelicula = idPel;
                objVal.IdUsuario = idUser;

                PeliculaEstanterium agregar = new PeliculaEstanterium();
                agregar.IdEstanteria = libVistas.IdEstanteria;
                agregar.IdPelicula = idPel;
                agregar.FechaAgregacion = DateTime.Now.ToString("yyyy-MM-dd");

                var eliminar = (from lib in Context.PeliculaEstanteria where lib.IdEstanteria == libPorVer.IdEstanteria && lib.IdPelicula == idPel select lib).FirstOrDefault();
                if(eliminar != null)
                {
                    Context.PeliculaEstanteria.Remove(eliminar);
                }
                Context.PeliculaEstanteria.Add(agregar);
                Context.ValoracionUsuarios.Add(objVal);

            }
            else
            {
                if(valoracion.EstaVisto == 1)
                {
                    valoracion.EstaVisto = 0;
                    var eliminar = (from lib in Context.PeliculaEstanteria where lib.IdEstanteria == libVistas.IdEstanteria && lib.IdPelicula == idPel select lib).Single();
                    Context.PeliculaEstanteria.Remove(eliminar);
                }
                else
                {
                    valoracion.EstaVisto = 1;
                    PeliculaEstanterium agregar = new PeliculaEstanterium();
                    agregar.IdEstanteria = libVistas.IdEstanteria;
                    agregar.IdPelicula = idPel;
                    agregar.FechaAgregacion = DateTime.Now.ToString("yyyy-MM-dd");

                    var eliminar = (from lib in Context.PeliculaEstanteria where lib.IdEstanteria == libPorVer.IdEstanteria && lib.IdPelicula == idPel select lib).FirstOrDefault();
                    if(eliminar != null)
                    {
                        Context.PeliculaEstanteria.Remove(eliminar);
                    }
                    Context.PeliculaEstanteria.Add(agregar);
                }
                
            }
            
            Context.SaveChanges();

            return RedirectToAction("Index",new { id = pelicula.IdTmdb });
        }

        //Valorar pelicula
        public IActionResult ValorarPelicula(int idPel, int idUser)
        {
            var calificacion = Int32.Parse( Request.Form["valoracion"]);
            var pelicula = (from pel in Context.Peliculas where pel.IdPelicula == idPel select pel).Single();
      
            var valoracion = (from valUser in Context.ValoracionUsuarios where valUser.IdPelicula == idPel && valUser.IdUsuario == idUser select valUser).Single();
            valoracion.Valoracion = calificacion;
            valoracion.FechaValoracion = DateTime.Now.ToString("yyyy-MM-dd");
            
            //Console.Write("\n"+"CALIFFFF "+calificacion+"\n");

            Context.SaveChanges();
            return RedirectToAction("Index",new { id = pelicula.IdTmdb });
        }
        public IActionResult SubirComentario(int idPel, string? comentario)
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("sUsuario"));

            var pelicula = (from pel in Context.Peliculas where pel.IdPelicula == idPel select pel).Single();

            string? Fecha= DateTime.Now.ToString("yyyy-MM-dd");
            Comentario ObjComentario = new Comentario();
            ObjComentario.IdUsuario = usuario.IdUsuario;
            ObjComentario.IdPelicula=idPel;
            ObjComentario.FechaPublicacion=Fecha;
            ObjComentario.Texto = comentario;
            ObjComentario.Estado = true;

            Context.Comentarios.Add(ObjComentario);
            Context.SaveChanges();
            return RedirectToAction("Index", new { id = pelicula.IdTmdb });
        }

        public IActionResult TopMovie( int pagina=1)
        {

              
            List<PeliculaTop> PelisMejorValoradas = new List<PeliculaTop>();
            var pelis = (from pel in Context.Peliculas select pel).OrderByDescending((x => x.Valoracion)).ToList();
            foreach(var item in pelis)
            {
                PeliculaTop PeliculasMejorVal = new PeliculaTop();
                PeliculasMejorVal.PeliTop = item;
                var directorPel = (from dir in Context.PeliculaDirectors where dir.IdPelicula == item.IdPelicula select dir).ToList();
                List<Director> listaDirectores = new List<Director>();
                foreach (var dir in directorPel)
                {
                    Console.WriteLine("************************+" + dir.IdDirector);
                    var directorReg = (from director in Context.Directors where director.IdDirector == dir.IdDirector select director).Single();
                    listaDirectores.Add(directorReg);
                }
                string NombreNull = "No disponible";
                Director objDirector = new Director();
                objDirector.NomDirector = NombreNull;
                listaDirectores.Add(objDirector);
                PeliculasMejorVal.directoresLista= listaDirectores;
                PelisMejorValoradas.Add(PeliculasMejorVal);
                var catPel = (from cat in Context.PeliculaCategoria where cat.IdPelicula == item.IdPelicula select cat).ToList();
                List<String> listaCateg = new List<String>();
                foreach (var cat in catPel)
                {
                    var categoria = (from categ in Context.Categoria where categ.IdCategoria == cat.IdCategoria select categ).Single();
                    listaCateg.Add(categoria.NomCategoria);
                }
                PeliculasMejorVal.listaCategorias = listaCateg.Take(3).ToList();
            }
            List<PeliculaTop> result = new List<PeliculaTop>();
            var totalReg=PelisMejorValoradas.Take(25).ToList();
            result = totalReg.Skip((pagina - 1) * _RegistrosPorPagina).Take(_RegistrosPorPagina).ToList();
            // Número total de páginas de la tabla Pelicula
            var _TotalPaginas = (int)Math.Ceiling((double)25 / _RegistrosPorPagina);
            // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
            _PaginadorPeliculaTop = new Paginador<PeliculaTop>()
            {
                RegistrosPorPagina = _RegistrosPorPagina,
                TotalRegistros = 25,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = result
            };
            return View(_PaginadorPeliculaTop);
        }

            
        public async Task AllMovies()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var baseAddress = new Uri("http://api.themoviedb.org/3/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                int idPelicula;
                string sumilla;
                string posterUrl;
                string titulo;
                string fecha;
                int? valoracion;
                string lenguaje;
                List<int> generos = new List<int>();
                List<int?> directores = new List<int?>();
                int? idDirectorTmdb;
                string nombreDirector;
                string directorUrl;
                string bioDirector;
                Pelicula objPeliculaRegistro;

                // api_key can be requestred on TMDB website
                int page = 1;
                //int totalpages = 10;
                while (page <= 50)
                {
                    using (var response = await httpClient.GetAsync("movie/top_rated?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es&page=" + page))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();

                        dynamic resultado = JsonConvert.DeserializeObject(responseData);
                        foreach (var objPelicula in resultado.results)
                        {
                            idPelicula = objPelicula.id;

                            using (var detalle = await httpClient.GetAsync("movie/" + idPelicula.ToString() + "?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es"))
                            {
                                string detalleData = await detalle.Content.ReadAsStringAsync();
                                if (detalleData == "")
                                    continue;
                                dynamic detallePelicula = JsonConvert.DeserializeObject(detalleData);
                                sumilla = detallePelicula.overview;
                                posterUrl = "https://image.tmdb.org/t/p/w500" + detallePelicula.poster_path;
                                titulo = detallePelicula.title;
                                fecha = detallePelicula.release_date;
                                valoracion = detallePelicula.vote_average;
                                lenguaje = detallePelicula.original_language;
                                generos = new List<int>();
                                /*Probar esta parte luego*/
                                //dynamic generosv2 = detallePelicula.genres;
                                foreach (var genero in detallePelicula.genres)
                                {
                                    int nombreGenero = genero.id;
                                    generos.Add(nombreGenero);
                                }

                                var ObjIdioma = (from Tleng in Context.Idiomas where Tleng.Abreviacion == lenguaje select Tleng).Single();

                                objPeliculaRegistro = new Pelicula(idPelicula, ObjIdioma.IdIdioma, titulo, fecha, valoracion, sumilla, posterUrl);
                                if (ModelState.IsValid)
                                {
                                    Context.Peliculas.Add(objPeliculaRegistro);
                                    Context.SaveChanges();

                                    Pelicula lastPelicula = Context.Peliculas.OrderByDescending(p => p.IdPelicula).FirstOrDefault();
                                    foreach(int genero in generos)
                                    {
                                        var ObjCategoria = (from TCat in Context.Categoria where TCat.IdTmdbCategoria == genero select TCat).Single();
                                        PeliculaCategorium registroCategoria = new PeliculaCategorium(lastPelicula.IdPelicula, ObjCategoria.IdCategoria);
                                        Context.PeliculaCategoria.Add(registroCategoria);
                                        Context.SaveChanges();
                                    }
                                }
                            }

                            using (var creditos = await httpClient.GetAsync("movie/" + idPelicula.ToString() + "/credits?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es"))
                            {
                                string responseCredits = await creditos.Content.ReadAsStringAsync();

                                dynamic personasResult = JsonConvert.DeserializeObject(responseCredits);

                                foreach (var objPersona in personasResult.crew)
                                {
                                    if (objPersona.job == "Director")
                                    {
                                        int idTemp = objPersona.id;
                                        directores.Add(idTemp);
                                    }
                                }
                            }

                            Pelicula lastPelicula2 = Context.Peliculas.OrderByDescending(p => p.IdPelicula).FirstOrDefault();
                            foreach (int? i in directores)
                            {
                                using (var peopledetalle = await httpClient.GetAsync("person/" + i.ToString() + "?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es"))
                                {
                                    string responseDetailPeople = await peopledetalle.Content.ReadAsStringAsync();

                                    dynamic personaResult = JsonConvert.DeserializeObject(responseDetailPeople);

                                    nombreDirector = personaResult.name;
                                    //idDirectorTmdb = personaResult.id;
                                    directorUrl = "https://image.tmdb.org/t/p/w500" + personaResult.profile_path;
                                    bioDirector = personaResult.biography;

                                    Director ObjDirector = new Director();

                                    if (Context.Directors.Any(x => x.IdDirTmdb == i) == true)
                                    {
                                        ObjDirector = (from TDir in Context.Directors where TDir.IdDirTmdb == i select TDir).Single();
                                    }
                                    else 
                                    {
                                        Director director = new Director(i, nombreDirector, bioDirector, directorUrl);
                                        if (ModelState.IsValid) 
                                        {
                                            Context.Directors.Add(director);
                                            Context.SaveChanges();
                                        }

                                        ObjDirector = Context.Directors.OrderByDescending(p => p.IdDirector).FirstOrDefault();
                                    }

                                    PeliculaDirector registroDirector = new PeliculaDirector(lastPelicula2.IdPelicula, ObjDirector.IdDirector);
                                    if (ModelState.IsValid)
                                    {
                                        Context.PeliculaDirectors.Add(registroDirector);
                                        Context.SaveChanges();
                                    }
                                }
                            }
                            directores.Clear();
                        }
                    }
                    page++;
                }

            }
        }
        
    }
}
