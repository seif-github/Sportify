using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class Match
    {
        public int Id { get; set; }

        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public DateTime MatchDate { get; set; }

        public ICollection<MatchStatistic> MatchStatistics { get; set; }
        public ICollection<PlayerStatistic> PlayerStatistics { get; set; }
    }
}