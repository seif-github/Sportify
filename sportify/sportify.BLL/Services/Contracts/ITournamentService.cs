using sportify.BLL.CustomModels;
using sportify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.Services.Contracts
{
    public interface ITournamentService
    {
        Task<List<TournamentModel>> GetAllAsync();
        Task<TournamentModel?> GetByIdAsync(int id);
        Task AddAsync(TournamentModel tournamentModel);
        Task UpdateAsync(TournamentModel tournamentModel);
        Task DeleteAsync(int id);
    }
}
