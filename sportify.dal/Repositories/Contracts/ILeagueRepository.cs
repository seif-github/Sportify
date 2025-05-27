using sportify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.DAL.Repositories.Contracts
{
    public interface ILeagueRepository: IGenericRepository<League>
    {
        Task<League?> GetLeagueWithTeamsAsync(int leagueId);
    }
}
