using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.PL.ViewModels;
using System.Security.Claims;
using System.Text.Json;

namespace sportify.PL.Controllers
{
    [Authorize]
    public class LeagueController : Controller
    {
        private readonly ILeagueService _leagueService;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;

        public LeagueController(ILeagueService leagueService, ITeamService teamService,
                IUserService userService)
        {
            _leagueService = leagueService;
            _teamService = teamService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var organizerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var leagues = await _leagueService.GetOrganizerLeaguesById(organizerId);
            return View(leagues);
        }

        [AllowAnonymous]
        public async Task<IActionResult> BrowseLeagues()
        {
            var leagues = await _leagueService.GetAllAsync();
            var organizerIds = leagues.Select(l => l.OrganizerID).Distinct().ToList();
            var organizers = await _userService.GetUsersByIds(organizerIds);
            var organizerDictionary = organizers.ToDictionary(u => u.Id, u => u.UserName);

            var viewModel = leagues.Select(l => new LeagueWithOrganizerViewModel
            {
                League = l,
                OrganizerName = organizerDictionary.TryGetValue(l.OrganizerID, out var name) ? name : "Unknown"
            }).ToList();

            return View(viewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Standings(int leagueId)
        {
            List<TeamDTO> teamsSorted = await _teamService.SortStandings(leagueId);
            return Ok(teamsSorted);
        }

        [AllowAnonymous]
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

