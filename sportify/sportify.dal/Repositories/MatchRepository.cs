using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sportify.DAL.Data;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;

namespace sportify.DAL.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly SportifyContext _context;
        private readonly DbSet<Match> _dbSet;
        public MatchRepository(SportifyContext context)
        {
            _context = context;
            _dbSet = _context.Set<Match>();
        }

        public async Task<List<Match>> GetMatchesWithTeamsByLeagueAsync(int leagueId)
        {
            return await _context.Matches
                .Include(m => m.FirstTeam)
                .Include(m => m.SecondTeam)
                .Where(m => m.LeagueId == leagueId)
                .ToListAsync();
        }

        public async Task DeleteAllMatchesAsync(int leagueId) // id -> leagueId
        {
            await _dbSet.Where(m => m.LeagueId == leagueId).ExecuteDeleteAsync();
            //var matches = await _dbSet.Where(m => m.LeagueId == leagueId).ToListAsync();
            //_dbSet.RemoveRange(matches);
            //await _context.SaveChangesAsync();
        }
    }
}
