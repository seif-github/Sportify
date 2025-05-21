using Microsoft.EntityFrameworkCore;
using sportify.DAL.Data;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sportify.DAL.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly SportifyContext _context;

        public DashboardRepository(SportifyContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalLeaguesCountAsync(string userId)
        {
            return await _context.Set<League>()
                .Where(l => l.OrganizerID == userId)
                .CountAsync();
        }

        public async Task<int> GetActiveTeamsCountAsync(string userId)
        {
            return await _context.Set<Team>()
                .Include(t => t.League)
                .Where(t => t.League.OrganizerID == userId)
                .CountAsync();
        }

        public async Task<int> GetUpcomingMatchesCountAsync(string userId)
        {
            return await _context.Set<Match>()
                .Include(m => m.League)
                .Where(m => m.League.OrganizerID == userId && m.Date > DateTime.Now)
                .CountAsync();
        }

        //public async Task<int> GetTotalPlayersCountAsync(string userId)
        //{
        //    return await _context.Players
        //        .Include(p => p.Team)
        //        .ThenInclude(t => t.League)
        //        .Where(p => p.Team.League.OrganizerID == userId)
        //        .CountAsync();
        //}

        public async Task<IEnumerable<League>> GetRecentLeaguesAsync(string userId)
        {
            return await _context.Set<League>()
                .Where(l => l.OrganizerID == userId)
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();
        }

        //public async Task<IEnumerable<Match>> GetUpcomingMatchesAsync(string userId, int count)
        //{
        //    return await _context.Matches
        //        .Include(m => m.League)
        //        .Include(m => m.FirstTeam)
        //        .Include(m => m.SecondTeam)
        //        .Where(m => m.League.OrganizerID == userId && m.Date > DateTime.Now)
        //        .OrderBy(m => m.Date)
        //        .Take(count)
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<Match>> GetPendingMatchesNearestDateAsync(string userId)
        {
            return await _context.Matches
                .Include(m => m.League)
                .Include(m => m.FirstTeam)
                .Include(m => m.SecondTeam)
                .Where(m => m.League.OrganizerID == userId && !m.IsCompleted)
                .OrderBy(m => m.Date)
                .Take(3)
                .ToListAsync();
        }
    }
}