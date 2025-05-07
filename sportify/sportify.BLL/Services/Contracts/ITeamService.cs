using sportify.BLL.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.Services.Contracts
{
    public interface ITeamService
    {
        Task<List<TeamModel>> GetAllAsync();
        Task<TeamModel?> GetByIdAsync(int id);
        Task AddAsync(TeamModel teamModel);
        Task UpdateAsync(TeamModel teamModel);
        Task DeleteAsync(int id);
    }
}
