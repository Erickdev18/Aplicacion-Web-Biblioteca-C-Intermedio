using Biblio.Web.DATA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblio.Web.Controllers
{
    public class LibroController : Controller
    {
        private readonly ILibrosDao _librosDao;
        public LibroController(ILibrosDao librosDao)
        {
            this._librosDao = librosDao;
        }
        public async Task<IActionResult> Index()
        {
            var result = await this._librosDao.GetAllAsync();

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<Libros>());

            }
        }

        // GET: LibroController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LibroController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibroController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libros libros)
        {
            try
            {
                var result = await this._librosDao.AddAsync(libros);
                if (result.IsSuccess)
                {
                    // If the operation was successful, redirect to the Index action
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // If the operation failed, add an error to the ModelState and return the view with the model
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(libros);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: LibroController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LibroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LibroController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LibroController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
