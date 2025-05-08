using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.DTOs
{
    public class MatchDTO
    {
        public int MatchID { get; set; }
        public int LeagueId { get; set; }
        public int FirstTeamId { get; set; }
        public int SecondTeamId { get; set; }
        public DateTime Date { get; set; }
        public int FirstTeamGoals { get; set; }
        public int SecondTeamGoals { get; set; }
        public int? WinnerId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
