using sportify.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.Services.Contracts
{
    public interface ITeamService
    {
        Task<List<TeamDTO>> GetAllAsync();
        Task<TeamDTO?> GetByIdAsync(int id);
        Task AddAsync(TeamDTO teamModel);
        Task UpdateAsync(TeamDTO teamModel);
        Task DeleteAsync(int id);
    }
}
