using Microsoft.AspNetCore.Mvc;
using MvcSegundaPracticaAGC.Models;
using MvcSegundaPracticaAGC.Repositories;

namespace MvcSegundaPracticaAGC.Controllers
{
    public class ComicController : Controller
    {
        private readonly RepositoryComic repository;

        public ComicController()
        {
            repository = new RepositoryComic();
        }
        public IActionResult Index()
        {
            List<Comics> listaComics = repository.GetComics();

            return View(listaComics);
        }

        public IActionResult Details(int id)
        {
            Comics comics = repository.FindComic(id);
            return View(comics);
        }


        
        public async Task<IActionResult> Create()
        { 
            return View();
      
        }
        [HttpPost]
        public async Task<IActionResult> Create(Comics comics)
        {

            await repository.CreateComic(comics);

            return RedirectToAction("Index");
        }

    }
}
