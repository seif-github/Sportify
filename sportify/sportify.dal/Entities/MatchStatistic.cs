namespace FootballLeague.Models
{
    public class MatchStatistic
    {
        public int Id { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
    }
}