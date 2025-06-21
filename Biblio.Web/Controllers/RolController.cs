using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biblio.Web.DATA;
using System.Threading.Tasks;

namespace Biblio.Web.Controllers
{
    public class RolController : Controller
    {
        private readonly IRolDao _rolDao;
        public RolController(IRolDao rolDao)
        {
            this._rolDao = rolDao;
        }
        // GET: RolController
        public async Task<ActionResult> Index()
        {
            var result = await this._rolDao.GetAllAsync();

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<Rol>());
            }
        }

        // GET: RolController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RolController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rol rol)
        {
            try
            {
                var result = await this._rolDao.AddAsync(rol);

                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(rol);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: RolController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RolController/Edit/5
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

        // GET: RolController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RolController/Delete/5
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
