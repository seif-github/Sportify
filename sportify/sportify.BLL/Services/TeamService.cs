using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Data;
using sportify.DAL.Entities;
using sportify.DAL.Repositories;
using sportify.DAL.Repositories.Contracts;

namespace sportify.BLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TeamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TeamDTO>> GetAllAsync()
        {
            var entity = await _unitOfWork.TeamRepository.GetAllAsync();
            return _mapper.Map<List<TeamDTO>>(entity);

        }

        public async Task<List<TeamDTO>> GetAllTeamsInLeagueAsync(int leagueId)
        {
            var teams = await _unitOfWork.TeamRepository.GetAllTeamsInLeagueAsync(leagueId);
            return _mapper.Map<List<TeamDTO>>(teams);
        }

        public async Task<TeamDTO?> GetTeamByIdAsync(int id)
        {
            var data = await _unitOfWork.TeamRepository.GetByIdAsync(id);
            return data == null ? null : _mapper.Map<TeamDTO>(data);
        }

        public async Task AddTeamsAsync(List<TeamDTO> teams)
        {
            var entities = _mapper.Map<List<Team>>(teams);
            await _unitOfWork.TeamRepository.AddRangeAsync(entities);
            await _unitOfWork.CompleteAsync();
        }
        public async Task<List<TeamDTO>> AddTeamsAndReturnAsync(List<TeamDTO> model)
        {
            var entity = _mapper.Map<List<Team>>(model);
            await _unitOfWork.TeamRepository.AddRangeAsync(entity);
            await _unitOfWork.CompleteAsync();

            // Map the saved entity back to DTO to return
            return _mapper.Map<List<TeamDTO>>(entity);
        }
        public async Task AddTeamAsync(TeamDTO team)
        {
            var entity = _mapper.Map<Team>(team);
            await _unitOfWork.TeamRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(TeamDTO team)
        {
            var entity = _mapper.Map<Team>(team);
            _unitOfWork.TeamRepository.Update(entity);
            await _unitOfWork.CompleteAsync();
        }

        //public async Task UpdateTeamNameAsync(int teamId, string newName)
        //{
        //    // 1. Get existing team
        //    var team = await _genericRepo.GetByIdAsync(teamId);
        //    if (team == null)
        //    {
        //        throw new KeyNotFoundException("Team not found");
        //    }

        //    // 2. Only update the name
        //    team.Name = newName;

        //    // 3. Save changes
        //    await _genericRepo.UpdateAsync(team);
        //    await _genericRepo.SaveChangesAsync();
        //}



        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.TeamRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
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
            var teams = await _unitOfWork.TeamRepository.GetAllTeamsInLeagueAsync(leagueId);
            //var teamDTOs = _mapper.Map<List<TeamDTO>>(teams);

            var matches = await _unitOfWork.MatchRepository.GetMatchesWithTeamsByLeagueAsync(leagueId);
            var completedMatches = matches.Where(m => m.IsCompleted).ToList();

            // reset standings
            foreach (var team in teams)
            {
                team.Wins = 0;
                team.Losses = 0;
                team.Draws = 0;
                team.GoalsScored = 0;
                team.GoalsConceded = 0;
                team.TotalMatchesPlayed = 0;
                team.Points = 0;
            }


            foreach (var match in completedMatches)
            {
                var firstTeam = teams.FirstOrDefault(t => t.TeamID == match.FirstTeamId);
                var secondTeam = teams.FirstOrDefault(t => t.TeamID == match.SecondTeamId);

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

            foreach (var team in teams)
            {
                //var teamEntity = _mapper.Map<Team>(teamDTO);
                _unitOfWork.TeamRepository.Update(team);
            }

            await _unitOfWork.CompleteAsync();

            var sortedTeams = teams
            .OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.GoalsScored - t.GoalsConceded)
            .Select(t => _mapper.Map<TeamDTO>(t))
            .ToList();

            return sortedTeams;

        }

        public void ClearTracking()
        {
            _unitOfWork.ClearTracking();
        }
    }
}
