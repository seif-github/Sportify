using Microsoft.EntityFrameworkCore;
using sportify.DAL.Data;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.DAL.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly SportifyContext _context;
        private readonly DbSet<Team> _dbSet;
        public TeamRepository(SportifyContext context)
        {
            _context = context;
            _dbSet = context.Set<Team>();
                       
        }
        public async Task<List<Team>> GetAllTeamsInLeagueAsync(int leagueId)
        {
            return await _dbSet.Where(m => m.LeagueID == leagueId).ToListAsync();
        }
    }
}
