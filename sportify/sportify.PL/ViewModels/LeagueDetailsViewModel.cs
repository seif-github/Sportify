using sportify.BLL.DTOs;

namespace sportify.PL.ViewModels
{
    public class LeagueDetailsViewModel
    {
        public LeagueDTO League { get; set; }
        public List<TeamDTO> Teams { get; set; }
        public List<MatchDTO> Matches { get; set; }
        public Dictionary<int, List<MatchDTO>> MatchesByRound { get; set; } = new Dictionary<int, List<MatchDTO>>();
        public int CurrentRound { get; set; }
        public string NewTeamName { get; set; } = string.Empty;
        public bool IsOrganizer { get; internal set; }
        public string OrganizerName { get; set; }
    }
}
