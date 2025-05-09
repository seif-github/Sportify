using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;

namespace sportify.BLL.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly IGenericRepository<League> _genericRepository;
        private readonly IMapper _mapper;

        public LeagueService(IGenericRepository<League> genericRepository, IMapper mapper)
        {
            this._genericRepository = genericRepository;
            this._mapper = mapper;
        }

        public async Task<List<LeagueDTO>> GetAllAsync()
        {
            var data = await _genericRepository.GetAllAsync();
            return _mapper.Map<List<LeagueDTO>>(data);
        }

        public async Task<LeagueDTO?> GetByIdAsync(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<LeagueDTO>(entity);
        }

        public async Task AddAsync(LeagueDTO model)
        {
            var entity = _mapper.Map<League>(model);
            await _genericRepository.AddAsync(entity);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeagueDTO model)
        {
            var entity = _mapper.Map<League>(model);
            await _genericRepository.UpdateAsync(entity);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _genericRepository.DeleteAsync(id);
            await _genericRepository.SaveChangesAsync();
        }
    }
}
