namespace Sportify.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string CoachName { get; set; }
        public int Ranking { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<LeagueTeam> LeagueTeams { get; set; } = new();
    }
}
