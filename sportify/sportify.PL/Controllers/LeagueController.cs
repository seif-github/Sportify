using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.PL.ViewModels;
using System.Security.Claims;
using System.Text.Json;

namespace sportify.PL.Controllers
{
    public class LeagueController : Controller
    {
        private readonly ILeagueService _leagueService;
        private readonly ITeamService _teamService;
        private readonly IAccountService _accountService;

        public LeagueController(ILeagueService leagueService, ITeamService teamService,
                IAccountService accountService)
        {
            _leagueService = leagueService;
            _teamService = teamService;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var League = await _leagueService.GetAllAsync();
            return View(League);
        }

        public async Task<IActionResult> Standings(int leagueId)
        {
            List<TeamDTO> teamsSorted = await _teamService.SortStandings(leagueId);
            return Ok(teamsSorted);
        }

        public async Task<IActionResult> Details(int id)
        {
            var league = await _leagueService.GetByIdAsync(id);
            var teams = await _teamService.GetAllTeamsInLeagueAsync(id);

                var viewModel = new LeagueDetailsViewModel
                {
                    League = league,
                    Teams = teams
                };
                return View(viewModel);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizeCreation(LeagueDTO model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.OrganizerID = userId;

                // Now actually create the league
                var createdLeague = await _leagueService.AddAndReturnAsync(model);
                TempData.Remove("LeagueData"); // Clean up

                return RedirectToAction("Details", new { id = createdLeague.LeagueID });
            }

            // If validation fails, return to teams page
            return RedirectToAction("AddTeams", "Team", new
            {
                numberOfTeams = model.NumberOfTeams
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var league = await _leagueService.GetByIdAsync(id);
            return league == null ? NotFound() : View(league);
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

