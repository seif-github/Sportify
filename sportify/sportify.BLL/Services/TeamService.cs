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
    public class TeamService : ITeamService
    {
        private readonly IGenericRepository<Team> _genericRepo;
        private readonly IMapper _mapper;
        public TeamService(IGenericRepository<Team> genericRepo, IMapper mapper)
        {
            this._genericRepo = genericRepo;
            this._mapper = mapper;
        }

        public Task<List<TeamDTO>> GetAllASync()
        {
            throw new NotImplementedException();
        }

        public Task<TeamDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TeamDTO team)
        {
            throw new NotImplementedException();
        }

        public async Task AddTeamsAsync(List<TeamDTO> teams)
        {
            var entity = _mapper.Map<List<Team>>(teams);
            foreach (var team in entity)
            {
                await _genericRepo.AddAsync(team);
            }
            await _genericRepo.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
