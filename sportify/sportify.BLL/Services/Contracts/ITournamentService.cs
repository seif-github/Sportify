using sportify.BLL.DTOs;
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
        Task<List<TournamentDTO>> GetAllAsync();
        Task<TournamentDTO?> GetByIdAsync(int id);
        Task AddAsync(TournamentDTO tournamentModel);
        Task UpdateAsync(TournamentDTO tournamentModel);
        Task DeleteAsync(int id);
    }
}
