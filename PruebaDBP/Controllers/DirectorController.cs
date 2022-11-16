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
        public IActionResult Index(int id)
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

                /*objDirector.listPeliculas= Lista de peliculas del director*/

                return View(objDirector);
                
            }

            
        }
        
        
    }
}