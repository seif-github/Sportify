using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.DAL.Entities
{
    public class Match
    {
        public int MatchID { get; set; }

        [ForeignKey("League")]
        public int LeagueId { get; set; }
        public League League { get; set; } = null!;

        [ForeignKey("FirstTeam")]
        public int FirstTeamId { get; set; }
        public Team FirstTeam { get; set; } = null!;

        [ForeignKey("SecondTeam")]
        public int SecondTeamId { get; set; }
        public Team SecondTeam { get; set; } = null!;
        public DateTime Date { get; set; }
        public int FirstTeamGoals { get; set; }
        public int SecondTeamGoals { get; set; }
        public bool IsCompleted { get; set; }
        // This will be stored as an integer in the database (0, 1, 2, 3)
        public MatchResult Result { get; set; }
    }

    public enum MatchResult
    {
        Pending = 0,
        FirstTeamWin = 1,
        SecondTeamWin = 2,
        Draw = 3
    }
}
