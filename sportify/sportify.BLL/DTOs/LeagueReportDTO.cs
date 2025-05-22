using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.DTOs
{
    public class LeagueReportDTO
    {
        public string LeagueName { get; set; } = string.Empty;
        public string OrganizerId { get; set; } = string.Empty;
        public string OrganizerName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public int NumberOfTeams { get; set; }
        public int DurationBetweenMatches { get; set; }
        public bool RoundRobin { get; set; }
        public string ImageUrl { get; set; }

        public List<TeamDTO> Teams { get; set; } = new();
        public List<MatchDTO> Matches { get; set; } = new();
        public int TotalMatchesPlayed { get; set; }
        public string TopScoringTeam { get; set; } = string.Empty;
        public TopMatch TopScoringMatch { get; set; }
        public string MostGoalsConcededTeam { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
    }

    public class TopMatch
    {
        DateTime MatchDate;
        public string Team1;
        public string Team2;
        public int Goals1;
        public int Goals2;
        public TopMatch(DateTime matchDate, string team1, string team2, int goals1, int goals2)
        {
            MatchDate = matchDate;
            Team1 = team1;
            Team2 = team2;
            Goals1 = goals1;    
            Goals2 = goals2;
        }
    }

}
