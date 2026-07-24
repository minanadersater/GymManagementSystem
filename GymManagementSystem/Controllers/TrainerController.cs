using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers
{
        [Authorize]

    public class TrainerController : Controller
    {
        private readonly ITrainerServices trainerServices;

        public TrainerController(ITrainerServices trainerServices)
        {
            this.trainerServices = trainerServices;
        }

        #region Index

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await trainerServices.GetAllTrainersAsync(ct);

            return View(trainers);
        }

        #endregion

        #region Details

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var trainer = await trainerServices
                .GetTrainerDetailsAsync(id, ct);

            if (trainer is null)
                return NotFound();

            return View(trainer);
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateTrainerViewModel model,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await trainerServices
                .CreateTrainerAsync(model, ct);

            if (!result)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Email or Phone already exists");

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id,
            CancellationToken ct)
        {
            var trainer = await trainerServices
                .GetTrainerToUpdateAsync(id, ct);

            if (trainer is null)
                return NotFound();

            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            TrainerToUpdateViewModel model,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await trainerServices
                .UpdateTrainerAsync(id, model, ct);

            if (!result)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to update trainer");

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken ct)
        {
            var trainer = await trainerServices
                .GetTrainerDetailsAsync(id, ct);

            if (trainer is null)
                return NotFound();

            return View(trainer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id,
            CancellationToken ct)
        {
            var result = await trainerServices
                .DeleteTrainerAsync(id, ct);

            if (!result)
            {
                TempData["Error"] =
                    "Trainer has future sessions and cannot be deleted.";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
