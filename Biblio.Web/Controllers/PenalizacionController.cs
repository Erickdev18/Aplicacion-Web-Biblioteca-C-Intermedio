using Biblio.Web.DATA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblio.Web.Controllers
{
    public class PenalizacionController : Controller
    {
        private readonly IPenalizacionDao _penalizacionDao;
        public PenalizacionController(IPenalizacionDao penalizacionDao)
        {
            this._penalizacionDao = penalizacionDao;
        }
        // GET: PenalizacionController
        public async Task<IActionResult> Index()
        {
            var result = await this._penalizacionDao.GetAllAsync();

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<Penalizacion>());

            }
        }

        // GET: PenalizacionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PenalizacionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PenalizacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Penalizacion penalizacion)
        {
            try
            {
                var result = await this._penalizacionDao.AddAsync(penalizacion);

                if (result.IsSuccess)
                {
                    // If the operation was successful, redirect to the Index action
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // If the operation failed, add an error to the ModelState and return the view with the model
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(penalizacion);
                }
                //return RedirectToAction(nameof(Index)); no se si usa el result de la operacion
            }
            catch
            {
                return View();
            }
        }

        // GET: PenalizacionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PenalizacionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Penalizacion penalizacion)
        {
            try
            {
                var result = await _penalizacionDao.UpdateAsync(penalizacion);

                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(penalizacion);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: PenalizacionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PenalizacionController/Delete/5
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
