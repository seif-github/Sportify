using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sportify.BLL.DTOs;
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
        private readonly IUserService _userService;
        private readonly ILeagueTeamCountUpdateService _leagueTeamCountService;

        public TeamController(ITeamService teamService, ILeagueService leagueService,
            ILeagueTeamCountUpdateService leagueTeamCountService, IUserService userService)
        {
            _teamService = teamService;
            _leagueService = leagueService;
            _leagueTeamCountService = leagueTeamCountService;
            _userService = userService;
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> EditTeams(int id) //leagueID
        {
            var league = await _leagueService.GetByIdAsync(id); if(league == null) return NotFound();
            var teams = await _teamService.GetAllTeamsInLeagueAsync(id);
            var organizerName = "Unknown Organizer";
            var isOrganizer = false;
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(league.OrganizerID))
            {
                var organizer = await _userService.GetUserById(league.OrganizerID);
                organizerName = organizer?.UserName ?? "Unknown Organizer";
                isOrganizer = currentUserId != null && currentUserId == league.OrganizerID;
            }

            var viewModel = new LeagueDetailsViewModel
            {
                League = league,
                Teams = teams,
                NewTeamName = "",
                OrganizerName = organizerName,
                IsOrganizer = isOrganizer
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
        public async Task<IActionResult> EditTeam([FromBody] TeamDTO team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verify that the team exists and the user has permission to edit it
            var existingTeam = await _teamService.GetTeamByIdAsync(team.TeamID);
            if (existingTeam == null)
            {
                return NotFound();
            }

            // Check if the user is the organizer of the league this team belongs to
            var league = await _leagueService.GetByIdAsync(existingTeam.LeagueID);
            if (league == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != league.OrganizerID)
            {
                return Forbid();
            }

            // Only update the name, preserving other properties
            existingTeam.Name = team.Name;

            await _teamService.UpdateAsync(existingTeam);

            return Ok(new { success = true });
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

            return RedirectToAction("EditTeams", new { id = leagueId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTeam(int leagueId, string teamName)
        {
            if (string.IsNullOrWhiteSpace(teamName))
            {
                TempData["Error"] = "Team name cannot be empty";
                return RedirectToAction("EditTeams", new { leagueId });
            }

            var newTeam = new TeamDTO
            {
                LeagueID = leagueId,
                Name = teamName,
                Wins = 0,
                Losses = 0,
                Draws = 0,
                Points = 0,
                TotalMatchesPlayed = 0
            };
            await _teamService.AddTeamAsync(newTeam);
            await _leagueTeamCountService.UpdateTeamCountAsync(new LeagueTeamCountUpdateDTO
            {
                LeagueID = leagueId,
                TeamCount = (await _teamService.GetAllTeamsInLeagueAsync(leagueId)).Count()
            });
            return RedirectToAction("EditTeams", new { id = leagueId });
        }
    }
}
