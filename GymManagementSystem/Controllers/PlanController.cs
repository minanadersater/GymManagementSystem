using GymManagementSystem.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly GymDbcontext dbContext = new GymDbcontext();

        //private readonly GymDbcontext _dbContext;
                

        public async Task<IActionResult> Index()
        {
            var plans = await dbContext.Plans.ToListAsync();
            return View(plans);
        }


        public async Task<IActionResult> Index2()
        {
            var plans = await dbContext.Plans.ToListAsync();
            return View(plans);
        }
        //public async Task<IActionResult> Details(int id)
        //{
        //    var plan = await _dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);
        //    if (plan == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(plan);
        //}

        public async Task<IActionResult> Details(int id)
        {
            var plan = await dbContext.Plans
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }
    }
}
