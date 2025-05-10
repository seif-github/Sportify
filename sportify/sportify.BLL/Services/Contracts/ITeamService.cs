using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sportify.BLL.DTOs;

namespace sportify.BLL.Services.Contracts
{
    public interface ITeamService
    {
        Task<List<TeamDTO>> GetAllASync();
        Task<TeamDTO?> GetByIdAsync(int id);
        Task AddTeamsAsync(List<TeamDTO> teams);
        Task UpdateAsync(TeamDTO team);
        Task DeleteAsync(int id);

    }
}
