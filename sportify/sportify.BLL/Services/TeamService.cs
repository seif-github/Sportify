using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Entities;
using sportify.DAL.Repositories;
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

        public async Task<List<TeamDTO>> GetAllAsync()
        {
            var entity = await _genericRepo.GetAllAsync();
            return _mapper.Map<List<TeamDTO>>(entity);

        }

        public async Task<List<TeamDTO>> GetAllTeamsInLeagueAsync(int leagueId)
        {
            var entity = await _genericRepo.GetAllAsync();
            var teams = entity.Where(i => i.LeagueID == leagueId);
            return entity == null ?
                    null :
                    _mapper.Map<List<TeamDTO>>(teams);
        }

        public async Task<TeamDTO?> GetTeamByIdAsync(int id)
        {
            var data = await _genericRepo.GetByIdAsync(id);
            return data == null ? null : _mapper.Map<TeamDTO>(data);
        }

        public async Task UpdateAsync(TeamDTO team)
        {
            var entity = _mapper.Map<Team>(team);
            await _genericRepo.UpdateAsync(entity);
            await _genericRepo.SaveChangesAsync();
        }

        public async Task UpdateTeamNameAsync(int teamId, string newName)
        {
            // 1. Get existing team
            var team = await _genericRepo.GetByIdAsync(teamId);
            if (team == null)
            {
                throw new KeyNotFoundException("Team not found");
            }

            // 2. Only update the name
            team.Name = newName;

            // 3. Save changes
            await _genericRepo.UpdateAsync(team);
            await _genericRepo.SaveChangesAsync();
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

        public async Task<List<TeamDTO>> AddTeamsAndReturnAsync(List<TeamDTO> model)
        {
            var entity = _mapper.Map<List<Team>>(model);
            await _genericRepo.AddRangeAsync(entity);
            await _genericRepo.SaveChangesAsync();

            // Map the saved entity back to DTO to return
            return _mapper.Map<List<TeamDTO>>(entity);
        }

        public async Task AddTeamAsync(TeamDTO team)
        {
            var entity = _mapper.Map<Team>(team);
            await _genericRepo.AddAsync(entity);
            await _genericRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _genericRepo.DeleteAsync(id);
            await _genericRepo.SaveChangesAsync();
        }

        public async Task<List<TeamDTO>> SortStandings(int leagueId)
        {
            var teams = await GetAllTeamsInLeagueAsync(leagueId);
            return teams.OrderByDescending(t => t.Points)
                .ThenBy(t => t.Name)
                .ToList();
        }
    }
}
