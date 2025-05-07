using AutoMapper;
using sportify.BLL.DTOs;
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
    public class TeamService : ITeamService
    {
        private readonly IGenericRepository<Team> _repository;
        private readonly IMapper _mapper;

        public TeamService(IGenericRepository<Team> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddAsync(TeamDTO teamModel)
        {
            var entity = _mapper.Map<Team>(teamModel);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();        
        }

        public async Task<List<TeamDTO>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return _mapper.Map<List<TeamDTO>>(data);
            
        }

        public async Task<TeamDTO?> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return _mapper.Map<TeamDTO>(data);
        }

        public async Task UpdateAsync(TeamDTO teamModel)
        {
            var entity = _mapper.Map<Team>(teamModel);
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
