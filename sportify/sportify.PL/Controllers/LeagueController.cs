using Microsoft.AspNetCore.Mvc;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using System.Security.Claims;

namespace sportify.PL.Controllers
{
    public class LeagueController : Controller
    {
        private readonly ILeagueService _leagueService;

        public LeagueController(ILeagueService leagueService)
        {
            _leagueService = leagueService;
        }

        public async Task<IActionResult> Index()
        {
            var League = await _leagueService.GetAllAsync();

            return View("Index",League);
        }

        public async Task<IActionResult> Details(int id)
        {
            var League = await _leagueService.GetByIdAsync(id);
            return League == null ? NotFound() : View(League);
        }


        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeagueDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.OrganizerID = userId;
            ModelState.Remove("OrganizerID");
            if (ModelState.IsValid)
            {
                var createdLeague = await _leagueService.AddAndReturnAsync(model);
                return RedirectToAction("AddTeams", "Team", new{ leagueId = createdLeague.LeagueID, numberOfTeams = createdLeague.NumberOfTeams });
            }
            return View(model);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var League = await _leagueService.GetByIdAsync(id);
            return League == null ? NotFound() : View(League);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LeagueDTO model)
        {
            if (ModelState.IsValid)
            {
                await _leagueService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var League = await _leagueService.GetByIdAsync(id);

            return League == null ? NotFound() : View(League);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _leagueService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

