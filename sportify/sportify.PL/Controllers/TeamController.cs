using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using sportify.BLL.DTOs;
using sportify.BLL.Services;
using sportify.BLL.Services.Contracts;

namespace sportify.PL.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly ILeagueService _leagueService;

        public TeamController(ITeamService teamService, ILeagueService leagueService)
        {
            this._teamService = teamService;
            this._leagueService = leagueService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddTeams(int leagueId, int numberOfTeams)
        {
            if (TempData["LeagueData"] is not string leagueData)
            {
                return RedirectToAction("Create", "League");
            }

            var model = JsonSerializer.Deserialize<LeagueDTO>(leagueData);
            TempData.Keep("LeagueData"); // Preserve for form submission

            var teams = new List<TeamDTO>();
            for (int i = 0; i < numberOfTeams; i++)
            {
                teams.Add(new TeamDTO
                {
                    Name = $"Team {i + 1}"
                });
            }

            ViewBag.LeagueData = model;
            return View(teams);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTeams(List<TeamDTO> teams, LeagueDTO leagueModel)
        {
            // Retrieve league data from TempData
            if (TempData["LeagueData"] is not string leagueDataJson)
            {
                return RedirectToAction("Create", "League");
            }

            var league = JsonSerializer.Deserialize<LeagueDTO>(leagueDataJson);

            // 1. First create the league
            var createdLeague = await _leagueService.AddAndReturnAsync(league);

            // 2. Then add teams with their league ID
            foreach (var team in teams)
            {
                team.LeagueID = createdLeague.LeagueID;
                if (string.IsNullOrWhiteSpace(team.Name))
                {
                    var index = teams.IndexOf(team);
                    team.Name = $"Team {index + 1}";
                }
            }

            await _teamService.AddTeamsAsync(teams);
            TempData.Remove("LeagueData");
            return RedirectToAction("Details", "League", new { id = createdLeague.LeagueID });
        }
    }
}
