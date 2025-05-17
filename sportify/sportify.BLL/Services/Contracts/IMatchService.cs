using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sportify.BLL.DTOs;

namespace sportify.BLL.Services.Contracts
{
    public interface IMatchService
    {
        Task<List<MatchDTO?>> GetMatchesByLeagueIdAsync(int id);
        Task AddMatchesAsync(List<MatchDTO> matches);
        Task UpdateAsync(MatchDTO model);
    }
}
