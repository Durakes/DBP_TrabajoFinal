using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaDBP.Models;

namespace PruebaDBP.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly bdlumiereContext Context;
        public CategoriaController(bdlumiereContext context)
        {
            Context = context;
        }
        public IActionResult Index()
        {
            //AllCategories();
            return View();
        }

        [HttpGet]
        public async Task AllCategories()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var baseAddress = new Uri("http://api.themoviedb.org/3/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                // api_key can be requestred on TMDB website
                using (var response = await httpClient.GetAsync("genre/movie/list?api_key=da8f080c81b970a5c0962ea17bfc0cda&language=es"))
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    dynamic resultado = JsonConvert.DeserializeObject(responseData);

                    foreach (var result in resultado.genres)
                    {
                        string nombre = result.name;
                        int idtmdbCat = result.id;
                        Categorium obj = new Categorium(idtmdbCat, nombre);
                        if (ModelState.IsValid)
                        {
                            Context.Categoria.Add(obj);
                            Context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
