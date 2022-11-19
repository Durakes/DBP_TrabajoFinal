using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PruebaDBP.Controllers
{
    public class DirectorController : Controller
    {
        private readonly bdlumiereContext Context;
        public DirectorController(bdlumiereContext context)
        {
            Context = context;
        }
        public async Task<IActionResult> Index(int id)
        {
            var objDir = (from dir in Context.Directors where dir.IdDirTmdb == id select dir).FirstOrDefault();

            if(objDir == null)
            {
                return NoContent();
            }
            else
            {
                IndexDirector objDirector = new IndexDirector();
                objDirector.director = objDir;
                //Lista de peliculas
                List<Pelicula> listPeliculas = new List<Pelicula>();
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                var baseAddress = new Uri("http://api.themoviedb.org/3/");

                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                    using (var response = await httpClient.GetAsync("person/"+ id.ToString() + "/movie_credits?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es"))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        dynamic resultado = JsonConvert.DeserializeObject(responseData);
                        foreach (var objPelicula in resultado.crew)
                        {
                            if(objPelicula.job == "Director")
                            {
                                int idPel = objPelicula.id;

                                var peliculaBD = (from peli in Context.Peliculas where peli.IdTmdb == idPel select peli).FirstOrDefault();

                                if(peliculaBD != null)
                                {
                                    listPeliculas.Add(peliculaBD);
                                }else
                                {
                                    using(var responsePel = await httpClient.GetAsync("movie/" + idPel.ToString() + "?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es"))
                                    {
                                        string responsePelData = await responsePel.Content.ReadAsStringAsync();
                                        dynamic resultPel = JsonConvert.DeserializeObject(responsePelData);
                                        string nombre = resultPel.title;
                                        string fechaEstreno = resultPel.release_date;
                                        string lenguaje = objPelicula.original_language;
                                        int? valoracion = resultPel.vote_average;
                                        string sumilla = resultPel.overview;
                                        string fotoURL = "https://image.tmdb.org/t/p/w500" + resultPel.poster_path;

                                        var ObjIdioma = (from Tleng in Context.Idiomas where Tleng.Abreviacion == lenguaje select Tleng).Single();
                                        Pelicula objPeliculaRegistro = new Pelicula(idPel, ObjIdioma.IdIdioma, nombre, fechaEstreno, valoracion, sumilla, fotoURL);

                                        listPeliculas.Add(objPeliculaRegistro);
                                    }
                                }
                            }
                        }
                        
                    }
                }
                objDirector.listPeliculas = listPeliculas;
                return View(objDirector);
                
            }

            
        }
        
        
    }
}