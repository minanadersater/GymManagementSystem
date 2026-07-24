using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.Controllers
{
    [Authorize]

    public class SessionController : Controller
    {
        private readonly ISessionServices sessionServices;

        public SessionController(ISessionServices sessionServices)
        {
            this.sessionServices = sessionServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Sessions = await sessionServices.GetAllSessionsAsync(ct);
            return View(Sessions);

        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownsAsync(ct);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownsAsync(ct);
                return View(model);
            }
            var Result = await sessionServices.CreateSessionAsync(model, ct);
            if(Result.Success)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] =Result.Error;
            await PopulateDropDownsAsync(ct);
            return View(model);

        }

        private async Task PopulateDropDownsAsync(CancellationToken ct)
        {
            ViewBag.Trainers =new SelectList( await sessionServices.GetTrainaerForDropDownAsync(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await sessionServices.GetCategoriesForDropDownAsync(ct), "Id", "CategoryName" );
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var session = await sessionServices.GetSessionByIdAsync(id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
     
            return View(session);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var session = await sessionServices.GetSessionToUpdateAsync(id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session cannot be updated. ";
                return RedirectToAction("Index");
            }
            await PopulateDropDownsAsync(ct);
            return View(session);
        }
        [HttpPost ]
        public async Task<IActionResult> Edit(int id,UpdateSessionViewModel model, CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                await PopulateDropDownsAsync(ct);
                return View(model);
            }
            var Result = await sessionServices.UpdateSessionAsync(id, model, ct);
            if (Result.Success)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = Result.Error;
            await PopulateDropDownsAsync(ct);
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var session = await sessionServices.GetSessionByIdAsync(id, ct);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {

            var Result = await sessionServices.DeleteSessionAsync(id, ct);
            TempData[Result.Success ? "SuccessMessage" : "ErrorMessage"] = Result.Success ? "Session deleted successfully." : Result.Error;
            return RedirectToAction(nameof(Index));


        }
    }
}
