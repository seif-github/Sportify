using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using sportify.BLL.DTOs;
using sportify.BLL.Helpers;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Entities;
using sportify.PL.Helpers;
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
        private readonly IMatchService _matchService;
        private readonly IUserService _userService;

        public LeagueController(ILeagueService leagueService, ITeamService teamService,
                IUserService userService, IMatchService matchService)
        {
            _leagueService = leagueService;
            _teamService = teamService;
            _matchService = matchService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> BrowseLeagues(string sortBy = "date", bool ascending = true)
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

            viewModel = sortBy.ToLower() switch
            {
                "name" => LeagueViewModelSorter.SortLeaguesByName(viewModel, ascending),
                "teams" => LeagueViewModelSorter.SortLeaguesByTeamCount(viewModel, ascending),
                _ => LeagueViewModelSorter.SortLeaguesByDate(viewModel, ascending)
            };

            ViewBag.SortBy = sortBy;
            ViewBag.Ascending = ascending;

            return View(viewModel);
        }

        //[AllowAnonymous]
        //public async Task<IActionResult> Standings(int leagueId)
        //{
        //    var teamsSorted = await _teamService.UpdateAndSortStandingsAsync(leagueId);
        //    return View(teamsSorted);
        //}

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id) //league ID
        {
            var league = await _leagueService.GetByIdAsync(id); if (league == null) return NotFound();
            var teams = await _teamService.GetAllTeamsInLeagueAsync(id);
            var teamsSorted = await _teamService.UpdateAndSortStandingsAsync(id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isOrganizer = currentUserId != null && currentUserId == league.OrganizerID;

            var organizerName = "Unknown Organizer";
            if (!string.IsNullOrEmpty(league.OrganizerID))
            {
                var organizer = await _userService.GetUserById(league.OrganizerID);
                organizerName = organizer?.UserName ?? "Unknown Organizer";
            }

            var viewModel = new LeagueDetailsViewModel
            {
                League = league,
                Teams = teamsSorted,
                IsOrganizer = isOrganizer,
                OrganizerName = organizerName
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
            var league = await _leagueService.GetByIdAsync(id); if (league == null) return NotFound();
            if (DateTime.Now >= league.StartDate) return BadRequest();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != league.OrganizerID) return Forbid();
            return View(league);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LeagueDTO model)
        {
            // Skip getting the league again since we'll be completely replacing it
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Verify permission by getting just the organizer ID
            var organizerId = await _leagueService.GetOrganizerIdByLeagueId(model.LeagueID);
            if (organizerId == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != organizerId) return Forbid();

            // Preserve the original OrganizerID to prevent hijacking
            model.OrganizerID = organizerId;

            // Update the entity
            await _leagueService.UpdateAsync(model);

            var teams = (await _teamService.GetAllTeamsInLeagueAsync(model.LeagueID)).ToList();
            await _matchService.DeleteAllMatchesAsync(model.LeagueID);
            var matches = MatchGenerator.GenerateMatches(model, teams);
            await _matchService.AddMatchesAsync(matches);

            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var league = await _leagueService.GetByIdAsync(id);
            if (league == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(currentUserId != league.OrganizerID) return Forbid();

            return View(league);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] int id)
        {
            var league = await _leagueService.GetByIdAsync(id); if (league == null) return NotFound();

            var organizerId = await _leagueService.GetOrganizerIdByLeagueId(id);
            if (organizerId != league.OrganizerID) return Forbid();

            await _matchService.DeleteAllMatchesAsync(id);

            // Get all teams in this league
            var teams = await _teamService.GetAllTeamsInLeagueAsync(id);

            // Delete all teams first
            foreach (var team in teams)
            {
                await _teamService.DeleteAsync(team.TeamID);
            }

            await _leagueService.DeleteAsync(id);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}

