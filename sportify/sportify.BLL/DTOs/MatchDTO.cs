using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string LeagueName { get; set; }
        public int FirstTeamId { get; set; }
        public string FirstTeamName { get; set; }
        public int SecondTeamId { get; set; }
        public string SecondTeamName { get; set; }
        public DateTime Date { get; set; }
        public int FirstTeamGoals { get; set; }
        public int SecondTeamGoals { get; set; }
        public MatchResult Result { get; set; }
        public bool IsCompleted { get; set; }

        [NotMapped]
        public int Hour
        {
            get => Date.Hour;
            set => Date = new DateTime(Date.Year, Date.Month, Date.Day, value, Date.Minute, 0);
        }

        [NotMapped]
        public int Minute
        {
            get => Date.Minute;
            set => Date = new DateTime(Date.Year, Date.Month, Date.Day, Date.Hour, value, 0);
        }
    }
}
