using Biblio.Web.DATA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblio.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoriaDao _categoriaDao;
        public CategoryController(ICategoriaDao categoriaDao)
        {
            this._categoriaDao = categoriaDao;
        }
        // GET: CategoryController
        public async Task<IActionResult> Index()
        {
            var result = await this._categoriaDao.GetAllAsync();

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<Categoria>());

            }
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)/*(IFormCollection collection)*/
        {
            try
            {

                var result = await this._categoriaDao.AddAsync(categoria);

                if (result.IsSuccess)
                {
                    // If the operation was successful, redirect to the Index action
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // If the operation failed, add an error to the ModelState and return the view with the model
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(categoria);
                }
                //return RedirectToAction(nameof(Index)); no se si usa el result de la operacion
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
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



    }
}
