using sportify.BLL.DTOs;
using sportify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sportify.BLL.Helpers
{
    public static class MatchGenerator
    {
        public static List<MatchDTO> GenerateMatches(LeagueDTO league, List<TeamDTO> teams)
        {
            var matches = new List<MatchDTO>();
            var teamIds = teams.Select(t => t.TeamID).ToList();
            bool isOddNumberOfTeams = teamIds.Count % 2 != 0;
            var random = new Random();

            if (isOddNumberOfTeams)
            {
                teamIds.Add(-1); // -1 represents a "bye" (no match)
            }

            int numberOfTeams = teamIds.Count;
            int rounds = numberOfTeams - 1;
            int totalRounds = league.RoundRobin ? rounds * 2 : rounds;
            DateTime currentDate = league.StartDate;


            var matchRounds = new Dictionary<int, int>(); // matchId -> roundNumber
            int matchId = 0;

            for (int round = 0; round < totalRounds; round++)
            {
                bool isSecondRoundRobin = round >= rounds;
                int displayRound = isSecondRoundRobin ? round - rounds + 1 : round + 1;

                for (int i = 0; i < numberOfTeams / 2; i++)
                {
                    int homeTeamIndex = i;
                    int awayTeamIndex = numberOfTeams - 1 - i;

                    if (teamIds[homeTeamIndex] == -1 || teamIds[awayTeamIndex] == -1)
                        continue;

                    int firstTeamId, secondTeamId;

                    if (isSecondRoundRobin && league.RoundRobin)
                    {
                        firstTeamId = teamIds[awayTeamIndex];
                        secondTeamId = teamIds[homeTeamIndex];
                    }
                    else
                    {
                        firstTeamId = teamIds[homeTeamIndex];
                        secondTeamId = teamIds[awayTeamIndex];
                    }

                    int hour = random.Next(15, 22); // (3 PM to 10 PM)
                    int minute = random.Next(0, 4) * 15; // 0, 15, 30, or 45 minutes

                    DateTime matchDateTime = new DateTime(
                        currentDate.Year,
                        currentDate.Month,
                        currentDate.Day,
                        hour, minute, 0);

                    matches.Add(new MatchDTO
                    {
                        LeagueId = league.LeagueID,
                        FirstTeamId = firstTeamId,
                        SecondTeamId = secondTeamId,
                        Date = matchDateTime,
                        FirstTeamGoals = 0,
                        SecondTeamGoals = 0,
                        Result = MatchResult.Pending,
                        IsCompleted = false
                    });

                    matchRounds[matchId++] = displayRound;
                }

                if (round < totalRounds - 1)
                {
                    var firstElement = teamIds[0];
                    var secondElement = teamIds[1];

                    for (int i = 2; i < numberOfTeams; i++)
                    {
                        teamIds[i - 1] = teamIds[i];
                    }

                    teamIds[numberOfTeams - 1] = secondElement;

                    if ((round + 1) % rounds != 0 || !league.RoundRobin)
                    {
                        currentDate = currentDate.AddDays(league.DurationBetweenMatches);
                    }
                    else if (league.RoundRobin)
                    {
                        currentDate = currentDate.AddDays(league.DurationBetweenMatches * 2);
                    }
                }
            }


            return matches;
        }
    }
}