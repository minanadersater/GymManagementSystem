using GymManagementSystem.DAL.Context;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Controllers
{
    public class PlanController : Controller
    {

        private readonly IPlanRepository planRepository;
        public PlanController(IPlanRepository _planRepository) 
        {
            planRepository = _planRepository;

        }
        public async Task<IActionResult> Index()
        {
            var plans = await planRepository.GetAll();
            return View(plans);
        }
        public async Task<IActionResult> Details(int id)
        {
            var plan = await planRepository.GetById(id);
            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        //private readonly GymDbcontext dbContext = new GymDbcontext();



        //public async Task<IActionResult> Index()
        //{
        //    var plans = await dbContext.Plans.ToListAsync();
        //    return View(plans);
        //}


        //public async Task<IActionResult> Index2()
        //{
        //    var plans = await dbContext.Plans.ToListAsync();
        //    return View(plans);
        //}
        //public async Task<IActionResult> Details(int id)
        //{
        //    var plan = await _dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);
        //    if (plan == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(plan);
        //}

        //public async Task<IActionResult> Details(int id)
        //{
        //    var plan = await dbContext.Plans
        //        .FirstOrDefaultAsync(p => p.Id == id);

        //    if (plan == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(plan);
        //}
    }
}
