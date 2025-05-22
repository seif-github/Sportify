using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Data;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace sportify.BLL.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeagueService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<LeagueDTO>> GetAllAsync()
        {
            var data = await _unitOfWork.LeagueRepository.GetAllAsync();
            return _mapper.Map<List<LeagueDTO>>(data);
        }
        public async Task<LeagueDTO?> GetByIdAsync(int id)
        {
            var data = await _unitOfWork.LeagueRepository.GetByIdAsync(id);
            return data == null ? null : _mapper.Map<LeagueDTO>(data);
        }
        public async Task<string?> GetOrganizerIdByLeagueId(int leagueId)
        {
            var league = await _unitOfWork.LeagueRepository.GetByIdAsync(leagueId);
            return league?.OrganizerID;
        }
        public async Task<List<LeagueDTO?>?> GetOrganizerLeaguesById(string organizerId)
        {
            var leagues = await _unitOfWork.LeagueRepository.GetAllAsync();
            var filteredLeagues = leagues.Where(league => league.OrganizerID == organizerId).ToList();
            return leagues == null ? null : _mapper.Map<List<LeagueDTO?>?>(filteredLeagues);
        }

        public async Task AddAsync(LeagueDTO model)
        {
            var entity = _mapper.Map<League>(model);
            await _unitOfWork.LeagueRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<LeagueDTO> AddAndReturnAsync(LeagueDTO model)
        {
            var entity = _mapper.Map<League>(model);
            await _unitOfWork.LeagueRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<LeagueDTO>(entity);
        }

        public async Task UpdateAsync(LeagueDTO model)
        {
            var entity = _mapper.Map<League>(model);
            _unitOfWork.LeagueRepository.Update(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.LeagueRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<LeagueReportDTO?> GetLeagueReportDataAsync(int leagueId)
        {
            var league = await _unitOfWork.LeagueRepository.GetLeagueWithTeamsAsync(leagueId);
            if (league == null || league.Teams == null) return null;

            var matches = await _unitOfWork.MatchRepository.GetMatchesWithTeamsByLeagueAsync(leagueId);
            int totalMatchesPlayed = matches.Count(m => m.IsCompleted);
            var matchesDto = matches.Select(m => new MatchDTO
            {
                Date = m.Date,
                FirstTeamName = m.FirstTeam?.Name ?? "Unknown",
                SecondTeamName = m.SecondTeam?.Name ?? "Unknown",
                FirstTeamGoals = m.FirstTeamGoals,
                SecondTeamGoals = m.SecondTeamGoals,
                IsCompleted = m.IsCompleted
            }).OrderBy(m => m.Date).ToList();

            var teams = await _unitOfWork.TeamRepository.GetAllTeamsInLeagueAsync(leagueId);
            var teamsDto = teams.Select(t => new TeamDTO
            {
                ImageUrl = t.ImageUrl,
                Name = t.Name,
                Wins = t.Wins,
                Losses = t.Losses,
                Draws = t.Draws,
                GoalsScored = t.GoalsScored,
                GoalsConceded = t.GoalsConceded,
                Points = t.Points
            }).OrderByDescending(x => x.Points).ToList();

            var topScoringTeam = league.Teams.OrderByDescending(t => t.GoalsScored).FirstOrDefault();
            var mostGoalsConcededTeam = league.Teams.OrderByDescending(t => t.GoalsConceded).FirstOrDefault();

            var topMatch = matches
                .Where(m => m.IsCompleted)
                .OrderByDescending(m => (m.FirstTeamGoals) + (m.SecondTeamGoals))
                .Select(m => new {
                    MatchDate = m.Date,
                    HomeTeam = m.FirstTeam?.Name ?? "Unknown",
                    AwayTeam = m.SecondTeam?.Name ?? "Unknown",
                    HomeScore = m.FirstTeamGoals,
                    AwayScore = m.SecondTeamGoals,
                    TotalGoals = (m.FirstTeamGoals) + (m.SecondTeamGoals)
                })
                .FirstOrDefault();

            var TopMatch = topMatch != null
                ? new TopMatch(
                    topMatch.MatchDate,
                    topMatch.HomeTeam,
                    topMatch.AwayTeam,
                    topMatch.HomeScore,
                    topMatch.AwayScore)
                : new TopMatch(
                    DateTime.MinValue,
                    "N/A",
                    "N/A",
                    0,
                    0);

            var organizerUser = await _unitOfWork.UserRepository.GetUserById(league.OrganizerID);
            var organizerName = organizerUser?.UserName ?? "Unknown";

            return new LeagueReportDTO
            {
                ImageUrl = league.ImageUrl,
                LeagueName = league.Name,
                OrganizerId = league.OrganizerID,
                OrganizerName = organizerName,
                StartDate = league.StartDate,
                NumberOfTeams = league.NumberOfTeams,
                DurationBetweenMatches = league.DurationBetweenMatches,
                RoundRobin = league.RoundRobin,
                Teams = teamsDto,
                Matches = matchesDto,
                TotalMatchesPlayed = totalMatchesPlayed,
                TopScoringTeam = topScoringTeam?.Name ?? "N/A",
                TopScoringMatch = TopMatch,
                MostGoalsConcededTeam = mostGoalsConcededTeam?.Name ?? "N/A",
                GeneratedAt = DateTime.Now
            };
        }

        public void ClearTracking()
        {
            _unitOfWork.ClearTracking();
        }

    }
}
