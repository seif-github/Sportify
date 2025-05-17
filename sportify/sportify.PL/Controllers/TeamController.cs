using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sportify.BLL.DTOs;
using sportify.BLL.Helpers;
using sportify.BLL.Services;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Entities;
using sportify.PL.ViewModels;

namespace sportify.PL.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly ILeagueService _leagueService;
        private readonly ILeagueTeamCountUpdateService _leagueTeamCountService;
        private readonly IMatchService _matchService;

        public TeamController(ITeamService teamService, ILeagueService leagueService,
            IMatchService matchService, ILeagueTeamCountUpdateService leagueTeamCountService)
        {
            this._teamService = teamService;
            this._leagueService = leagueService;
            this._leagueTeamCountService = leagueTeamCountService;
            this._matchService = matchService;
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
            var createdTeams = await _teamService.AddTeamsAndReturnAsync(teams);
            var matches = MatchGenerator.GenerateMatches(createdLeague, createdTeams);

            await _matchService.AddMatchesAsync(matches);

            TempData.Remove("LeagueData");
            return RedirectToAction("Details", "League", new { id = createdLeague.LeagueID });
        }

        [HttpGet]
        public async Task<IActionResult> EditTeams(int leagueId)
        {
            var teams = await _teamService.GetAllTeamsInLeagueAsync(leagueId);
            var league = await _leagueService.GetByIdAsync(leagueId);
            var viewModel = new LeagueDetailsViewModel
            {
                League = league,
                Teams = teams,
                NewTeamName = ""
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeams(LeagueDetailsViewModel model)
        {
            // Only update team names
            foreach (var team in model.Teams)
            {
                await _teamService.UpdateAsync(team);
            }

            var league = await _leagueService.GetByIdAsync(model.League.LeagueID);
            var teams = await _teamService.GetAllTeamsInLeagueAsync(model.League.LeagueID);

            var viewModel = new LeagueDetailsViewModel
            {
                League = league,
                Teams = teams,
                NewTeamName = ""
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTeam(int teamId, int leagueId)
        {
            await _teamService.DeleteAsync(teamId);

            await _leagueTeamCountService.UpdateTeamCountAsync(new LeagueTeamCountUpdateDTO
            {
                LeagueID = leagueId,
                TeamCount = (await _teamService.GetAllTeamsInLeagueAsync(leagueId)).Count()
            });

            return RedirectToAction("EditTeams", new { leagueId = leagueId });
        }

    }
}
