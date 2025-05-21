using sportify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.DAL.Repositories.Contracts
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalLeaguesCountAsync(string userId);
        Task<int> GetActiveTeamsCountAsync(string userId);
        Task<int> GetUpcomingMatchesCountAsync(string userId);
        //Task<int> GetTotalPlayersCountAsync(string userId);
        Task<IEnumerable<League>> GetRecentLeaguesAsync(string userId);
        //Task<IEnumerable<Match>> GetUpcomingMatchesAsync(string userId, int count);
        Task<IEnumerable<Match>> GetPendingMatchesNearestDateAsync(string userId);
    }
}
