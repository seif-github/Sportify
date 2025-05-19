using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Repositories.Contracts;
using System.Threading.Tasks;

namespace sportify.BLL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMapper _mapper;

        public DashboardService(IDashboardRepository dashboardRepository, IMapper mapper)
        {
            _dashboardRepository = dashboardRepository;
            _mapper = mapper;
        }

        public async Task<DashboardDTO> GetDashboardDataAsync(string userId)
        {
            // Await each operation sequentially
            var totalLeagues = await _dashboardRepository.GetTotalLeaguesCountAsync(userId);
            var activeTeams = await _dashboardRepository.GetActiveTeamsCountAsync(userId);
            var upcomingMatches = await _dashboardRepository.GetUpcomingMatchesCountAsync(userId);

            //var upcomingMatchesList = await GetUpcomingMatchesAsync(userId);
            //var counts = await GetCountsAsync(userId);

            var recentLeagues = await _dashboardRepository.GetRecentLeaguesAsync(userId, 5);
            var pendingMatches = await _dashboardRepository.GetPendingMatchesNearestDateAsync(userId);

            return new DashboardDTO
            {
                TotalLeagues = totalLeagues,
                ActiveTeams = activeTeams,
                UpcomingMatches = upcomingMatches,
                RecentLeagues = _mapper.Map<IEnumerable<LeagueDTO>>(recentLeagues),
                //UpcomingMatchesList = upcomingMatchesList
                PendingMatches = _mapper.Map<IEnumerable<MatchDTO>>(pendingMatches)
            };
        }
        //private async Task<IEnumerable<MatchDTO>> GetPendingMatchesNearestDateAsync(string userId)
        //{
        //    var matches = await _dashboardRepository.GetPendingMatchesNearestDateAsync(userId);
        //    return _mapper.Map<IEnumerable<MatchDTO>>(matches);
        //}

        //private async Task<(int totalLeagues, int activeTeams, int upcomingMatches)> GetCountsAsync(string userId)
        //{
        //    var totalLeaguesTask = _dashboardRepository.GetTotalLeaguesCountAsync(userId);
        //    var activeTeamsTask = _dashboardRepository.GetActiveTeamsCountAsync(userId);
        //    var upcomingMatchesTask = _dashboardRepository.GetUpcomingMatchesCountAsync(userId);

        //    await Task.WhenAll(totalLeaguesTask, activeTeamsTask, upcomingMatchesTask);

        //    return (
        //        await totalLeaguesTask,
        //        await activeTeamsTask,
        //        await upcomingMatchesTask
        //    );
        //}

        //private async Task<IEnumerable<LeagueDTO>> GetRecentLeaguesAsync(string userId)
        //{
        //    var leagues = await _dashboardRepository.GetRecentLeaguesAsync(userId, 5);
        //    return _mapper.Map<IEnumerable<LeagueDTO>>(leagues);
        //}

        //private async Task<IEnumerable<MatchDTO>> GetUpcomingMatchesAsync(string userId)
        //{
        //    var matches = await _dashboardRepository.GetUpcomingMatchesAsync(userId, 5);
        //    return _mapper.Map<IEnumerable<MatchDTO>>(matches);
        //}
    }
}