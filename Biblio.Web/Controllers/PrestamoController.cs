using Biblio.Web.DATA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblio.Web.Controllers
{
    public class PrestamoController : Controller
    {
        private readonly IPrestamoDao _prestamoDao;
        public PrestamoController(IPrestamoDao prestamoDao)
        {
            this._prestamoDao = prestamoDao;
        }
        // GET: PrestamoController
        public async Task<IActionResult> Index()
        {
            var result = await this._prestamoDao.GetAllAsync();

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<Prestamo>());

            }
        }

        // GET: PrestamoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PrestamoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrestamoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prestamo prestamo)/*(IFormCollection collection)*/
        {
            try
            {

                var result = await this._prestamoDao.AddAsync(prestamo);

                if (result.IsSuccess)
                {
                    // If the operation was successful, redirect to the Index action
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // If the operation failed, add an error to the ModelState and return the view with the model
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(prestamo);
                }
                //return RedirectToAction(nameof(Index)); no se si usa el result de la operacion
            }
            catch
            {
                return View();
            }
        }

        // GET: PrestamoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PrestamoController/Edit/5
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
