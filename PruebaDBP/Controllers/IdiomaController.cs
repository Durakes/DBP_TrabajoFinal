using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PruebaDBP.Controllers
{
    public class IdiomaController : Controller
    {
        private readonly bdlumiereContext Context;
        public IdiomaController(bdlumiereContext context)
        {
            Context = context;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task AllLanguages()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var baseAddress = new Uri("http://api.themoviedb.org/3/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                // api_key can be requestred on TMDB website
                using (var response = await httpClient.GetAsync("configuration/languages?api_key=da8f080c81b970a5c0962ea17bfc0cda"))
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    dynamic resultado = JsonConvert.DeserializeObject(responseData);
                    foreach (var result in resultado)
                    {
                        string abreviacion = result.iso_639_1;
                        string nombre = result.english_name;
                        Idioma obj = new Idioma(nombre, abreviacion);
                        if (ModelState.IsValid)
                        {
                            Context.Idiomas.Add(obj);
                            Context.SaveChanges();
                        }
                        
                    }
                }
            }
        }
    }
}
