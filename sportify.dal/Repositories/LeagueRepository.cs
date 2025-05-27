using sportify.DAL.Data;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.DAL.Repositories
{
    public class LeagueRepository : GenericRepository<League>, ILeagueRepository
    {
        public LeagueRepository(DbContext context) : base(context) { }

        public async Task<League?> GetLeagueWithTeamsAsync(int leagueId)
        {
            return await _context.Set<League>()
                .Include(l => l.Teams)
                .FirstOrDefaultAsync(l => l.LeagueID == leagueId);
        }
    }
}
