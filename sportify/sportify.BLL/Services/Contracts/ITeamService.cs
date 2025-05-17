using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sportify.BLL.DTOs;
using sportify.DAL.Entities;

namespace sportify.BLL.Services.Contracts
{
    public interface ITeamService
    {
        Task<List<TeamDTO>> GetAllAsync();
        Task<List<TeamDTO>> GetAllTeamsInLeagueAsync(int leagueId);
        Task<TeamDTO?> GetTeamByIdAsync(int id);
        Task AddTeamsAsync(List<TeamDTO> teams);
        Task<List<TeamDTO>> AddTeamsAndReturnAsync(List<TeamDTO> model);
        Task AddTeamAsync(TeamDTO team);
        Task UpdateAsync(TeamDTO team);
        Task DeleteAsync(int id);
        Task<List<TeamDTO>> SortStandings(int leagueId);

    }
}
