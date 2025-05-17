using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sportify.DAL.Entities;

namespace sportify.BLL.DTOs
{
    public class MatchDTO
    {
        public int MatchID { get; set; }
        public int LeagueId { get; set; }
        public int FirstTeamId { get; set; }
        public string FirstTeamName { get; set; } // Add this
        public int SecondTeamId { get; set; }
        public string SecondTeamName { get; set; } // Add this
        public DateTime Date { get; set; }
        public int FirstTeamGoals { get; set; }
        public int SecondTeamGoals { get; set; }
        public MatchResult Result { get; set; }
        public bool IsCompleted { get; set; }
    }
}
