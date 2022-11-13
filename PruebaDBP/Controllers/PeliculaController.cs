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
            int? idDirectorTmdb;
            string nombreDirector;
            string directorUrl;
            string bioDirector;
            Pelicula objPeliculaRegistro;

            

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

                            PeliculaDirector registroDirector = new PeliculaDirector(lastPelicula2.IdPelicula, ObjDirector.IdDirector);
                            if (ModelState.IsValid)
                            {
                                Context.PeliculaDirectors.Add(registroDirector);
                                Context.SaveChanges();
                            }
                        }
                    }
                }
                IndexPelicula objVista = new IndexPelicula();
                objVista.objPelicula = lastPelicula2;
                var sesUsuario = (HttpContext.Session.GetString("sUsuario"));
                
                if(sesUsuario != null)
                {
                    objVista.usuario = JsonConvert.DeserializeObject<Usuario>(sesUsuario);
                    objVista.listEstanterias = (from est in Context.Estanteria
                                    where est.IdUsuario == objVista.usuario.IdUsuario
                                    select est).ToList();
                    
                }
                else
                {
                    objVista.usuario = JsonConvert.DeserializeObject<Usuario>(sesUsuario);
                    objVista.listEstanterias = null;
                }

                return View(objVista);
            }
            else
            {
                IndexPelicula objVista = new IndexPelicula();
                objVista.objPelicula = objPelicula;
                var sesUsuario = (HttpContext.Session.GetString("sUsuario"));
                
                if(sesUsuario != null)
                {
                    objVista.usuario = JsonConvert.DeserializeObject<Usuario>(sesUsuario);
                    objVista.listEstanterias = (from est in Context.Estanteria
                                    where est.IdUsuario == objVista.usuario.IdUsuario
                                    select est).ToList();
                    
                }
                else
                {
                    objVista.usuario = null;
                    objVista.listEstanterias = null;
                }
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
        private Paginador<Pelicula> _PaginadorPelicula;
        
        public async Task<IActionResult> Resultados(string nombreBuscar, int pagina = 1)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            List<Pelicula> peliculasEncontradasAPI = new List<Pelicula>();

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
                            lenguaje = objPelicula.original_language;
                            titulo = objPelicula.title;
                            fecha = "2020-11-02";
                            valoracion = objPelicula.vote_average;
                            sumilla = objPelicula.overview;
                            if (objPelicula.poster_path != null)
                                posterURL = "https://image.tmdb.org/t/p/w500" + objPelicula.poster_path;
                            else
                                posterURL = "";

                            var ObjIdioma = (from Tleng in Context.Idiomas where Tleng.Abreviacion == lenguaje select Tleng).Single();
                            Pelicula objPeliculaEncontrado = new Pelicula(idPeliculaTMDB, ObjIdioma.IdIdioma, titulo, fecha, valoracion, sumilla, posterURL);
                            peliculasEncontradasAPI.Add(objPeliculaEncontrado);
                        }
                    }
                    page++;
                }
                List<Pelicula> pruebita2 = new List<Pelicula>();
                pruebita2 = peliculasEncontradasAPI.Skip((pagina - 1) * _RegistrosPorPagina).Take(_RegistrosPorPagina).ToList();
                // Número total de páginas de la tabla Pelicula
                var _TotalPaginas = (int)Math.Ceiling((double)totalResult / _RegistrosPorPagina);
                // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
                _PaginadorPelicula = new Paginador<Pelicula>()
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
        //Valorar pelicula
        public IActionResult ValorarPelicula()
        {
            var calif = Request.Form["valoracion"];
            Console.Write("CALIFFFF "+calif+"\n");
            return NoContent();
        }
        public IActionResult TopMovie()
    {
        return View();

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
