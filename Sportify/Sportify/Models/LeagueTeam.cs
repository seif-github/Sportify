namespace Sportify.Models
{
    public class LeagueTeam
    {
        public int LeagueId { get; set; }
        public League League { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}