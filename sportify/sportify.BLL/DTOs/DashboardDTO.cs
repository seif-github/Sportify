using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.DTOs
{
    public class DashboardDTO
    {
        public int TotalLeagues { get; set; }
        public int ActiveTeams { get; set; }
        public int UpcomingMatches { get; set; }
        //public int TotalPlayers { get; set; }
        public IEnumerable<LeagueDTO> RecentLeagues { get; set; }
        //public IEnumerable<MatchDTO> UpcomingMatchesList { get; set; }
        public IEnumerable<MatchDTO> PendingMatches { get; set; }
    }
}
