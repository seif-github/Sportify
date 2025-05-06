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
        Task<IEnumerable<Tournament>> GetAllTournamentsAsync();
        Task<Tournament?> GetByIdAsync(int id);
        Task AddAsync(Tournament tournament);
        Task UpdateAsync(Tournament tournament);
        Task DeleteAsync(int id);
    }
}
