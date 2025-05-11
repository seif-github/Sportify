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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var data = await _genericRepository.GetByIdAsync(id);
            return data == null ? null : _mapper.Map<LeagueDTO>(data);
        }
        
        public async Task<List<LeagueDTO?>?> GetOrganizerLeaguesById(string organizerId)
        {
            var leagues = await _genericRepository.GetAllAsync();
            var filteredLeagues = leagues.Where(league => league.OrganizerID == organizerId).ToList();
            return leagues == null ? null : _mapper.Map<List<LeagueDTO?>?>(filteredLeagues);
        }

        public async Task AddAsync(LeagueDTO model)
        {
            var entity = _mapper.Map<League>(model);
            await _genericRepository.AddAsync(entity);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task<LeagueDTO> AddAndReturnAsync(LeagueDTO model)
        {
            var entity = _mapper.Map<League>(model);
            await _genericRepository.AddAsync(entity);
            await _genericRepository.SaveChangesAsync();

            // Map the saved entity back to DTO to return
            return _mapper.Map<LeagueDTO>(entity);
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
