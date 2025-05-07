using AutoMapper;
using sportify.BLL.CustomModels;
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
        private readonly IGenericRepository<Tournament> _repository;
        private readonly IMapper _mapper;

        public TournamentService(IGenericRepository<Tournament> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TournamentModel>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return _mapper.Map<List<TournamentModel>>(data);
        }

        public async Task<TournamentModel?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<TournamentModel>(entity);
        }

        public async Task AddAsync(TournamentModel model)
        {
            var entity = _mapper.Map<Tournament>(model);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(TournamentModel model)
        {
            var entity = _mapper.Map<Tournament>(model);
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
        }
    }

}
