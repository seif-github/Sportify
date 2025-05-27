using sportify.BLL.DTOs;

namespace sportify.BLL.Services.Contracts
{
    public interface ILeagueTeamCountUpdateService
    {
        Task UpdateTeamCountAsync(LeagueTeamCountUpdateDTO model);
    }
}