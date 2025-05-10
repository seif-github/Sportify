using Microsoft.AspNetCore.Mvc;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using System.Security.Claims;
using System.Text.Json;

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

        [HttpGet]
        public IActionResult Create()
        {
            // If coming back from teams page, use TempData
            if (TempData["LeagueData"] is string leagueData)
            {
                var model = JsonSerializer.Deserialize<LeagueDTO>(leagueData);
                TempData.Keep("LeagueData"); // Keep for another request
                return View(model);
            }

            // Otherwise show fresh form
            return View(new LeagueDTO
            {
                StartDate = DateTime.Today,
                DurationBetweenMatches = 7,
                NumberOfTeams = 8
            });
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
                TempData["LeagueData"] = JsonSerializer.Serialize(model);
                //var createdLeague = await _leagueService.AddAndReturnAsync(model);
                return RedirectToAction("AddTeams", "Team", new{
                    numberOfTeams = model.NumberOfTeams
                });
            }
            return View(model);
        }

        //[HttpGet]
        //public IActionResult CreateFromTeamSetup()
        //{
        //    if (TempData["LeagueData"] is string leagueData)
        //    {
        //        var model = JsonSerializer.Deserialize<LeagueDTO>(leagueData);
        //        TempData.Keep("LeagueData"); // Keep the data for additional requests
        //        return View("Create", model);
        //    }
        //    return RedirectToAction("Create");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> FinalizeCreation(LeagueDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        model.OrganizerID = userId;

        //        // Now actually create the league
        //        var createdLeague = await _leagueService.AddAndReturnAsync(model);
        //        TempData.Remove("LeagueData"); // Clean up

        //        return RedirectToAction("Details", new { id = createdLeague.LeagueID });
        //    }

        //    // If validation fails, return to teams page
        //    return RedirectToAction("AddTeams", "Team", new
        //    {
        //        numberOfTeams = model.NumberOfTeams
        //    });
        //}

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

