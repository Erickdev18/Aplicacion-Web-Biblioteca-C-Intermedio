using Biblio.Web.DATA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblio.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioDao _usuarioDao;
        public UsuarioController(IUsuarioDao usuarioDao)
        {
            this._usuarioDao = usuarioDao;
        }
        // GET: UsuarioController
        public async Task<IActionResult> Index()
        {
            var result = await this._usuarioDao.GetAllAsync();

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<Usuario>());

            }
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Usuario usuario)
        {
            try
            {

                var result = await this._usuarioDao.AddAsync(usuario);

                if (result.IsSuccess)
                {
                    // If the operation was successful, redirect to the Index action
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // If the operation failed, add an error to the ModelState and return the view with the model
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(usuario);
                }
                //return RedirectToAction(nameof(Index)); no se si usa el result de la operacion
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Usuario usuario)
        {

            try
            {
                var result = await _usuarioDao.UpdateAsync(usuario);

                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(usuario);
                }
            }
            catch
            {
                return View(usuario);
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
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
