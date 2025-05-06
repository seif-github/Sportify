using sportify.BLL.Services.Contracts;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _repository;

        public TournamentService(ITournamentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Tournament>> GetAllTournamentsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Tournament?> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

        public async Task AddAsync(Tournament tournament)
        => await _repository.AddAsync(tournament);

        public async Task UpdateAsync(Tournament tournament)
            => await _repository.UpdateAsync(tournament);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);
    }

}
