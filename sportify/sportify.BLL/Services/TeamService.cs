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
        private readonly ITeamRepository _teamRepo;
        private readonly IMatchRepository _matchRepo;
        private readonly IMapper _mapper;
        public TeamService(IGenericRepository<Team> genericRepo, ITeamRepository teamRepo, IMatchRepository matchRepo, IMapper mapper)
        {
            _genericRepo = genericRepo;
            _teamRepo = teamRepo;
            _matchRepo = matchRepo;
            _mapper = mapper;
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

        //public async Task<List<TeamDTO>> SortStandings(int leagueId)
        //{
        //    var teams = await GetAllTeamsInLeagueAsync(leagueId);
        //    return teams.OrderByDescending(t => t.Points)
        //        .ThenBy(t => t.Name)
        //        .ToList();
        //}

        public async Task<List<TeamDTO>> UpdateAndSortStandingsAsync(int leagueId)
        {
            var teams = await _teamRepo.GetAllTeamsInLeagueAsync(leagueId);
            var teamDTOs = _mapper.Map<List<TeamDTO>>(teams);

            // reset standings
            foreach (var team in teamDTOs)
            {
                team.Wins = 0;
                team.Losses = 0;
                team.Draws = 0;
                team.GoalsScored = 0;
                team.GoalsConceded = 0;
                team.TotalMatchesPlayed = 0;
                team.Points = 0;
            }

            var matches = await _matchRepo.GetMatchesWithTeamsByLeagueAsync(leagueId);
            var completedMatches = matches.Where(m => m.IsCompleted).ToList();

            foreach (var match in completedMatches)
            {
                var firstTeam = teamDTOs.FirstOrDefault(t => t.TeamID == match.FirstTeamId);
                var secondTeam = teamDTOs.FirstOrDefault(t => t.TeamID == match.SecondTeamId);

                if (firstTeam == null || secondTeam == null) continue;

                firstTeam.GoalsScored += match.FirstTeamGoals;
                firstTeam.GoalsConceded += match.SecondTeamGoals;
                secondTeam.GoalsScored += match.SecondTeamGoals;
                secondTeam.GoalsConceded += match.FirstTeamGoals;

                firstTeam.TotalMatchesPlayed++;
                secondTeam.TotalMatchesPlayed++;

                switch (match.Result)
                {
                    case MatchResult.FirstTeamWin:
                        firstTeam.Wins++;
                        firstTeam.Points += 3;
                        secondTeam.Losses++;
                        break;
                    case MatchResult.SecondTeamWin:
                        secondTeam.Wins++;
                        secondTeam.Points += 3;
                        firstTeam.Losses++;
                        break;
                    case MatchResult.Draw:
                        firstTeam.Draws++;
                        firstTeam.Points += 1;
                        secondTeam.Draws++;
                        secondTeam.Points += 1;
                        break;
                }
            }

            foreach (var teamDTO in teamDTOs)
            {
                var teamEntity = _mapper.Map<Team>(teamDTO);
                await _genericRepo.UpdateAsync(teamEntity);
            }

            var sortedTeams = teamDTOs
            .OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.GoalsScored - t.GoalsConceded)
            .ToList();

            return sortedTeams;

        }
    }
}
