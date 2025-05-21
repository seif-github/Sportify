using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Data;
using sportify.DAL.Repositories.Contracts;
using System.Threading.Tasks;

namespace sportify.BLL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DashboardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DashboardDTO> GetDashboardDataAsync(string userId)
        {
            var totalLeagues = await _unitOfWork.DashboardRepository.GetTotalLeaguesCountAsync(userId);
            var activeTeams = await _unitOfWork.DashboardRepository.GetActiveTeamsCountAsync(userId);
            var upcomingMatches = await _unitOfWork.DashboardRepository.GetUpcomingMatchesCountAsync(userId);
            var recentLeagues = await _unitOfWork.DashboardRepository.GetRecentLeaguesAsync(userId);
            var pendingMatches = await _unitOfWork.DashboardRepository.GetPendingMatchesNearestDateAsync(userId);

            return new DashboardDTO
            {
                TotalLeagues = totalLeagues,
                ActiveTeams = activeTeams,
                UpcomingMatches = upcomingMatches,
                RecentLeagues = _mapper.Map<IEnumerable<LeagueDTO>>(recentLeagues),
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