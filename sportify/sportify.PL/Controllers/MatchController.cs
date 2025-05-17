using Microsoft.AspNetCore.Mvc;
using sportify.BLL.Services.Contracts;

namespace sportify.PL.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchService _matchService;
        public MatchController(IMatchService matchService)
        {
            this._matchService = matchService;
        }

        [HttpGet]
        public async Task<IActionResult> LeagueMatches(int id) // id -> leagueId
        {
            var matches = await _matchService.GetMatchesByLeagueIdAsync(id);
            return View(matches);
        }
    }
}
