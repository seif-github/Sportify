namespace Sportify.Models
{
    public enum SportType {
        Soccer=0,
        BasketBall=1
    }
    public class League
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public SportType SportType { get; set; }
        public string OrganizerID { get; set; }
        public User Organizer { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public List<UserLeague> UserLeagues { get; set; } = new();
        public List<LeagueTeam> LeagueTeams { get; set; } = new();

    }
}
