namespace Sportify.Models
{
    public class UserLeague
    {
        public string UserId { get; set; }  
        public User User { get; set; }

        public int LeagueId { get; set; }
        public League League { get; set; }
    }
}