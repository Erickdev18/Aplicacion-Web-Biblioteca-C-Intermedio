using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biblio.Web.DATA;
using System.Threading.Tasks;

namespace Biblio.Web.Controllers
{
    public class EstadoPrestamoController : Controller
    {
        private readonly IEstadoPrestamo _EstadoPrestamoDao;
        public EstadoPrestamoController(IEstadoPrestamo estadoPrestamoDao)
        {
            this._EstadoPrestamoDao = estadoPrestamoDao;
        }

        // GET: EstadoPrestamoController
        public async Task<IActionResult> Index()
        {
            var result = await this._EstadoPrestamoDao.GetAllAsync();

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<EstadoPrestamo>());
            }

        }

        // GET: EstadoPrestamoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EstadoPrestamoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstadoPrestamoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstadoPrestamo estadoPrestamo)
        {
            try
            {
                var result = await this._EstadoPrestamoDao.AddAsync(estadoPrestamo);

                if(result.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(estadoPrestamo);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: EstadoPrestamoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EstadoPrestamoController/Edit/5
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

        // GET: EstadoPrestamoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EstadoPrestamoController/Delete/5
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
