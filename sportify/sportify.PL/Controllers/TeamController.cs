using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities.IO;
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
        private readonly IUserService _userService;
        private readonly ILeagueTeamCountUpdateService _leagueTeamCountService;
        private readonly IMatchService _matchService;
        private readonly IFileService _fileService;


        public TeamController(ITeamService teamService, ILeagueService leagueService,
            IMatchService matchService, IUserService userService, 
            ILeagueTeamCountUpdateService leagueTeamCountService, IFileService fileService)
        {
            _teamService = teamService;
            _leagueService = leagueService;
            _leagueTeamCountService = leagueTeamCountService;
            _matchService = matchService;
            _userService = userService;
            _fileService = fileService;
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
            TempData.Keep("LeagueData");

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
            if (TempData["LeagueData"] is not string leagueDataJson)
            {
                return RedirectToAction("Create", "League");
            }

            var league = JsonSerializer.Deserialize<LeagueDTO>(leagueDataJson);

            var createdLeague = await _leagueService.AddAndReturnAsync(league);

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
                return BadRequest(new
                {
                    error = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .FirstOrDefault()?.ErrorMessage
                });
            }

            var existingTeam = await _teamService.GetTeamByIdAsync(team.TeamID);

            if (existingTeam == null)
            {
                return NotFound(new { error = "Team not found" });
            }

            var league = await _leagueService.GetByIdAsync(existingTeam.LeagueID);
            if (league == null)
            {
                return NotFound(new { error = "League not found" });
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != league.OrganizerID)
            {
                return Forbid();
            }

            if (string.IsNullOrWhiteSpace(team.Name))
            {
                return BadRequest(new { error = "Team name cannot be empty" });
            }

            _teamService.ClearTracking();

            existingTeam.Name = team.Name;

            await _teamService.UpdateAsync(existingTeam);

            return Ok(new { success = true });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeamLogo(int TeamID, IFormFile ImageFile, int LeagueID)
        {
            try
            {
                var existingTeam = await _teamService.GetTeamByIdAsync(TeamID);
                if (existingTeam == null)
                {
                    return NotFound(new { success = false, message = "Team not found" });
                }

                var league = await _leagueService.GetByIdAsync(existingTeam.LeagueID);
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId != league?.OrganizerID)
                {
                    return Forbid();
                }

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingTeam.ImageUrl))
                    {
                        await _fileService.DeleteFileAsync(existingTeam.ImageUrl);
                    }

                    _teamService.ClearTracking();

                    existingTeam.ImageUrl = await _fileService.SaveFileAsync(ImageFile);
                    await _teamService.UpdateAsync(existingTeam);

                    return Ok(new
                    {
                        success = true,
                        imageUrl = existingTeam.ImageUrl
                    });
                }

                return BadRequest(new { success = false, message = "No image provided" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTeam(int teamId, int leagueId)
        {
            await _matchService.DeleteAllMatchesAsync(leagueId);
            await _teamService.DeleteAsync(teamId);

            await _leagueTeamCountService.UpdateTeamCountAsync(new LeagueTeamCountUpdateDTO
            {
                LeagueID = leagueId,
                TeamCount = (await _teamService.GetAllTeamsInLeagueAsync(leagueId)).Count()
            });

            var teams = (await _teamService.GetAllTeamsInLeagueAsync(leagueId)).ToList();

            var league = await _leagueService.GetByIdAsync(leagueId);
            if (league == null)
                return NotFound();


            var matches = MatchGenerator.GenerateMatches(league, teams);
            await _matchService.AddMatchesAsync(matches);

            await _teamService.UpdateAndSortStandingsAsync(leagueId);

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

            var teams = (await _teamService.GetAllTeamsInLeagueAsync(leagueId)).ToList();

            await _leagueTeamCountService.UpdateTeamCountAsync(new LeagueTeamCountUpdateDTO
            {
                LeagueID = leagueId,
                TeamCount = teams.Count
            });

            var league = await _leagueService.GetByIdAsync(leagueId);
            if (league == null)
                return NotFound();

            await _matchService.DeleteAllMatchesAsync(leagueId);

            var matches = MatchGenerator.GenerateMatches(league, teams);
            await _matchService.AddMatchesAsync(matches);

            await _teamService.UpdateAndSortStandingsAsync(leagueId);

            return RedirectToAction("EditTeams", new { id = leagueId });
        }
    }
}
