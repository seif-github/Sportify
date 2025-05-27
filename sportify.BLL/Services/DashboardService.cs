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
        
    }
}