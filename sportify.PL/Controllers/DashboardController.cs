using Microsoft.AspNetCore.Mvc;
using sportify.BLL.Services.Contracts;
using System.Security.Claims;
using sportify.BLL.Services;

namespace sportify.PL.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Forbid();
            var dashboardData = await _dashboardService.GetDashboardDataAsync(userId);
            return View(dashboardData);
        }
    }
}
