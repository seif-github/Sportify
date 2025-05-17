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

            // For odd number of teams, add a "dummy" team (represented as -1)
            // This team will be paired with the team that sits out each round
            if (isOddNumberOfTeams)
            {
                teamIds.Add(-1); // -1 represents a "bye" (no match)
            }

            int numberOfTeams = teamIds.Count; // Including dummy team if odd
            int rounds = numberOfTeams - 1;    // N-1 rounds needed for N teams (including dummy)
            int totalRounds = league.RoundRobin ? rounds * 2 : rounds;
            DateTime currentDate = league.StartDate;

            // List to track round numbers - we don't directly add this to MatchDTO
            // but we'll use it to ensure rounds are properly tracked in our view
            var matchRounds = new Dictionary<int, int>(); // matchId -> roundNumber
            int matchId = 0;

            // Generate round-robin schedule
            for (int round = 0; round < totalRounds; round++)
            {
                bool isSecondRoundRobin = round >= rounds;
                int displayRound = isSecondRoundRobin ? round - rounds + 1 : round + 1;

                // Generate matches for this round
                for (int i = 0; i < numberOfTeams / 2; i++)
                {
                    int homeTeamIndex = i;
                    int awayTeamIndex = numberOfTeams - 1 - i;

                    // Skip if either team is the dummy (-1)
                    if (teamIds[homeTeamIndex] == -1 || teamIds[awayTeamIndex] == -1)
                        continue;

                    int firstTeamId, secondTeamId;

                    // For second round-robin, swap home/away teams
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

                    matches.Add(new MatchDTO
                    {
                        LeagueId = league.LeagueID,
                        FirstTeamId = firstTeamId,
                        SecondTeamId = secondTeamId,
                        Date = currentDate,
                        FirstTeamGoals = 0,
                        SecondTeamGoals = 0,
                        Result = MatchResult.Pending,
                        IsCompleted = false
                        // No RoundNumber property
                    });

                    // Store round number in our local dictionary - we'll use this in our view
                    matchRounds[matchId++] = displayRound;
                }

                // Rotate teams for next round (except last round)
                if (round < totalRounds - 1)
                {
                    // Standard circle method: first position is fixed, all others rotate
                    var firstElement = teamIds[0];
                    var secondElement = teamIds[1];

                    for (int i = 2; i < numberOfTeams; i++)
                    {
                        teamIds[i - 1] = teamIds[i];
                    }

                    teamIds[numberOfTeams - 1] = secondElement;

                    // Move to next date for the next round
                    // If we're transitioning to second round-robin, consider adding a longer break
                    if ((round + 1) % rounds != 0 || !league.RoundRobin)
                    {
                        currentDate = currentDate.AddDays(league.DurationBetweenMatches);
                    }
                    else if (league.RoundRobin)
                    {
                        // Optional: Add longer break between round-robin cycles
                        currentDate = currentDate.AddDays(league.DurationBetweenMatches * 2);
                    }
                }
            }

            // We can add round information to ViewData in the controller
            // ViewData["MatchRounds"] = matchRounds;

            return matches;
        }
    }
}