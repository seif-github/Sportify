using Microsoft.AspNetCore.Mvc;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;

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
            var tournaments = await _leagueService.GetAllAsync();

            return View(tournaments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tournament = await _leagueService.GetByIdAsync(id);
            return tournament == null ? NotFound() : View(tournament);
        }

        // GET: /Tournament/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Tournament/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeagueDTO model)
        {
            if (ModelState.IsValid)
            {
                await _leagueService.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: /Tournament/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tournament = await _leagueService.GetByIdAsync(id);
            return tournament == null ? NotFound() : View(tournament);
        }

        // POST: /Tournament/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeagueDTO model)
        {
            if (id != model.LeagueID) return BadRequest();

            if (ModelState.IsValid)
            {
                await _leagueService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: /Tournament/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tournament = await _leagueService.GetByIdAsync(id);

            return tournament == null ? NotFound() : View(tournament);
        }

        // POST: /Tournament/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _leagueService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

