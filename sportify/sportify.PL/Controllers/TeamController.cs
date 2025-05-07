using Microsoft.AspNetCore.Mvc;
using sportify.BLL.Services;
using sportify.BLL.CustomModels;
using sportify.BLL.Services.Contracts;

namespace sportify.PL.Controllers
{

    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _teamService.GetAllAsync();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TeamModel newTeam)
        {
            if (ModelState.IsValid)
            {
                await _teamService.AddAsync(newTeam);
                return RedirectToAction("Index");
            }
            return View(newTeam);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var team = await _teamService.GetByIdAsync(Id);

            return team==null? NotFound(): View(team);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(int Id)
        {
            await _teamService.DeleteAsync(Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var team = await _teamService.GetByIdAsync(id);
            return team == null ? NotFound() : View(team);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamModel model)
        {
            if (ModelState.IsValid)
            {
                await _teamService.UpdateAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var team = await _teamService.GetByIdAsync(id);
            return team == null ? NotFound() : View(team);
        }
    }
}
