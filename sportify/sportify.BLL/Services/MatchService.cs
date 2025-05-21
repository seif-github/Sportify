using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Data;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;
using Match = sportify.DAL.Entities.Match;

namespace sportify.BLL.Services
{
    public class MatchService : IMatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public MatchService(IUnitOfWork unitOfWork, ITeamService teamService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _teamService = teamService;
            _mapper = mapper;
        }

        public async Task<MatchDTO?> GetMatchByIdAsync(int id)
        {
            var data = await _unitOfWork.MatchRepository.GetByIdAsync(id);
            return data == null ? null : _mapper.Map<MatchDTO>(data);
        }
        public async Task<List<MatchDTO>> GetMatchesByLeagueIdAsync(int id) // id -> league id
        {
            var matches = await _unitOfWork.MatchRepository.GetMatchesWithTeamsByLeagueAsync(id);
            return matches.Select(m => _mapper.Map<MatchDTO>(m)).ToList();
            //return matches.Select(m => new MatchDTO
            //{
            //    MatchID = m.MatchID,
            //    LeagueId = m.LeagueId,
            //    FirstTeamId = m.FirstTeamId,
            //    FirstTeamName = m.FirstTeam?.Name ?? "Unknown Team",
            //    SecondTeamId = m.SecondTeamId,
            //    SecondTeamName = m.SecondTeam?.Name ?? "Unknown Team",
            //    Date = m.Date,
            //    FirstTeamGoals = m.FirstTeamGoals,
            //    SecondTeamGoals = m.SecondTeamGoals,
            //    Result = m.Result,
            //    IsCompleted = m.IsCompleted
            //}).ToList();
        }

        public async Task UpdateAsync(MatchDTO model)
        {
            var entity = _mapper.Map<Match>(model);
            _unitOfWork.MatchRepository.Update(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task AddMatchesAsync(List<MatchDTO> matches)
        {
            var matchEntities = _mapper.Map<List<Match>>(matches);
            foreach (var match in matchEntities)
            {
                match.FirstTeam = null;
                match.SecondTeam = null;
            }
            await _unitOfWork.MatchRepository.AddRangeAsync(matchEntities);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAllMatchesAsync(int id)
        {
            await _unitOfWork.MatchRepository.DeleteAllMatchesAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateMatchResultAsync(int matchId, int firstTeamGoals, int secondTeamGoals)
        {

            var match = await _unitOfWork.MatchRepository.GetByIdAsync(matchId);
            if (match == null) throw new Exception("Match not found");

            match.FirstTeamGoals = firstTeamGoals;
            match.SecondTeamGoals = secondTeamGoals;
            match.IsCompleted = true;
            match.Result = DetermineResult(firstTeamGoals, secondTeamGoals);

            _unitOfWork.MatchRepository.Update(match);
            await _unitOfWork.CompleteAsync();

            await _teamService.UpdateAndSortStandingsAsync(match.LeagueId);
            await _unitOfWork.CompleteAsync();
        }
        private MatchResult DetermineResult(int firstTeamGoals, int secondTeamGoals)
        {
            if (firstTeamGoals > secondTeamGoals) return MatchResult.FirstTeamWin;
            if (secondTeamGoals > firstTeamGoals) return MatchResult.SecondTeamWin;
            return MatchResult.Draw;
        }

        public void ClearTracking()
        {
            _unitOfWork.ClearTracking();
        }
    }
}
