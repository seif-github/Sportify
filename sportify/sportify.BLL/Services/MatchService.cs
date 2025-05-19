using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;
using Match = sportify.DAL.Entities.Match;

namespace sportify.BLL.Services
{
    public class MatchService : IMatchService
    {
        private readonly IGenericRepository<Match> _genericRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public MatchService(IGenericRepository<Match> genericRepository,
            IMatchRepository matchRepository, ITeamService teamService, IMapper mapper)
        {
            this._genericRepository = genericRepository;
            this._matchRepository = matchRepository;
            this._teamService = teamService;
            this._mapper = mapper;
        }
        public async Task<List<MatchDTO>> GetMatchesByLeagueIdAsync(int id)
        {
            var matches = await _matchRepository.GetMatchesWithTeamsByLeagueAsync(id);

            return matches.Select(m => new MatchDTO
            {
                MatchID = m.MatchID,
                LeagueId = m.LeagueId,
                FirstTeamId = m.FirstTeamId,
                FirstTeamName = m.FirstTeam?.Name ?? "Unknown Team",
                SecondTeamId = m.SecondTeamId,
                SecondTeamName = m.SecondTeam?.Name ?? "Unknown Team",
                Date = m.Date,
                FirstTeamGoals = m.FirstTeamGoals,
                SecondTeamGoals = m.SecondTeamGoals,
                Result = m.Result,
                IsCompleted = m.IsCompleted
            }).ToList();
        }

        public async Task UpdateAsync(MatchDTO model)
        {
            var entity = _mapper.Map<Match>(model);
            await _genericRepository.UpdateAsync(entity);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task AddMatchesAsync(List<MatchDTO> matches)
        {
            var matchEntities = _mapper.Map<List<Match>>(matches);
            foreach (Match match in matchEntities)
            {
                match.FirstTeam = null;
                match.SecondTeam = null;
            }
            await _genericRepository.AddRangeAsync(matchEntities);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task DeleteAllMatchesAsync(int id)
        {
            await _matchRepository.DeleteAllMatchesAsync(id);
        }

        public async Task UpdateMatchResultAsync(int matchId, int firstTeamGoals, int secondTeamGoals)
        {
            var match = await _genericRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new Exception("Match not found");

            match.FirstTeamGoals = firstTeamGoals;
            match.SecondTeamGoals = secondTeamGoals;
            match.IsCompleted = true;

            if (firstTeamGoals > secondTeamGoals)
                match.Result = MatchResult.FirstTeamWin;
            else if (secondTeamGoals > firstTeamGoals)
                match.Result = MatchResult.SecondTeamWin;
            else
                match.Result = MatchResult.Draw;

            await _genericRepository.UpdateAsync(match);
            await _genericRepository.SaveChangesAsync();

            await _teamService.UpdateAndSortStandingsAsync(match.LeagueId);
        }
    }
}
