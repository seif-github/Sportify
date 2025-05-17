using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.DAL.Repositories.Contracts
{
    public interface IMatchRepository
    {
        Task<List<Entities.Match>> GetMatchesWithTeamsByLeagueAsync(int leagueId);
        Task DeleteAllMatchesAsync(int leagueId);
    }
}
