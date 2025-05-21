using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using sportify.BLL.Services;
using sportify.BLL.Services.Contracts;
using sportify.PL.Hubs;
using sportify.PL.ViewModels;
using System.Security.Claims;


namespace sportify.PL.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly ITeamService _teamService;
        private readonly ILeagueService _leagueService;
        private readonly IUserService _userService;
        private readonly IHubContext<ScoreHub> _hubContext;

        public MatchController(IMatchService matchService, ITeamService teamService, ILeagueService leagueService, IUserService userService, IHubContext<ScoreHub> hubContext)
        {
            _matchService = matchService;
            _teamService = teamService;
            _leagueService = leagueService;
            _userService = userService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> LeagueMatches(int id) // id -> leagueId
        {
            var league = await _leagueService.GetByIdAsync(id); if (league == null) return NotFound();
            var matches = await _matchService.GetMatchesByLeagueIdAsync(id);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var organizerName = "Unknown Organizer";
            var isOrganizer = false;

            if (!string.IsNullOrEmpty(league.OrganizerID))
            {
                var organizer = await _userService.GetUserById(league.OrganizerID);
                organizerName = organizer?.UserName ?? "Unknown Organizer";
                isOrganizer = currentUserId != null && currentUserId == league.OrganizerID;
            }

            var matchesByRound = matches
                .OrderBy(m => m.Date)
                .Select((m, index) => new { Match = m, Round = index / (league.NumberOfTeams / 2) + 1 })
                .GroupBy(m => m.Round)
                .ToDictionary(g => g.Key, g => g.Select(m => m.Match).ToList());

            int currentRound = 1;
            foreach (var round in matchesByRound.OrderBy(r => r.Key))
            {
                if (round.Value.Any(m => !m.IsCompleted))
                {
                    currentRound = round.Key;
                    break;
                }
                currentRound = round.Key + 1;
            }

            var viewModel = new LeagueDetailsViewModel
            {
                League = league,
                MatchesByRound = matchesByRound,
                IsOrganizer = isOrganizer,
                OrganizerName = organizerName
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMatchScore([FromBody] UpdateMatchScoreViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid input data." });
                }

                var league = await _leagueService.GetByIdAsync(model.LeagueId);
                if (league == null)
                {
                    return Json(new { success = false, message = "League not found." });
                }

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId != league.OrganizerID)
                {
                    return Json(new { success = false, message = "Unauthorized." });
                }

                // Verify the match is in the current round
                var matches = await _matchService.GetMatchesByLeagueIdAsync(model.LeagueId);
                var matchesByRound = matches
                    .OrderBy(m => m.Date)
                    .Select((m, index) => new { Match = m, Round = index / (league.NumberOfTeams / 2) + 1 })
                    .GroupBy(m => m.Round)
                    .ToDictionary(g => g.Key, g => g.Select(m => m.Match).ToList());

                int currentRound = 1;
                foreach (var round in matchesByRound.OrderBy(r => r.Key))
                {
                    if (round.Value.Any(m => !m.IsCompleted))
                    {
                        currentRound = round.Key;
                        break;
                    }
                    currentRound = round.Key + 1;
                }

                var match = matches.FirstOrDefault(m => m.MatchID == model.MatchId);
                if (match == null)
                {
                    return Json(new { success = false, message = "Match not found." });
                }

                var matchRound = matches
                    .OrderBy(m => m.Date)
                    .Select((m, index) => new { Match = m, Round = index / (league.NumberOfTeams / 2) + 1 })
                    .FirstOrDefault(m => m.Match.MatchID == model.MatchId)?.Round;

                if (matchRound != currentRound)
                {
                    return Json(new { success = false, message = "Scores can only be entered for the current round." });
                }

                await _matchService.UpdateMatchResultAsync(model.MatchId, model.FirstTeamGoals, model.SecondTeamGoals);

                var updatedMatch = await _matchService.GetMatchByIdAsync(model.MatchId);

                // Broadcast the update to all clients watching this league
                await _hubContext.Clients.Group($"league-{model.LeagueId}")
                    .SendAsync("ReceiveScoreUpdate", new
                    {
                        matchId = model.MatchId,
                        firstTeamGoals = model.FirstTeamGoals,
                        secondTeamGoals = model.SecondTeamGoals,
                        isCompleted = true,
                        firstTeamName = updatedMatch.FirstTeamName,
                        secondTeamName = updatedMatch.SecondTeamName,
                        date = updatedMatch.Date.ToString("MMM dd, yyyy")
                    });

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error updating match score: {ex.Message}");
                return Json(new { success = false, message = $"Error updating match: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMatchTime([FromBody] UpdateMatchTimeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid time data" });
                }

                var match = await _matchService.GetMatchByIdAsync(model.MatchId);
                if (match == null)
                {
                    return Json(new { success = false, message = "Match not found" });
                }

                // Update just the time portion
                match.Date = new DateTime(
                    match.Date.Year,
                    match.Date.Month,
                    match.Date.Day,
                    model.Hour,
                    model.Minute,
                    0
                );
                _matchService.ClearTracking();

                await _matchService.UpdateAsync(match);

                //var hubContext = HttpContext.RequestServices.GetRequiredService<IHubContext<ScoreHub>>();
                //await hubContext.Clients.Group($"League_{model.LeagueId}").SendAsync("ReceiveTimeUpdate", new
                //{
                //    matchId = model.MatchId,
                //    hour = model.Hour,
                //    minute = model.Minute
                //});


                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
