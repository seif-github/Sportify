namespace Sportify.Models
{
    public enum Status
    {
        Postponded=0,
        Running =1,
    }
    public class Match
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public League League { get; set; }

        public int FirstTeamId { get; set; }
        public Team FirstTeam { get; set; }

        public int SecondTeamId { get; set; }
        public Team SecondTeam { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public int FirstTeamScore { get; set; }
        public int SecondTeamScore { get; set; }
        public int? WinnerTeamId { get; set; }
        public Team WinnerTeam { get; set; }
    }
}
