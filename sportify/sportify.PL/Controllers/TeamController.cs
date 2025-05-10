using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;

namespace sportify.PL.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService, ILeagueService leagueService)
        {
            this._teamService = teamService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddTeams(int leagueId, int numberOfTeams)
        {
            ViewBag.LeagueId = leagueId;
            ViewBag.NumberOfTeams = numberOfTeams;

            var teams = new List<TeamDTO>();
            for (int i = 0; i < numberOfTeams; i++)
            {
                teams.Add(new TeamDTO());
            }

            return View(teams);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTeams(List<TeamDTO> teams, int leagueId)
        {
            foreach (var team in teams)
            {
                team.LeagueID = leagueId;
            }
            await _teamService.AddTeamsAsync(teams);

            return RedirectToAction("Index", "League");
        }
    }
}
