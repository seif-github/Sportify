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

        public List<TeamDTO> Teams { get; set; } = new();
        public int TotalMatchesPlayed { get; set; }
        public string TopScoringTeam { get; set; } = string.Empty;
        public string MostGoalsConcededTeam { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
    }

}
