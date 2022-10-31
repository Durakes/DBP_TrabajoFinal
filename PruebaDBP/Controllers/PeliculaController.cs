using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaDBP.Models;
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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Resultados()
        {
            return View();
        }
        public IActionResult TopMovie()
        {
            return View();

        }

        public async Task SearchMovie(string nombreBuscar)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var baseAddress = new Uri("http://api.themoviedb.org/3/");

            using(var httpClient = new HttpClient { BaseAddress = baseAddress})
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                int page = 1;
                int totalpages = 1;
                string nombre = "https://api.themoviedb.org/3/search/movie?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es&query=Avengers&page=1&include_adult=false"
                while (page <= totalpages)
                {
                    using (var busqueda = await httpClient.GetAsync("search/movie?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es&query=" + nombreBuscar + "&page=" + page.ToString() + "&include_adult=false"))
                    {
                        string resultadosBusqueda = await busqueda.Content.ReadAsStringAsync();

                        dynamic busquedaDatos = JsonConvert.DeserializeObject(resultadosBusqueda);
                        totalpages = busquedaDatos.total_pages;
                        page = busquedaDatos.page;
                    }
                }
            }
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
                int? minutos;
                string lenguaje;
                List<String> generos = new List<string>();
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
                                minutos = detallePelicula.runtime;
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

                                objPeliculaRegistro = new Pelicula(idPelicula, ObjIdioma.IdIdioma, titulo, fecha, minutos, sumilla, posterUrl);
                                if (ModelState.IsValid)
                                {
                                    Context.Peliculas.Add(objPeliculaRegistro);
                                    Context.SaveChanges();

                                    Pelicula lastPelicula = Context.Peliculas.OrderByDescending(p => p.IdPelicula).FirstOrDefault();
                                    foreach(String genero in generos)
                                    {
                                        var ObjCategoria = (from TCat in Context.Categoria where TCat.NomCategoria == genero select TCat).Single();
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
