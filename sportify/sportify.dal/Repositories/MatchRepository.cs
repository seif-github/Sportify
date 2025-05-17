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
        private readonly DbSet<Entities.Match> _dbSet;
        public MatchRepository(SportifyContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<Entities.Match>();
        }

        public async Task<List<Match>> GetMatchesWithTeamsByLeagueAsync(int leagueId)
        {
            return await _dbSet.Where(m => m.LeagueId == leagueId)
                .Include(m => m.FirstTeam)
                .Include(m => m.SecondTeam)
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
