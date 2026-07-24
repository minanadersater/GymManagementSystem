using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnalyticsService analyticsService;

        public HomeController(ILogger<HomeController> logger,IAnalyticsService analyticsService)
        {
            _logger = logger;
            this.analyticsService = analyticsService;
        }

        public async Task<IActionResult> Index(CancellationToken ct )
        {
            var Data = await analyticsService.GetAnalyticsDataAsync(ct);
            return View(Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
             return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
